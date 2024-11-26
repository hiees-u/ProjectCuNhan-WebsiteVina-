go 
--create role Moderator
Create ROLE Moderator;
-- R -- O -- L -- E --
Grant Select On dbo.Users to Moderator
Grant Select, Update, Insert On dbo.UserInfo to Moderator
Grant Select On dbo.Province to Moderator
Grant Select, Insert On dbo.District to Moderator
Grant Select, Insert On dbo.Commune to Moderator
Grant Select, Insert On dbo.Address to Moderator
GRANT ALTER ANY ROLE TO Moderator;
GRANT ALTER ANY USER TO Moderator;
GRANT CREATE LOGIN TO Moderator;
GRANT ALTER ANY SERVER ROLE TO Moderator;





-- P -- R -- O -- C -- E -- D -- U -- R -- E -- 

GO									--Category [LOẠI SẢN PHẨM]

--UPDATE
CREATE PROCEDURE SP_UpdateCategory
    @category_id INT,
    @category_name NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Category
    SET category_name = @category_name,
        ModifiedBy = SUSER_NAME(),
		ModifiedTime = GETDATE()
    WHERE category_id = @category_id;
END;
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_UpdateCategory TO Moderator;
--RUN
EXEC SP_UpdateCategory @category_id = 7, @category_name = N'MINH HIẾU TEST1'
GO
--INSERT
CREATE PROCEDURE SP_InsertCategory
	@category_name nvarchar(30)
AS
BEGIN
	INSERT INTO Category(category_name, ModifiedBy)
	VALUES (@category_name, SUSER_NAME());
END;
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_InsertCategory TO Moderator;
--RUN EXAMPLE
EXEC SP_InsertCategory @category_name = N'MINH HIẾU TEST'
GO
--SELECT
CREATE PROCEDURE SP_SelectCategory
    @category_id INT = NULL,
    @category_name NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @category_id IS NULL AND @category_name IS NULL
    BEGIN
        SELECT * FROM Category WHERE DeleteTime IS NULL ORDER BY ModifiedTime DESC;
    END
    ELSE
    BEGIN
        SELECT * FROM Category
        WHERE (@category_id IS NULL OR category_id = @category_id)
          AND (@category_name IS NULL OR category_name LIKE N'%' + @category_name + N'%')
		  AND DeleteTime IS NULL
		ORDER BY ModifiedTime DESC;
    END
END;

--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_SelectCategory TO Moderator;
--RUN EXAMPLE
EXEC SP_SelectCategory;
EXEC SP_SelectCategory @category_id = NULL;
EXEC SP_SelectCategory @category_name = N'A';
EXEC SP_SelectCategory @category_id = NULL, @category_name = N'A';
GO
--DELETE
CREATE PROCEDURE SP_DeleteCategory
    @category_id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra nếu còn sản phẩm nào sử dụng SubCategory này
    IF EXISTS (
        SELECT 1 
        FROM Product 
        WHERE Category_id = @category_id
          AND DeleteTime IS NULL
    )
    BEGIN
        -- Nếu tồn tại sản phẩm, trả về 0
        RETURN 0;
    END

    -- Nếu không còn sản phẩm nào, thực hiện xóa mềm
    UPDATE Category
    SET DeleteTime = GETDATE(), ModifiedBy = SUSER_NAME()
    WHERE category_id = @category_id;

    -- Trả về 1 khi xóa thành công
    RETURN 1;
END;

--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_DeleteCategory TO Moderator;
--RUN
EXEC SP_DeleteCategory @category_id = 5




GO									--Sub Category [LOẠI SẢN PHẨM PHỤ]

--UPDATE
CREATE PROCEDURE SP_UpdateSubCategory
    @subCategory_id INT,
    @subCategory_name NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE SubCategory
    SET SubCategoryName = @subCategory_name,
        ModifiedBy = SUSER_NAME(),
		ModifiedTime = GETDATE()
    WHERE SubCategoryID = @subCategory_id;
END;
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_UpdateSubCategory TO Moderator;
--RUN
EXEC SP_UpdateSubCategory @subCategory_id = 5, @subCategory_name = N'MINH HIẾU TEST1'
GO
--INSERT
DROP PROC SP_InsertSubCategory
CREATE PROCEDURE SP_InsertSubCategory
	@subCategory_name nvarchar(30)
AS
BEGIN
	INSERT INTO SubCategory(SubCategoryName, ModifiedBy, ModifiedTime)
	VALUES (@subCategory_name, SUSER_NAME(), GETDATE());
END;
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_InsertSubCategory TO Moderator;
--RUN EXAMPLE
EXEC SP_InsertSubCategory @subCategory_name = N'MINH HIẾU TEST'

