go 
--create role Moderator
Create ROLE Moderator;
-- R -- O -- L -- E
Grant Select On dbo.Users to Moderator
Grant Select, Update, Insert On dbo.UserInfo to Moderator
Grant Select On dbo.Province to Moderator
Grant Select, Insert On dbo.District to Moderator
Grant Select, Insert On dbo.Commune to Moderator
Grant Select, Insert On dbo.Address to Moderator

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
        SELECT * FROM Category WHERE DeleteTime IS NULL;
    END
    ELSE
    BEGIN
        SELECT * FROM Category
        WHERE (@category_id IS NULL OR category_id = @category_id)
          AND (@category_name IS NULL OR category_name LIKE N'%' + @category_name + N'%')
		  AND DeleteTime IS NULL;
    END
END;
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_SelectCategory TO Moderator;
--RUN EXAMPLE
EXEC SP_SelectCategory;
EXEC SP_SelectCategory @category_id = 1;
EXEC SP_SelectCategory @category_name = N'A';
EXEC SP_SelectCategory @category_id = 1, @category_name = N'A';
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
CREATE PROCEDURE SP_InsertSubCategory
	@subCategory_name nvarchar(30)
AS
BEGIN
	INSERT INTO SubCategory(SubCategoryName, ModifiedBy)
	VALUES (@subCategory_name, SUSER_NAME());
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
    END
    ELSE
    BEGIN
        SELECT * FROM SubCategory
        WHERE DeleteTime IS NULL
          AND (@subCategory_name IS NULL OR SubCategoryName LIKE N'%' + @subCategory_name + N'%')
		  AND (@subCategory_id IS NULL OR SubCategoryID = @subCategory_id);
    END
END;

--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetSubCategory TO Moderator;
--RUN
EXEC SP_GetSubCategory
EXEC SP_GetSubCategory @subCategory_id = 3
EXEC SP_GetSubCategory @subCategory_id = 3, @subCategory_name = N''
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

    -- Check if the input price is different from the current active price
    SELECT @CurrentPrice = price
    FROM PriceHistory
    WHERE 
        product_id = @ProductID 
        AND isActive = 0
        AND DeleteTime IS NULL

    IF @CurrentPrice <> @Price
    BEGIN
        -- Check if the new price exists in PriceHistory and is not deleted
        SELECT @ExistingPriceHistoryID = priceHistoryId
        FROM PriceHistory
        WHERE 
            product_id = @ProductID 
            AND price = @Price
            AND DeleteTime IS NULL

        IF @ExistingPriceHistoryID IS NOT NULL
        BEGIN
            -- If the price exists, set its isActive to 0
            UPDATE PriceHistory
            SET 
                isActive = 0,
                ModifiedTime = GETDATE(),
                ModifiedBy = @ModifiedBy
            WHERE 
                priceHistoryId = @ExistingPriceHistoryID

            -- Set the old active price to 1
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
            -- If the price does not exist, insert the new price
            INSERT INTO PriceHistory (product_id, price, CreateDate, ModifiedBy, ModifiedTime, isActive)
            VALUES (@ProductID, @Price, GETDATE(), @ModifiedBy, GETDATE(), 0)

            -- Set the old active price to 1
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
GO

GO									--SUPPLIER [ Nhà Sản xuất ]
--GET
CREATE PROCEDURE SP_GetSupplier
    @ProductID INT = NULL
AS
BEGIN
    IF @ProductID IS NULL
    BEGIN
        SELECT S.*
        FROM Supplier S
    END
    ELSE
    BEGIN
        SELECT S.*
        FROM Supplier S
        JOIN Product P ON S.SupplierID = P.Supplier
        WHERE P.product_id = @ProductID
    END
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetSupplier TO Moderator;
GO
--RUN
EXEC SP_GetSupplier 10

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
CREATE PROCEDURE SP_GetDepartment
AS
BEGIN
    SELECT * 
    FROM Department
    WHERE DeleteTime IS NULL
END
--GÁN QUYỀN
GRANT EXECUTE ON OBJECT::SP_GetDepartment TO Moderator;
GO
--RUN
EXEC SP_GetDepartment
GO
--INSERT
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
        INSERT INTO Department (DepartmentName, ModifiedBy, CreateTime)
        VALUES (@DepartmentName, SUSER_NAME(), GETDATE())
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
--SELECT
--INSERT
--UPDATE
--DELETE
--##################################################################################################################
GO									--Customer [ Khách Hàng ]
--SELECT
--INSERT
--UPDATE
--DELETE