GO
--SELECT
CREATE PROCEDURE SP_GetSubCategory
    @subCategory_id INT = NULL,
    @subCategory_name NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @subCategory_id IS NULL AND @subCategory_name IS NULL
    BEGIN
        SELECT * FROM SubCategory WHERE DeleteTime IS NULL
		ORDER BY ModifiedTime DESC;
    END
    ELSE
    BEGIN
        SELECT * FROM SubCategory
        WHERE DeleteTime IS NULL
          AND (@subCategory_name IS NULL OR SubCategoryName LIKE N'%' + @subCategory_name + N'%')
		  AND (@subCategory_id IS NULL OR SubCategoryID = @subCategory_id)
		ORDER BY ModifiedTime DESC;
    END
END;

--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetSubCategory TO Moderator;
--RUN
EXEC SP_GetSubCategory
EXEC SP_GetSubCategory @subCategory_id = 3
EXEC SP_GetSubCategory @subCategory_id = 3, @subCategory_name = N'ca'
EXEC SP_GetSubCategory @subCategory_name = N'Ca'
GO
--DELETE
CREATE PROCEDURE SP_DeleteSubCategory
    @subCategory_id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM Product 
        WHERE SubCategoryID = @subCategory_id
          AND DeleteTime IS NULL
    )
    BEGIN
        RETURN 0;
    END

    UPDATE SubCategory
    SET DeleteTime = GETDATE(), ModifiedBy = SUSER_NAME()
    WHERE SubCategoryID = @subCategory_id;

    RETURN 1;
END;

--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_DeleteSubCategory TO Moderator;
--RUN
EXEC SP_DeleteSubCategory @subCategory_id = 5



GO									--PRODUCT [ SẢN PHẨM ]
--GET
CREATE PROCEDURE SP_GetAllProductsModerator
AS
BEGIN
	SELECT p.*, ph.price, ph.priceHistoryId, sc.SubCategoryName, c.category_name
	FROM Product p
	JOIN PriceHistory ph ON p.product_id = ph.product_id
	JOIN SubCategory sc ON sc.SubCategoryID = p.SubCategoryID
	JOIN Category c ON c.category_id = p.Category_id
	WHERE ph.isActive = 0 
	  AND p.DeleteTime IS NULL
	ORDER BY p.CreateTime DESC;
END;

exec SP_GetAllProductsModerator;

GRANT EXECUTE ON SP_GetAllProductsModerator TO Moderator;
GO
--UPDATE
CREATE PROCEDURE SP_UpdateProduct
    @ProductID INT,
    @ProductName NVARCHAR(50),
    @Image VARCHAR(50),
    @CategoryID INT,
    @Supplier INT,
    @SubCategoryID INT,
    @ExpiryDate DATETIME,
    @Description NVARCHAR(MAX),
    @Price DECIMAL(10,0)
AS
BEGIN
    DECLARE @CurrentPrice DECIMAL(10,0)
    DECLARE @ExistingPriceHistoryID INT
    DECLARE @ModifiedBy NVARCHAR(50)
    SET @ModifiedBy = SUSER_NAME()

    -- Update Product table
    UPDATE Product
    SET 
        product_name = @ProductName,
        image = @Image,
        Category_id = @CategoryID,
        Supplier = @Supplier,
        SubCategoryID = @SubCategoryID,
        ExpriryDate = @ExpiryDate,
        Description = @Description,
        ModifiedBy = @ModifiedBy
    WHERE 
        product_id = @ProductID

    -- Kiểm tra xem giá đầu vào có khác với giá hiện tại đang hoạt động không
    SELECT @CurrentPrice = price
    FROM PriceHistory
    WHERE 
        product_id = @ProductID 
        AND isActive = 0
        AND DeleteTime IS NULL

    IF @CurrentPrice <> @Price
    BEGIN
        -- Kiểm tra xem giá mới có tồn tại trong PriceHistory và không bị xóa không
        SELECT @ExistingPriceHistoryID = priceHistoryId
        FROM PriceHistory
        WHERE 
            product_id = @ProductID 
            AND price = @Price
            AND DeleteTime IS NULL

        IF @ExistingPriceHistoryID IS NOT NULL
        BEGIN
            -- Nếu giá tồn tại, hãy đặt isActive của nó thành 0
            UPDATE PriceHistory
            SET 
                isActive = 0,
                ModifiedTime = GETDATE(),
                ModifiedBy = @ModifiedBy
            WHERE 
                priceHistoryId = @ExistingPriceHistoryID

            -- Cập nhật giá cũ thành 1 => không active
            UPDATE PriceHistory
            SET 
                isActive = 1,
                ModifiedTime = GETDATE(),
                ModifiedBy = @ModifiedBy
            WHERE 
                product_id = @ProductID 
                AND isActive = 0 
                AND price != @Price
        END
        ELSE
        BEGIN
            -- Nếu giá không tồn tại, chèn giá mới
            INSERT INTO PriceHistory (product_id, price, CreateDate, ModifiedBy, ModifiedTime, isActive)
            VALUES (@ProductID, @Price, GETDATE(), @ModifiedBy, GETDATE(), 0)

            -- Cập nhật giá cũ thành 1 => không active
            UPDATE PriceHistory
            SET 
                isActive = 1,
                ModifiedTime = GETDATE(),
                ModifiedBy = @ModifiedBy
            WHERE 
                product_id = @ProductID 
                AND isActive = 0 
                AND price != @Price
        END
    END
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_UpdateProduct TO Moderator;
GO
--RUN
EXEC SP_UpdateProduct 
	@ProductID = 20,
	@ProductName = N'Tên Sản Phẩm2',
	@Image = 'Hình Ảnh2',
	@CategoryID = 3,
	@Supplier = 5, 
	@SubCategoryID = 3, 
	@ExpiryDate = '2024-11-13', 
	@Description = N'Mô tả sản phẩm2', 
	@Price = 102
--INSERT --> khi thêm sản phẩm nhập giá => insert PriceHistory trạng thái là 0
go
CREATE PROCEDURE SP_InsertProduct
    @ProductName NVARCHAR(50),
    @Image VARCHAR(50),
    @CategoryID INT,
    @Supplier INT,
    @SubCategoryID INT,
    @ExpiryDate DATETIME,
    @Description NVARCHAR(MAX),
    @Price DECIMAL(10,0)
AS
BEGIN
    -- Insert into Product table and get the inserted ID
    DECLARE @InsertedProductID INT

    INSERT INTO Product (product_name, image, Category_id, Supplier, SubCategoryID, ExpriryDate, Description, ModifiedBy)
    VALUES (@ProductName, @Image, @CategoryID, @Supplier, @SubCategoryID, @ExpiryDate, @Description, SUSER_NAME())

    SET @InsertedProductID = SCOPE_IDENTITY()

    -- Insert into PriceHistory table
    INSERT INTO PriceHistory (product_id, price, CreateDate, ModifiedBy, ModifiedTime, isActive)
    VALUES (@InsertedProductID, @Price, GETDATE(), SUSER_NAME(), GETDATE(), 0)
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_InsertProduct TO Moderator;
GO
--RUN
EXEC SP_InsertProduct @ProductName = N'Tên Sản Phẩm',
						@Image = 'Hình Ảnh',
						@CategoryID = 3,
						@Supplier = 5, 
						@SubCategoryID = 3, 
						@ExpiryDate = '2024-11-13', 
						@Description = N'Mô tả sản phẩm', 
						@Price = 100

GO
--DELETE
CREATE PROCEDURE SP_DeleteProduct
	@ProductID INT
AS
BEGIN
	UPDATE Product SET DeleteTime = GETDATE(), ModifiedBy = SUSER_NAME()
	WHERE product_id = @ProductID;

	UPDATE PriceHistory SET isActive = 1, ModifiedBy = SUSER_NAME(), ModifiedTime = GETDATE(), DeleteTime = GETDATE()
	WHERE product_id = @ProductID AND isActive = 0
END;
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_DeleteProduct TO Moderator;
GO
--RUN
EXEC SP_DeleteProduct @ProductID = 19

GO
--nếu có price history thì phải đổi trạng thái thành 1(không chọn)
--Cell thông báo sản phẩm còn trong CELLS
--CART xóa cả sản phẩm bên trong CART
--SELECT

--BỎ
CREATE PROCEDURE SP_GetProductsByModerator
AS
BEGIN
    SELECT P.*, PH.price
    FROM Product P
    JOIN PriceHistory PH ON P.product_id = PH.product_id
    WHERE 
        PH.DeleteTime IS NULL 
        AND P.DeleteTime IS NULL
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetProductsByModerator TO Moderator;

GO
--RUN
EXEC SP_GetProductsByModerator

exec SP_GetAllProducts;
GO

GO									--SUPPLIER [ Nhà Sản xuất ]
--GET


CREATE PROCEDURE SP_GetSupplier
    @SuppelierName Nvarchar(30) = NULL
AS
BEGIN
    IF @SuppelierName IS NULL
    BEGIN
        SELECT S.*
        FROM Supplier S
		WHERE S.DeleteTime IS NULL;
    END
    ELSE
    BEGIN
        SELECT S.*
        FROM Supplier S
        WHERE S.SupplierName LIKE N'%' + @SuppelierName + N'%'
		AND S.DeleteTime IS NULL;
    END
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetSupplier TO Moderator;
GO
--RUN
EXEC SP_GetSupplier
EXEC SP_GetSupplier N'Bu'

GO
--UPDATE
CREATE PROCEDURE SP_UpdateSupplier
    @SupplierID INT,
    @SupplierName NVARCHAR(30),
    @AddressID INT
AS
BEGIN
    UPDATE Supplier
    SET 
        SupplierName = @SupplierName,
        AddressID = @AddressID
    WHERE 
        SupplierID = @SupplierID
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_UpdateSupplier TO Moderator;
GO
--RUN
EXEC SP_UpdateSupplier 
    22,  -- ID Nhà Cung Cấp
    N'TEST UPDATE', 
    27  -- ID Địa Chỉ Mới
GO
--INSERT
CREATE PROCEDURE SP_InsertSupplier
    @SupplierName NVARCHAR(30),
    @AddressID INT
AS
BEGIN
    INSERT INTO Supplier (SupplierName, AddressID)
    VALUES (@SupplierName, @AddressID)
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_InsertSupplier TO Moderator;
GO
--RUN
EXEC SP_InsertSupplier N'TEST INSERT', 29
GO
--DELETE
CREATE PROCEDURE SP_DeleteSupplier
	@SupplierID INT
AS
BEGIN
	IF EXISTS (
		SELECT 1 FROM Product WHERE Supplier = @SupplierID AND DeleteTime IS NULL
	)
	BEGIN
		RAISERROR('Không thể xóa nhà cung cấp này vì có sản phẩm đang sử dụng.', 16, 1);
	END
	ELSE
	BEGIN
		UPDATE Supplier
		SET DeleteBy = SUSER_NAME(), DeleteTime = GETDATE()
		WHERE SupplierID = @SupplierID
	END
END
GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_DeleteSupplier TO Moderator;
GO
--RUN
EXEC SP_DeleteSupplier 22
GO
--##################################################################################################################
GO									--CustomerType [ Loại Khách Hàng ]
--INSERT
CREATE PROCEDURE SP_InsertCustomerType
    @CustomerTypeName NVARCHAR(50)
AS
BEGIN
    INSERT INTO CustomerType (type_customer_name)
    VALUES (@CustomerTypeName)
END
GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_InsertCustomerType TO Moderator;
--RUN
EXEC SP_InsertCustomerType N'TEST INSERT'
GO

--GET
CREATE PROCEDURE SP_GetCustomerType
    @CustomerID INT = NULL
AS
BEGIN
    IF @CustomerID IS NULL
    BEGIN
        SELECT *
        FROM CustomerType
    END
    ELSE
    BEGIN
        SELECT CT.*
        FROM CustomerType CT
        JOIN Customer C ON C.customerId = CT.type_customer_id
        WHERE C.customerId = @CustomerID
    END
END
GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetCustomerType TO Moderator;
--RUN
EXEC SP_GetCustomerType
EXEC SP_GetCustomerType 1 -- GET THEO ID KHÁCH HÀNG
GO
--UPDATE
CREATE PROCEDURE SP_UpdateCustomerType
    @CustomerID INT,
    @CustomerTypeName NVARCHAR(30)
AS
BEGIN
    UPDATE CustomerType
    SET type_customer_name = @CustomerTypeName
    WHERE type_customer_id = @CustomerID
END

GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_UpdateCustomerType TO Moderator;
GO
--RUN
EXEC SP_UpdateCustomerType 4, N'TEST UPDATE'
GO
--DELETE => XÓA CỨNG
CREATE PROCEDURE SP_DeleteCustomerType
    @CustomerTypeID INT,
    @Result INT OUTPUT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Customer WHERE type_customer_id = @CustomerTypeID)
    BEGIN
        SET @Result = 0 --KHÔNG THÀNH CÔNG CÒN KHÁCH
    END
    ELSE
    BEGIN
        DELETE FROM CustomerType
        WHERE type_customer_id = @CustomerTypeID
        
        SET @Result = 1
    END
END

GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_DeleteCustomerType TO Moderator;
GO
--RUN
DECLARE @Result INT
EXEC SP_DeleteCustomerType 
    @CustomerTypeID = 1, 
    @Result = @Result OUTPUT
SELECT @Result
GO
--THAY THẾ CUSTOMER TYPE
CREATE PROCEDURE SP_ReplaceCustomerType
    @OldCustomerTypeID INT,
    @NewCustomerTypeID INT,
    @Result INT OUTPUT
AS
BEGIN
    BEGIN TRANSACTION

    BEGIN TRY
        UPDATE Customer
        SET type_customer_id = @NewCustomerTypeID
        WHERE type_customer_id = @OldCustomerTypeID
		--SUCCESS
        SET @Result = 1

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
		--FAILE
        SET @Result = 0
    END CATCH
END

--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_ReplaceCustomerType TO Moderator;
GO
--RUN
DECLARE @Result INT
EXEC SP_ReplaceCustomerType 
    @OldCustomerTypeID = 2, 
	@NewCustomerTypeID = 1,
    @Result = @Result OUTPUT
SELECT @Result

GO
--##################################################################################################################
GO									--Department [ Phòng Ban ]
--SELECT
DROP PROC SP_GetDepartment
CREATE PROCEDURE SP_GetDepartment
AS
BEGIN
    SELECT d.DepartmentID, d.DepartmentName, COUNT(uf.AccountName) as N'Số lượng nhân viên'
	FROM Department d
	LEFT JOIN Employee e ON e.DepartmentID = d.DepartmentID
	LEFT JOIN UserInfo uf ON uf.Employ_ID = e.EmployeeID AND uf.AccountName IS NOT NULL
	WHERE d.DeleteTime IS NULL and (e.DeleteTime IS NULL OR e.DeleteTime IS NULL)
	GROUP BY d.DepartmentID, d.DepartmentName;
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetDepartment TO Moderator;
GO
--RUN
EXEC SP_GetDepartment

GO
--INSERT
DROP PROC SP_InsertDepartment
CREATE PROCEDURE SP_InsertDepartment
    @DepartmentName NVARCHAR(50)
AS
BEGIN
    -- Kiểm tra xem tên phòng ban đã tồn tại hay chưa
    IF EXISTS (SELECT 1 FROM Department WHERE DepartmentName = @DepartmentName AND DeleteTime IS NULL)
    BEGIN
        -- Nếu tên phòng ban đã tồn tại, thông báo lỗi
        RAISERROR('Tên phòng ban đã tồn tại.', 16, 1)
    END
    ELSE
    BEGIN
        -- Chèn phòng ban mới nếu tên chưa tồn tại
        INSERT INTO Department (DepartmentName, ModifiedBy, CreateTime, ModifiedTime)
        VALUES (@DepartmentName, SUSER_NAME(), GETDATE(), GETDATE())
    END
END

GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_InsertDepartment TO Moderator;
GO
--RUN
EXEC SP_InsertDepartment N'TEST INSERT'
GO
--UPDATE
CREATE PROCEDURE SP_UpdateDepartmentName
    @DepartmentID INT,
    @NewDepartmentName NVARCHAR(30)
AS
BEGIN
    -- Kiểm tra xem tên phòng ban mới đã tồn tại hay chưa
    IF EXISTS (SELECT 1 FROM Department WHERE DepartmentName = @NewDepartmentName AND DeleteTime IS NULL)
    BEGIN
        -- Nếu tên phòng ban mới đã tồn tại, thông báo lỗi
        RAISERROR('Tên phòng ban mới đã tồn tại.', 16, 1)
    END
    ELSE
    BEGIN
        -- Cập nhật tên phòng ban nếu tên mới chưa tồn tại
        UPDATE Department
        SET 
            DepartmentName = @NewDepartmentName,
            ModifiedBy = SUSER_NAME(),
            ModifiedTime = GETDATE()
        WHERE 
            DepartmentID = @DepartmentID
    END
END
GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_UpdateDepartmentName TO Moderator;
GO
--RUN
EXEC SP_UpdateDepartmentName 2, N'TEST INSERT'
GO
--DELETE
CREATE PROCEDURE SP_SoftDeleteDepartment
    @DepartmentID INT
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        -- Kiểm tra và cập nhật DepartmentID của nhân viên
        UPDATE Employee
        SET DepartmentID = 7
        WHERE DepartmentID = @DepartmentID

        -- Xóa mềm phòng ban
        UPDATE Department
        SET 
            DeleteTime = GETDATE(),
            ModifiedBy = SUSER_NAME()
        WHERE 
            DepartmentID = @DepartmentID

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        -- Thông báo lỗi nếu có
        RAISERROR('Có lỗi xảy ra trong quá trình xóa mềm phòng ban.', 16, 1)
    END CATCH
END

GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_SoftDeleteDepartment TO Moderator;
GO
--RUN
EXEC SP_SoftDeleteDepartment 5
--##################################################################################################################
GO									--Employee [ Nhân Viên ]
GO --SELECT
CREATE PROCEDURE SP_GetEmployees
    @DepartmentID INT = NULL,
    @EmployeeTypeID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
		E.EmployeeID,
		UF.AccountName, 
		UF.full_name, 
		ET.EmployeeTypeID, 
		ET.EmployeeTypeName, 
		D.DepartmentID, 
		D.DepartmentName,
		UF.gender,
		UF.address_id,
		A.Note + ' / ' + A.HouseNumber + ', ' + C.CommuneName + ', ' + DI.DistrictName + ', ' + PR.ProvinceName AS N'ĐỊA CHỈ'
	FROM Employee E
		JOIN UserInfo UF ON UF.Employ_ID = E.EmployeeID
		JOIN EmployeeType ET ON ET.EmployeeTypeID = E.EmployeeTypeID
		JOIN Department D ON D.DepartmentID = E.DepartmentID
		LEFT JOIN Address A ON UF.address_id = A.AddressID
		LEFT JOIN Commune C ON C.CommuneID = A.CommuneID
		LEFT JOIN District DI ON DI.DistrictID = C.DistrictID
		LEFT JOIN Province PR ON PR.ProvinceID = DI.ProvinceID
	WHERE 
		E.DeleteTime IS NULL 
		AND ET.DeleteTime IS NULL 
		AND D.DeleteTime IS NULL 
		AND UF.DeleteTime IS NULL
        AND (@DepartmentID IS NULL OR E.DepartmentID = @DepartmentID)
        AND (@EmployeeTypeID IS NULL OR E.EmployeeTypeID = @EmployeeTypeID);
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetEmployees TO Moderator;
GO
--RUN
EXEC SP_GetEmployees
EXEC SP_GetEmployees @DepartmentID = 4
EXEC SP_GetEmployees @EmployeeTypeID = 4
EXEC SP_GetEmployees @EmployeeTypeID = 5, @DepartmentID = 4

select * from Employee e, UserInfo u where u.Employ_ID = e.EmployeeID

GO --INSERT
CREATE PROCEDURE SP_InsertEmployee
    @EmployeeTypeID INT,
    @DepartmentID INT,
    @AccountName VARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NewEmployeeID INT;
    DECLARE @EmployeeType NVARCHAR(30);
    DECLARE @ROLE INT;
    DECLARE @ErrorMessage NVARCHAR(4000);

    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra xem EmployeeTypeID có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM EmployeeType WHERE EmployeeTypeID = @EmployeeTypeID)
        BEGIN
            RAISERROR(N'Loại nhân viên không tồn tại.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra xem DepartmentID có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM Department WHERE DepartmentID = @DepartmentID)
        BEGIN
            RAISERROR(N'Phòng ban không tồn tại.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Chèn vào bảng Nhân viên
        INSERT INTO Employee (EmployeeTypeID, DepartmentID, ModifiedBy)
        VALUES (@EmployeeTypeID, @DepartmentID, SUSER_NAME());

        SET @NewEmployeeID = SCOPE_IDENTITY();

        -- Lấy EmployeeTypeName
        SELECT @EmployeeType = EmployeeTypeName
        FROM EmployeeType
        WHERE EmployeeTypeID = @EmployeeTypeID;

        -- Tạo Login
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = 'CREATE LOGIN [' + @AccountName + '] WITH PASSWORD = ''123'';';
        EXEC sp_executesql @Sql;

        -- Tạo User trong cơ sở dữ liệu hiện tại
        SET @Sql = 'CREATE USER [' + @AccountName + '] FOR LOGIN [' + @AccountName + '];';
        EXEC sp_executesql @Sql;

        -- Gán vai trò cho người dùng
        EXEC sp_addrolemember @EmployeeType, @AccountName;

        -- Kiểm tra nếu @EmployeeTypeID = 6 thì cấp quyền đặc biệt
        IF @EmployeeTypeID = 6
        BEGIN
            -- Gán tài khoản vào vai trò 'CustomerServerRole' nếu role này tồn tại
            IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'CustomerServerRole')
            BEGIN
                -- Đảm bảo tài khoản có quyền ALTER SERVER ROLE trước khi thực hiện
                EXEC sp_addsrvrolemember @AccountName, 'sysadmin'; -- Hoặc cấp quyền sysadmin cho tài khoản hiện tại

                SET @Sql = 'ALTER SERVER ROLE CustomerServerRole ADD MEMBER [' + @AccountName + '];';
                EXEC sp_executesql @Sql;
            END
            ELSE
            BEGIN
                PRINT N'Role CustomerServerRole không tồn tại. Không thể thêm thành viên.';
            END
        END

        -- Lấy Role ID từ bảng Roles
        SELECT @ROLE = R.role_id FROM Roles R WHERE R.role_name = @EmployeeType;

        -- Chèn vào bảng Users
        INSERT INTO Users (AccountName, role_id)
        VALUES (@AccountName, @ROLE);

        -- Chèn vào bảng UserInfo
        INSERT INTO UserInfo (AccountName, Employ_ID, ModifiedBy)
        VALUES (@AccountName, @NewEmployeeID, SUSER_NAME());

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback if any error occurs
        ROLLBACK TRANSACTION;
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END

select * From EmployeeType
select * from Department

GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_InsertEmployee TO Moderator;
GO--RUN
EXEC SP_InsertEmployee @EmployeeTypeID = 6, @DepartmentID = 7, @AccountName = 'HiuModerator'

GO--UPDATE
CREATE PROCEDURE SP_UpdateEmployee
    @AccountName NVARCHAR(25),
    @EmployeeTypeID INT,
    @DepartmentID INT,
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @AddressID INT,
    @Phone NVARCHAR(15),
    @Gender INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @EmployeeID INT;
    DECLARE @CurrentEmployeeTypeID INT;
    DECLARE @ErrorMessage NVARCHAR(4000);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Lấy EmployeeID và loại nhân viên hiện tại từ bảng UserInfo và Employee
        SELECT @EmployeeID = UF.Employ_ID, @CurrentEmployeeTypeID = E.EmployeeTypeID
        FROM UserInfo UF
        JOIN Employee E ON UF.Employ_ID = E.EmployeeID
        WHERE UF.AccountName = @AccountName;

        -- Cập nhật bảng Employee
        UPDATE Employee
        SET 
            EmployeeTypeID = @EmployeeTypeID,
            DepartmentID = @DepartmentID,
            ModifiedBy = SUSER_NAME(),
            ModifiedTime = GETDATE()
        WHERE EmployeeID = @EmployeeID;

        -- Cập nhật bảng UserInfo
        UPDATE UserInfo
        SET 
            full_name = @FullName,
            email = @Email,
            address_id = @AddressID,
            phone = @Phone,
            gender = @Gender,
            ModifiedBy = SUSER_NAME(),
            ModifiedTime = GETDATE()
        WHERE Employ_ID = @EmployeeID;

        -- Nếu loại nhân viên thay đổi, cập nhật quyền
        IF @CurrentEmployeeTypeID <> @EmployeeTypeID
        BEGIN
            -- Lấy tên loại nhân viên mới
            DECLARE @NewEmployeeType NVARCHAR(30);
            SELECT @NewEmployeeType = EmployeeTypeName
            FROM EmployeeType 
            WHERE EmployeeTypeID = @EmployeeTypeID;

            -- Xóa quyền cũ
            DECLARE @OldEmployeeType NVARCHAR(30);
            SELECT @OldEmployeeType = EmployeeTypeName
            FROM EmployeeType 
            WHERE EmployeeTypeID = @CurrentEmployeeTypeID;
            EXEC sp_droprolemember @OldEmployeeType, @AccountName;

            -- Gán quyền mới
            EXEC sp_addrolemember @NewEmployeeType, @AccountName;

            -- Xử lý đặc biệt nếu EmployeeTypeID = 6
            IF @EmployeeTypeID = 6
            BEGIN
                -- Gán tài khoản vào vai trò 'CustomerServerRole' nếu role này tồn tại
                IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'CustomerServerRole')
                BEGIN
                    -- Đảm bảo tài khoản có quyền ALTER SERVER ROLE trước khi thực hiện
                    EXEC sp_addsrvrolemember @AccountName, 'sysadmin'; -- Hoặc cấp quyền sysadmin cho tài khoản hiện tại

                    DECLARE @Sql NVARCHAR(MAX);
                    SET @Sql = 'ALTER SERVER ROLE CustomerServerRole ADD MEMBER [' + @AccountName + '];';
                    EXEC sp_executesql @Sql;
                END
                ELSE
                BEGIN
                    PRINT N'Role CustomerServerRole không tồn tại. Không thể thêm thành viên.';
                END
            END
            ELSE
            BEGIN
                -- Nếu EmployeeTypeID khác 6, xóa bỏ quyền nếu tồn tại
                IF EXISTS (
					SELECT 1 FROM sys.server_role_members 
					WHERE role_principal_id = (SELECT principal_id FROM sys.server_principals 
					WHERE name = 'CustomerServerRole') AND member_principal_id = (SELECT principal_id FROM sys.server_principals WHERE name = @AccountName)
				)
                BEGIN
                    --DECLARE @Sql NVARCHAR(MAX);
                    SET @Sql = 'ALTER SERVER ROLE CustomerServerRole DROP MEMBER [' + @AccountName + '];';
                    EXEC sp_executesql @Sql;
                END
            END
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback nếu có lỗi xảy ra
        ROLLBACK TRANSACTION;
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END


select * from EmployeeType
select * from Department

SELECT EmployeeTypeName
            FROM EmployeeType 
            WHERE EmployeeTypeID = 1;

SELECT * 
FROM Employee e, UserInfo uf
WHERE e.EmployeeID = uf.Employ_ID




GO--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_UpdateEmployee TO Moderator;
--RUN
EXEC SP_UpdateEmployee
    @AccountName = N'HiuModerator2',
    @EmployeeTypeID = 5,
    @DepartmentID = 2,
    @FullName = N'Nguyễn Văn B',
    @Email = 'nguyenvana@example.com',
    @AddressID = 1,
    @Phone = '0123456789',
    @Gender = 0;



GO--DELETE
DROP PROC SP_DeleteEmployee
go
CREATE PROCEDURE SP_DeleteEmployee
    @AccountName NVARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @EmployeeID INT;
    DECLARE @ErrorMessage NVARCHAR(4000);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Lấy EmployeeID từ bảng UserInfo
        SELECT @EmployeeID = Employ_ID
        FROM UserInfo
        WHERE AccountName = @AccountName;

        -- Cập nhật ModifiedTime, ModifiedBy và DeleteTime trong bảng Employee
        UPDATE Employee
        SET 
            ModifiedTime = GETDATE(),
            ModifiedBy = SUSER_NAME(),
            DeleteTime = GETDATE()
        WHERE EmployeeID = @EmployeeID;

        -- Cập nhật ModifiedTime, ModifiedBy và DeleteTime trong bảng UserInfo
        UPDATE UserInfo
        SET 
            ModifiedTime = GETDATE(),
            ModifiedBy = SUSER_NAME(),
            DeleteTime = GETDATE()
        WHERE AccountName = @AccountName;

        -- Xóa login và user trong SQL Server nếu tồn tại
        DECLARE @Sql NVARCHAR(MAX);

        IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @AccountName)
        BEGIN
            SET @Sql = 'DROP LOGIN [' + @AccountName + '];';
            EXEC sp_executesql @Sql;
        END

        IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @AccountName)
        BEGIN
            SET @Sql = 'DROP USER [' + @AccountName + '];';
            EXEC sp_executesql @Sql;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback nếu có lỗi xảy ra
        ROLLBACK TRANSACTION;
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END

GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_DeleteEmployee TO Moderator;
--RUN
EXEC SP_DeleteEmployee @AccountName = N'';

--##################################################################################################################
GO									--Customer [ Khách Hàng ]
--SELECT
--> theo loại khách hàng
CREATE PROCEDURE SP_GetCustomer
    @TypeCustomerID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        UF.AccountName, 
        UF.full_name, 
        UF.email, 
        UF.phone, 
        UF.gender, 
        CT.type_customer_name, 
        A.AddressID,
        A.Note + ' / ' + A.HouseNumber + ' , ' + CO.CommuneName + ' , ' + DI.DistrictName + ' , ' + PR.ProvinceName AS [ĐỊA CHỈ]
    FROM UserInfo UF
    JOIN Customer C ON C.customerId = UF.customer_Id
    JOIN CustomerType CT ON CT.type_customer_id = C.type_customer_id
    LEFT JOIN Address A ON A.AddressID = UF.address_id
    LEFT JOIN Commune CO ON CO.CommuneID = A.CommuneID
    LEFT JOIN District DI ON DI.DistrictID = CO.DistrictID
    LEFT JOIN Province PR ON PR.ProvinceID = DI.ProvinceID
    WHERE 
		@TypeCustomerID IS NULL OR C.type_customer_id = @TypeCustomerID
		AND UF.DeleteTime IS NULL AND C.DeleteTime IS NULL
    ORDER BY UF.CreateTime DESC;
END
GO
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetCustomer TO Moderator;
--RUN
EXEC SP_GetCustomer @TypeCustomerID = 2;

GO--UPDATE
CREATE PROCEDURE SP_UpdateCustomer
    @AccountName NVARCHAR(50),
    @NewTypeCustomerID INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMessage NVARCHAR(4000);
    
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Cập nhật loại khách hàng trong bảng Customer dựa trên AccountName
        UPDATE Customer
        SET type_customer_id = @NewTypeCustomerID, ModifiedBy = SUSER_NAME(), ModifiedTime = GETDATE()
        WHERE customerId = (SELECT customer_Id FROM UserInfo WHERE AccountName = @AccountName)

        COMMIT TRANSACTION;
        
        -- Thông báo hoàn thành
        PRINT N'Đã cập nhật loại khách hàng thành công!';
    END TRY
    BEGIN CATCH
        -- Rollback nếu có lỗi xảy ra
        ROLLBACK TRANSACTION;
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_UpdateCustomer TO Moderator;
GO--RUN
EXEC SP_UpdateCustomer @AccountName = N'john_doe', @NewTypeCustomerID = 2

GO--DELETE
CREATE PROCEDURE SP_DeleteCustomer
    @AccountName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @CustomerID INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Lấy CustomerID từ bảng UserInfo
        SELECT @CustomerID = customer_Id
        FROM UserInfo
        WHERE AccountName = @AccountName;

        -- Kiểm tra xem khách hàng có đơn đặt hàng hay không
        IF EXISTS (SELECT 1 FROM [Order] WHERE CreateBy = @CustomerID)
        BEGIN
            -- Xóa mềm (Soft delete)
            UPDATE Customer
            SET 
                DeleteTime = GETDATE(),
                ModifiedBy = SUSER_NAME()
            WHERE customerId = @CustomerID;

            UPDATE UserInfo
            SET 
                DeleteTime = GETDATE(),
                ModifiedBy = SUSER_NAME()
            WHERE AccountName = @AccountName;
        END
        ELSE
        BEGIN
            -- Xóa tất cả cart của user đó
            DELETE FROM Cart
            WHERE customerId = @CustomerID;

            -- Xóa thông tin trong bảng UserInfo
            DELETE FROM UserInfo
            WHERE AccountName = @AccountName;

            -- Xóa user từ bảng Users
            DELETE FROM Users
            WHERE AccountName = @AccountName;

            -- Xóa thông tin trong bảng Customer
            DELETE FROM Customer
            WHERE customerId = @CustomerID;
        END
		 -- Xóa login và user trong SQL Server
        DECLARE @Sql NVARCHAR(MAX);
        IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @AccountName)
        BEGIN
            SET @Sql = 'DROP LOGIN [' + @AccountName + '];';
            EXEC sp_executesql @Sql;
        END

        IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @AccountName)
        BEGIN
            SET @Sql = 'DROP USER [' + @AccountName + '];';
            EXEC sp_executesql @Sql;
        END
        COMMIT TRANSACTION;
        
        -- Thông báo hoàn thành
        PRINT N'Đã xóa khách hàng thành công!';
    END TRY
    BEGIN CATCH
        -- Rollback nếu có lỗi xảy ra
        ROLLBACK TRANSACTION;
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_DeleteCustomer TO Moderator;
GO--RUN
EXEC SP_DeleteCustomer @AccountName = N'hieuThu3'
go
--//////////////
DROP PROC GetUserAccountName
GO
CREATE PROCEDURE GetUserAccountName
AS
BEGIN
    SELECT SUSER_NAME() AS AccountName;
END
GO--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::GetUserAccountName TO Moderator;
GO--RUN
EXEC GetUserAccountName