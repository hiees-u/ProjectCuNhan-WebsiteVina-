--##############################################################################################################################################################
-- tạo nhóm quyền người dùng:
go
Create ROLE Customer;

Grant Select On dbo.Product to Customer
Grant Select, Insert On dbo.Customer to Customer
Grant Select, Update, Insert, DELETE  On dbo.Cart to Customer
Grant Select, Update, Insert On dbo.UserInfo to Customer
Grant Select, Update, Insert, Delete On dbo.Cart to Customer
Grant Select, Update, Insert On dbo.[Order] to Customer
Grant Select, Update, Insert, Delete On dbo.OrderDetail to Customer
Grant Select On dbo.PriceHistory to Customer
Grant Select On dbo.Province to Customer
Grant Select, Insert On dbo.District to Customer
Grant Select, Insert On dbo.Commune to Customer
Grant Select, Insert On dbo.Address to Customer;
Grant Select On CustomerType to Customer;
Grant Select On Commune to Customer
Grant Select On District to Customer
Grant Select On Province to Customer

go 
--create role Moderator
Create ROLE Moderator;
Grant Select, Update, Insert On dbo.UserInfo to Moderator
Grant Select On dbo.Province to Moderator
Grant Select On dbo.Users to Moderator
Grant Select, Insert On dbo.District to Moderator
Grant Select, Insert On dbo.Commune to Moderator
Grant Select, Insert On dbo.Address to Moderator

----procedure update state Order

--create role Warehouse Employee
Create ROLE WarehouseEmployee;
Grant Select On dbo.Users to WarehouseEmployee
Grant Select, Update, Insert On dbo.UserInfo to WarehouseEmployee
Grant Select On dbo.Province to WarehouseEmployee
Grant Select, Insert On dbo.District to WarehouseEmployee
Grant Select, Insert On dbo.Commune to WarehouseEmployee
Grant Select, Insert On dbo.Address to WarehouseEmployee

--create role Order Approver
Create ROLE OrderApprover;
Grant Select On dbo.Users to OrderApprover
Grant Select, Update, Insert On dbo.UserInfo to OrderApprover
Grant Select On dbo.Province to OrderApprover
Grant Select, Insert On dbo.District to OrderApprover
Grant Select, Insert On dbo.Commune to OrderApprover
Grant Select, Insert On dbo.Address to OrderApprover



--procedure change password user (customer)
go
--##############################################################################################################################################################
CREATE PROCEDURE ChangePassword
    @NewPassword NVARCHAR(50)
AS
BEGIN
    DECLARE @AccountName NVARCHAR(50);
    SET @AccountName = SUSER_NAME(); -- Get the currently logged-in user's account name

    -- Kiểm tra xem Login có tồn tại hay không
    IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @AccountName)
    BEGIN
        -- Thay đổi mật khẩu cho Login
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = 'ALTER LOGIN [' + @AccountName + '] WITH PASSWORD = ''' + @NewPassword + ''';';
        EXEC sp_executesql @Sql;

        -- Thông báo thay đổi mật khẩu thành công
        PRINT N'Mật khẩu đã được thay đổi thành công!';
    END
    ELSE
    BEGIN
        -- Nếu Login không tồn tại, thông báo lỗi
        PRINT N'Tài khoản không tồn tại.';
    END
END

GRANT EXECUTE ON OBJECT::dbo.ChangePassword TO Customer;
GRANT EXECUTE ON dbo.ChangePassword TO OrderApprover;
GRANT EXECUTE ON dbo.ChangePassword TO WarehouseEmployee;
GRANT EXECUTE ON dbo.ChangePassword TO Moderator;

go

EXEC dbo.ChangePassword '123';


select r.role_name from Users u, Roles r where u.role_id = r.role_id

go
--##############################################################################################################################################################
--không dùng
CREATE FUNCTION dbo.GetRoleByUserName ( @AccountName NVARCHAR(50) )
RETURNS NVARCHAR(30)
AS
BEGIN
    DECLARE @Role NVARCHAR(30);

    SELECT @Role = r.role_name 
    FROM Users u
    JOIN Roles r ON u.role_id = r.role_id
    WHERE u.AccountName = @AccountName;

    RETURN @Role;  -- Return the role name
END

go
Select dbo.GetRoleByUserName(N'hieu1') as RoleName

--gán quyền 
GRANT EXECUTE ON dbo.GetRoleByUserName TO Customer;


select * from  Users

SELECT 1 FROM sys.server_principals WHERE name = 'string'

select * from EmployeeType

--##############################################################################################################################################################
go
--Lấy ra quyền của người dùng login vào sqlserver
CREATE PROCEDURE GetRoleNameByCurrentUser
AS
BEGIN
    DECLARE @AccountName NVARCHAR(50);
    SET @AccountName = SUSER_NAME();
    
    SELECT r.role_name
    FROM Users u
    INNER JOIN Roles r ON r.role_id = u.role_id
    WHERE u.AccountName = @AccountName;
END
--gán quyền 
GRANT EXECUTE ON dbo.GetRoleNameByCurrentUser TO Customer;
GRANT EXECUTE ON dbo.GetRoleNameByCurrentUser TO OrderApprover;
GRANT EXECUTE ON dbo.GetRoleNameByCurrentUser TO WarehouseEmployee;
GRANT EXECUTE ON dbo.GetRoleNameByCurrentUser TO Moderator;

select * From Users u, Roles r where u.role_id = r.role_id

--thực thi
EXEC GetRoleNameByCurrentUser;

--##############################################################################################################################################################
--Register -- Tạo 1 tài khoản Khách hàng
CREATE PROCEDURE CreateCustomer
    @AccountName NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    -- Tạo Login
    DECLARE @Sql NVARCHAR(MAX)
    SET @Sql = 'CREATE LOGIN [' + @AccountName + '] WITH PASSWORD = ''' + @Password + ''';'
    EXEC sp_executesql @Sql

    -- Tạo User trong database hiện tại
    SET @Sql = 'CREATE USER [' + @AccountName + '] FOR LOGIN [' + @AccountName + '];'
    EXEC sp_executesql @Sql

    -- Gán quyền mặc định cho User (quyền Khách Hàng)
    EXEC sp_addrolemember N'Customer', @AccountName

	 -- Gán Login vào server role 'CustomerServerRole'
    SET @Sql = 'ALTER SERVER ROLE CustomerServerRole ADD MEMBER [' + @AccountName + '];'
    EXEC sp_executesql @Sql

    -- Lưu thông tin vào bảng Users
    INSERT INTO Users (AccountName, role_id)
    VALUES (@AccountName, 1)

	DECLARE @InsertedCustomerID TABLE (customerId INT);
	--thêm vào Customer
	insert into Customer(type_customer_id)
	OUTPUT INSERTED.customerId INTO @InsertedCustomerID
	values(1) -- Loại Khách Hàng là Khách Hàng mới

	DECLARE @customerId INT;
	Select @customerId = customerId from @InsertedCustomerID;

	--thêm vào UserInfo
	insert into UserInfo (AccountName, customer_Id)
	values (@AccountName, @customerId);

    -- Thông báo hoàn thành
    PRINT N'Đăng ký khách hàng thành công!'
END

select * from Users
select * from UserInfo

EXEC CreateCustomer 'hieu3', '123@@@';

go

--##############################################################################################################################################################
--Register -- Tạo 1 tài khoản người điều hành
CREATE PROCEDURE CreateModerator
    @AccountName NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    -- Tạo Login
    DECLARE @Sql NVARCHAR(MAX)
    SET @Sql = 'CREATE LOGIN [' + @AccountName + '] WITH PASSWORD = ''' + @Password + ''';'
    EXEC sp_executesql @Sql

    -- Tạo User trong database hiện tại
    SET @Sql = 'CREATE USER [' + @AccountName + '] FOR LOGIN [' + @AccountName + '];'
    EXEC sp_executesql @Sql

    -- Gán quyền cho User (quyền Người điều hành)
    EXEC sp_addrolemember N'Moderator', @AccountName

	 -- Gán Login vào server role 'CustomerServerRole' -- được phép sửa xóa login sqlserver
    SET @Sql = 'ALTER SERVER ROLE CustomerServerRole ADD MEMBER [' + @AccountName + '];'
    EXEC sp_executesql @Sql

    -- Lưu thông tin vào bảng Users -- user này có table role là Moderator
    INSERT INTO Users (AccountName, role_id)
    VALUES (@AccountName, 3)

	DECLARE @InsertedEmployeeID TABLE (employeeId INT);
	--thêm vào Employee
	insert into Employee(EmployeeTypeID, DepartmentID)
	OUTPUT INSERTED.EmployeeID INTO @InsertedEmployeeID
	values(6, 2) -- Moderator và Phòng IT

	DECLARE @employeeId INT;
	Select @employeeId = employeeId from @InsertedEmployeeID;

	--thêm vào UserInfo
	insert into UserInfo (AccountName, Employ_ID)
	values (@AccountName, @employeeId);

    -- Thông báo hoàn thành
    PRINT N'Đăng ký nhân viên điều hành thành công!'
END


EXEC CreateModerator 'HiuModerator', '123@';

select * from Roles

select u.AccountName, et.EmployeeTypeName, d.DepartmentName, r.role_name
From Users u, UserInfo uf, Employee e, EmployeeType et, Department d , Roles r
where u.AccountName = uf.AccountName and uf.Employ_ID = e.EmployeeID and et.EmployeeTypeID = e.EmployeeTypeID and e.DepartmentID = d.DepartmentID and u.role_id = r.role_id

select * from EmployeeType

go
--##############################################################################################################################################################
--Register -- Tạo 1 tài khoản nhân viên quản lí kho
CREATE PROCEDURE CreateWarehouseEmployee
    @AccountName NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    -- Tạo Login
    DECLARE @Sql NVARCHAR(MAX)
    SET @Sql = 'CREATE LOGIN [' + @AccountName + '] WITH PASSWORD = ''' + @Password + ''';'
    EXEC sp_executesql @Sql

    -- Tạo User trong database hiện tại
    SET @Sql = 'CREATE USER [' + @AccountName + '] FOR LOGIN [' + @AccountName + '];'
    EXEC sp_executesql @Sql

    -- Gán quyền cho User (quyền nhân viên quản lý kho)
    EXEC sp_addrolemember N'WarehouseEmployee', @AccountName

	 -- Gán Login vào server role 'CustomerServerRole' -- được phép sửa xóa login sqlserver
    SET @Sql = 'ALTER SERVER ROLE CustomerServerRole ADD MEMBER [' + @AccountName + '];'
    EXEC sp_executesql @Sql

    -- Lưu thông tin vào bảng Users -- user này có table role là Moderator
    INSERT INTO Users (AccountName, role_id)
    VALUES (@AccountName, 4)

	DECLARE @InsertedEmployeeID TABLE (employeeId INT);
	--thêm vào Employee
	insert into Employee(EmployeeTypeID, DepartmentID)
	OUTPUT INSERTED.EmployeeID INTO @InsertedEmployeeID
	values(4, 3) -- Warehouse Employee và Kho A

	DECLARE @employeeId INT;
	Select @employeeId = employeeId from @InsertedEmployeeID;

	--thêm vào UserInfo
	insert into UserInfo (AccountName, Employ_ID)
	values (@AccountName, @employeeId);

    -- Thông báo hoàn thành
    PRINT N'Đăng ký nhân viên quản lý kho thành công!'
END

EXEC CreateWarehouseEmployee 'Hiu1WarehouseEmployee', '123@';
go
--##############################################################################################################################################################
--Register -- Tạo 1 tài khoản nhân viên duyệt đơn
CREATE PROCEDURE CreateOrderApprover
    @AccountName NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    -- Tạo Login
    DECLARE @Sql NVARCHAR(MAX)
    SET @Sql = 'CREATE LOGIN [' + @AccountName + '] WITH PASSWORD = ''' + @Password + ''';'
    EXEC sp_executesql @Sql

    -- Tạo User trong database hiện tại
    SET @Sql = 'CREATE USER [' + @AccountName + '] FOR LOGIN [' + @AccountName + '];'
    EXEC sp_executesql @Sql

    -- Gán quyền cho User (quyền nhân viên quản lý kho)
    EXEC sp_addrolemember N'OrderApprover', @AccountName

	 -- Gán Login vào server role 'CustomerServerRole' -- được phép sửa xóa login sqlserver
    SET @Sql = 'ALTER SERVER ROLE CustomerServerRole ADD MEMBER [' + @AccountName + '];'
    EXEC sp_executesql @Sql

    -- Lưu thông tin vào bảng Users -- user này có table role là Order Approver
    INSERT INTO Users (AccountName, role_id)
    VALUES (@AccountName, 10)

	DECLARE @InsertedEmployeeID TABLE (employeeId INT);
	--thêm vào Employee
	insert into Employee(EmployeeTypeID, DepartmentID)
	OUTPUT INSERTED.EmployeeID INTO @InsertedEmployeeID
	values(5, 4) -- Warehouse Employee và Kho A

	DECLARE @employeeId INT;
	Select @employeeId = employeeId from @InsertedEmployeeID;

	--thêm vào UserInfo
	insert into UserInfo (AccountName, Employ_ID)
	values (@AccountName, @employeeId);

    -- Thông báo hoàn thành
    PRINT N'Đăng ký nhân viên duyệt đơn thành công!'
END

EXEC CreateOrderApprover 'HiusOrderApprover', '123@@';

--#########################################################################PROCEDURE GET ALL PRODUCT DELETIME IS NULL#####################################################################################
go

CREATE PROCEDURE GetAllProducts
AS
BEGIN
	select p.* , ph.price, ph.priceHistoryId
	from Product p, PriceHistory ph 
	where p.product_id = ph.product_id and ph. isActive = 0 and totalQuantity > 0 and p.DeleteTime is NULL
	Order By p.CreateTime Desc
END;

exec GetAllProducts;

GRANT EXECUTE ON GetAllProducts TO Customer;
GRANT EXECUTE ON GetAllProducts TO OrderApprover;
GRANT EXECUTE ON GetAllProducts TO Moderator;
GRANT EXECUTE ON GetAllProducts TO WarehouseEmployee;
--#########################################################################PROCEDURE GET CATE BY PRODUCT NAME DELETIME IS NULL#####################################################################################
go
CREATE FUNCTION dbo.GetCategoryName (
    @ProductID INT
)
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @CategoryName NVARCHAR(100);

    SELECT @CategoryName = c.category_name
    FROM Category c
    JOIN Product p ON c.category_id = p.Category_id
    WHERE p.product_id = @ProductID AND c.DeleteTime IS NULL;

    RETURN @CategoryName;
END;

SELECT dbo.GetCategoryName(3) AS CategoryName;

GRANT EXECUTE ON GetCategoryName TO Customer;
GRANT EXECUTE ON GetCategoryName TO OrderApprover;
GRANT EXECUTE ON GetCategoryName TO Moderator;
GRANT EXECUTE ON GetCategoryName TO WarehouseEmployee;
--#########################################################################PROCEDURE GET SUB CATE BY PRODUCT NAME DELETIME IS NULL#####################################################################################
go
CREATE FUNCTION dbo.GetSubCategoryName (
    @ProductID INT
)
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @SubCategoryName NVARCHAR(100);

    SELECT @SubCategoryName = sc.SubCategoryName
    FROM SubCategory sc
    JOIN Product p ON sc.SubCategoryID = p.SubCategoryID
    WHERE p.product_id = @ProductID AND sc.DeleteTime IS NULL;

    RETURN @SubCategoryName;
END;


go

SELECT dbo.GetSubCategoryName(3)

GRANT EXECUTE ON GetSubCategoryName TO Customer;
GRANT EXECUTE ON GetSubCategoryName TO OrderApprover;
GRANT EXECUTE ON GetSubCategoryName TO Moderator;
GRANT EXECUTE ON GetSubCategoryName TO WarehouseEmployee;
--#########################################################################PROCEDURE GET TOP 10 SUB CATE BY PRODUCT NAME DELETIME IS NULL#####################################################################################
go

CREATE FUNCTION dbo.GetTop10SubCategories ()
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 10 sc.*
	FROM SubCategory sc
	WHERE sc.DeleteTime IS NULL
	Order by sc.CreateTime
);

SELECT * FROM dbo.GetTop10SubCategories()

GRANT SELECT ON OBJECT::dbo.GetTop10SubCategories TO  Customer;
GRANT SELECT ON OBJECT::dbo.GetTop10SubCategories TO  OrderApprover;
GRANT SELECT ON OBJECT::dbo.GetTop10SubCategories TO  Moderator;
GRANT SELECT ON OBJECT::dbo.GetTop10SubCategories TO  WarehouseEmployee;
--#########################################################################PROCEDURE GET TOP 10 CATE BY PRODUCT NAME DELETIME IS NULL#####################################################################################
go
CREATE FUNCTION dbo.GetTop10Categories ()
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 10 c.*
	FROM Category c
	WHERE c.DeleteTime IS NULL
	Order by c.CreateTime
);

SELECT * FROM dbo.GetTop10Categories()

GRANT SELECT ON OBJECT::dbo.GetTop10Categories TO  Customer;
GRANT SELECT ON OBJECT::dbo.GetTop10Categories TO  OrderApprover;
GRANT SELECT ON OBJECT::dbo.GetTop10Categories TO  Moderator;
GRANT SELECT ON OBJECT::dbo.GetTop10Categories TO  WarehouseEmployee;
--#########################################################################PROCEDURE GET Supplier BY ID#####################################################################################
go
CREATE FUNCTION dbo.GetSupplierById (
    @SupplierID INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT * 
    FROM Supplier 
    WHERE SupplierID = @SupplierID 
      AND DeleteTime IS NULL
);


select * from dbo.GetSupplierById(11)

GRANT SELECT ON OBJECT::dbo.GetSupplierById TO  Customer;
GRANT SELECT ON OBJECT::dbo.GetSupplierById TO  OrderApprover;
GRANT SELECT ON OBJECT::dbo.GetSupplierById TO  Moderator;
GRANT SELECT ON OBJECT::dbo.GetSupplierById TO  WarehouseEmployee;
go
--#########################################################################Create Cart Customer#####################################################################################
CREATE PROCEDURE SP_AddToCart
    @ProductID INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserName VARCHAR(25);
    SET @UserName = SUSER_NAME();

    DECLARE @CustomerID INT;

    -- Gán giá trị cho biến @CustomerID
    SELECT @CustomerID = c.customerId
    FROM UserInfo uf
    JOIN Customer c ON uf.customer_Id = c.customerId
    WHERE uf.AccountName = @UserName;

    -- Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
    IF EXISTS (SELECT 1 FROM Cart WHERE customerId = @CustomerID AND product_id = @ProductID)
    BEGIN
        -- Nếu tồn tại, cập nhật số lượng
        UPDATE Cart
        SET quantity = quantity + @Quantity
        WHERE customerId = @CustomerID AND product_id = @ProductID;
    END
    ELSE
    BEGIN
        -- Nếu chưa tồn tại, chèn bản ghi mới
        INSERT INTO Cart (customerId, product_id, quantity)
        VALUES (@CustomerID, @ProductID, @Quantity);
    END
END;


--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_AddToCart TO  Customer;

EXEC SP_AddToCart @ProductID = 4, @Quantity = 1
go
--#########################################################################Select Cart Customer#####################################################################################
CREATE PROCEDURE SP_GetCartByUser
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserName VARCHAR(25);
	SET @UserName = SUSER_NAME();

	DECLARE @CustomerID INT;

	-- Gán giá trị cho biến @CustomerID
	SELECT @CustomerID = c.customerId
	FROM UserInfo uf
	JOIN Customer c ON uf.customer_Id = c.customerId
	WHERE uf.AccountName = @UserName;

	-- Truy vấn giỏ hàng của khách hàng
	SELECT p.product_id, p.product_name, ph.price, ph.priceHistoryId, c.quantity, p.image
	FROM Cart c
	Join Product p on c.product_id = p.product_id
	join PriceHistory ph on ph.product_id = p.product_id
	WHERE c.customerId = @CustomerID;
END;

--gán quyền
GRANT EXECUTE ON OBJECT::dbo.SP_GetCartByUser TO Customer;

EXEC SP_GetCartByUser;
go
--#########################################################################Drop Cart Customer#####################################################################################
CREATE PROCEDURE SP_DeleteProductFromCart
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserName VARCHAR(25);
    SET @UserName = SUSER_NAME();

    DECLARE @CustomerID INT;

    -- Gán giá trị cho biến @CustomerID
    SELECT @CustomerID = c.customerId
    FROM UserInfo uf
    JOIN Customer c ON uf.customer_Id = c.customerId
    WHERE uf.AccountName = @UserName;

    -- Xóa sản phẩm từ giỏ hàng
    DELETE FROM Cart
    WHERE product_id = @ProductID AND customerId = @CustomerID;
END;

--gán quyền
GRANT EXECUTE ON OBJECT::dbo.SP_DeleteProductFromCart TO Customer;

EXEC SP_DeleteProductFromCart @ProductID = 1;
go
--#########################################################################Update Cart Customer#####################################################################################
CREATE PROCEDURE SP_UpdateCart
    @ProductID INT,
    @Quantity INT
AS
BEGIN
    DECLARE @UserName VARCHAR(25); 
	SET @UserName = SUSER_NAME(); 

	DECLARE @CustomerID INT;

	SELECT @CustomerID = c.customerId 
	FROM UserInfo uf 
	JOIN Customer c ON uf.customer_Id = c.customerId 
	WHERE uf.AccountName = @UserName;

	UPDATE Cart
	SET quantity = @Quantity
	WHERE customerId = @CustomerID and product_id = @ProductID
END;


--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_UpdateCart TO  Customer;

EXEC SP_UpdateCart @ProductID = 4, @Quantity = 1
go
--#########################################################################Create Order + Order Detail Customer#####################################################################################
CREATE TYPE ProductQuantityType AS TABLE
(
    PriceHistoryId INT,
    Quantity INT
);
GO
CREATE PROCEDURE SP_InsertOrderWithDetails
    @Phone NVARCHAR(11),
    @Address_ID INT,
    @Name_Recipient NVARCHAR(50),
    @ProductQuantities ProductQuantityType READONLY
AS
BEGIN
    DECLARE @IdCustomer INT;
    DECLARE @AccountName NVARCHAR(50);
    DECLARE @OrderID INT;
    DECLARE @TotalPayment DECIMAL(10, 0);

    -- Lấy tên tài khoản hiện tại
    SET @AccountName = SUSER_NAME();

    -- Lấy customer_Id từ bảng Customer
    SELECT @IdCustomer = c.customerId
    FROM UserInfo uf
    JOIN Customer c ON c.customerId = uf.customer_Id
    WHERE uf.AccountName = @AccountName;

    -- Kiểm tra xem IdCustomer có giá trị hay không
    IF @IdCustomer IS NOT NULL
    BEGIN
        -- Tính tổng số tiền
        SELECT @TotalPayment = SUM(ph.price * pq.Quantity)
        FROM @ProductQuantities pq
        JOIN PriceHistory ph ON ph.priceHistoryId = pq.PriceHistoryId;

        -- Chèn dữ liệu vào bảng Order và lấy OrderID mới tạo
        INSERT INTO [Order] (Phone, Adress_ID, Name_Recipient, CreateBy, Total_Payment)
        VALUES (@Phone, @Address_ID, @Name_Recipient, @IdCustomer, @TotalPayment);

        SET @OrderID = SCOPE_IDENTITY();

        -- Chèn dữ liệu vào bảng OrderDetail
        INSERT INTO OrderDetail (Order_Id, priceHistoryId, Quantity)
        SELECT @OrderID, pq.PriceHistoryId AS ProductID, pq.Quantity
        FROM @ProductQuantities pq;
    END
    ELSE
    BEGIN
        PRINT 'User does not have a corresponding customer record.';
    END
END;


--phân quyền
GRANT EXECUTE ON TYPE::dbo.ProductQuantityType TO Customer;
GRANT EXECUTE ON OBJECT::dbo.SP_InsertOrderWithDetails TO  Customer;
--RUN EXAMPLE

--#########################################################################Update User Info#####################################################################################
CREATE PROCEDURE SP_UpdateUserInfo
    @FullName NVARCHAR(100),
    @Phone VARCHAR(20),
    @Email VARCHAR(100),
    @AddressID INT,
    @Gender INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserName VARCHAR(25);
    SET @UserName = SUSER_NAME();
    
    -- Hiển thị tên người dùng
    SELECT @UserName AS UserName;

    -- Cập nhật thông tin người dùng
    UPDATE UserInfo
    SET full_name = @FullName, 
        phone = @Phone, 
        email = @Email, 
        address_id = @AddressID, 
        gender = @Gender,
		ModifiedBy = @UserName,
		ModifiedTime = GETDATE()
    WHERE AccountName = @UserName;
END;
--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_UpdateUserInfo TO  Customer;

--RUN
EXEC SP_UpdateUserInfo @FullName = N'Tên đầy đủu', @Phone = '09876543210', @Email = 'email@example.com', @AddressID = 1, @Gender = 1;
--#########################################################################GET User Info#####################################################################################
CREATE PROCEDURE SP_GetUserInfoByUserName
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserName VARCHAR(25);
	SET @UserName = SUSER_NAME();

	SELECT 
	uf.AccountName as 'Tên Đăng Nhập',
	uf.full_name as 'Họ Tên', 
	uf.email as 'Email', 
	uf.phone as 'Số Điện Thoại',
	uf.gender AS N'Giới Tính',
	ct.type_customer_name as N'Loại Khách Hàng',
	a.AddressID AS N'Địa Chỉ ID',
	a.CommuneID AS N'Xã',
	co.DistrictID AS N'Quận',
	di.ProvinceID AS N'Tỉnh',
	a.Note + N', Xã ' + co.CommuneName + N', Quận/Huyện ' + di.DistrictName + N', Tỉnh/Thành Phố ' + pr.ProvinceName as N'Địa Chỉ'
	FROM UserInfo uf
	JOIN Customer c ON uf.customer_Id = c.customerId
	JOIN CustomerType ct ON c.type_customer_id = ct.type_customer_id
	JOIN Address a ON a.AddressID = uf.address_id
	JOIN Commune co ON a.CommuneID = co.CommuneID
	JOIN District di ON co.DistrictID = di.DistrictID
	JOIN Province pr ON di.ProvinceID = pr.ProvinceID
	WHERE uf.AccountName = @UserName;
END;
--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_GetUserInfoByUserName TO  Customer;

--RUN
EXEC SP_GetUserInfoByUserName;
go
--#########################################################################GET All Address#####################################################################################
CREATE PROCEDURE SP_GetFullAddress
AS
BEGIN
	DECLARE @UserName VARCHAR(25);
	SET @UserName = SUSER_NAME();
    
	-- Khai báo biến kiểu TABLE
	DECLARE @AddressTable TABLE (
		AddressID INT
	);

	-- Chèn dữ liệu vào biến @AddressTable từ câu lệnh SELECT đầu tiên
	INSERT INTO @AddressTable (AddressID)
	SELECT DISTINCT uf.address_id
	FROM UserInfo uf
	WHERE uf.AccountName = @UserName;

	-- Chèn dữ liệu vào biến @AddressTable từ câu lệnh SELECT thứ hai
	INSERT INTO @AddressTable (AddressID)
	SELECT DISTINCT o.Adress_ID
	FROM UserInfo uf
	JOIN [Order] o ON o.CreateBy = uf.customer_Id
	WHERE uf.AccountName = @UserName;

	-- Truy vấn và trả về kết quả đầy đủ của địa chỉ
	SELECT a.AddressID ,
		a.Note + N', Xã ' + co.CommuneName + N', Huyện ' + di.DistrictName + N', Tỉnh ' + pr.ProvinceName AS N'Địa Chỉ'
	FROM @AddressTable ad
	JOIN Address a ON a.AddressID = ad.AddressID
	JOIN Commune co ON a.CommuneID = co.CommuneID
	JOIN District di ON co.DistrictID = di.DistrictID
	JOIN Province pr ON di.ProvinceID = pr.ProvinceID;
END;

--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_GetFullAddress TO  Customer;
--RUN
EXEC SP_GetFullAddress;
go
--#########################################################################GET User Province#####################################################################################
CREATE PROCEDURE SP_GetProvinces
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProvinceID, ProvinceName
    FROM Province;
END;

--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_GetProvinces TO  Customer;
--RUN
EXEC SP_GetProvinces;
go
--#########################################################################GET User Communers#####################################################################################
CREATE PROCEDURE SP_GetCommunes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CommuneID, CommuneName, DistrictID
    FROM Commune;
END;
--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_GetCommunes TO  Customer;
--RUN
EXEC SP_GetCommunes;
go
--#########################################################################GET User Communers by ID District#####################################################################################
CREATE PROCEDURE SP_GetCommunesByDistrictID
@DistrictID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CommuneID, CommuneName, DistrictID
    FROM Commune
	WHERE DistrictID = @DistrictID
END;
--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_GetCommunesByDistrictID TO  Customer;
--RUN
EXEC SP_GetCommunesByDistrictID @DistrictID = 8
go
--#########################################################################GET User District#####################################################################################
CREATE PROCEDURE SP_GetDistrict
AS
BEGIN
    SET NOCOUNT ON;

    select DistrictID, DistrictName, ProvinceID from District
END;
--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_GetDistrict TO  Customer;
--RUN
EXEC SP_GetDistrict;
go
--#########################################################################GET User District by ProvinceID#####################################################################################
CREATE PROCEDURE SP_GetDistrictByProvinceID
@ProvinceID INT
AS
BEGIN
    SET NOCOUNT ON;

    select DistrictID, DistrictName, ProvinceID from District where ProvinceID = @ProvinceID;
END;
--phân quyền
GRANT EXECUTE ON OBJECT::dbo.SP_GetDistrictByProvinceID TO  Customer;
--RUN
EXEC SP_GetDistrictByProvinceID @ProvinceID = 62
--#########################################################################GET ADDRESS BY ID#####################################################################################
go
CREATE PROCEDURE SP_GetAddressById
    @AddressId INT
WITH EXECUTE AS OWNER
AS
BEGIN
    SELECT * 
    FROM Address 
    WHERE AddressID = @AddressId;
END;

GRANT EXECUTE ON SP_GetAddressById TO Customer;

EXEC SP_GetAddressById @AddressId = 1;
go
--#########################################################################INSERT COMMUNE#####################################################################################
CREATE PROCEDURE InsertCommune
    @CommuneName NVARCHAR(255),
    @DistrictID INT
AS
BEGIN
    -- Bắt đầu một giao dịch
    BEGIN TRANSACTION;

    DECLARE @NewID TABLE (ID INT);

    -- Chèn bản ghi mới vào bảng Commune và lấy ID vào bảng tạm @NewID
    INSERT INTO Commune (CommuneName, DistrictID)
    OUTPUT INSERTED.CommuneID INTO @NewID(ID)
    VALUES (@CommuneName, @DistrictID);

    -- Kết thúc giao dịch
    COMMIT TRANSACTION;

    -- Trả về ID vừa chèn
    SELECT ID FROM @NewID;
END;

--PHân quyền
GRANT EXEC ON OBJECT::dbo.InsertCommune TO  Customer;
GRANT EXEC ON OBJECT::dbo.InsertCommune TO  OrderApprover;
GRANT EXEC ON OBJECT::dbo.InsertCommune TO  Moderator;
GRANT EXEC ON OBJECT::dbo.InsertCommune TO  WarehouseEmployee;

--RUN
EXEC InsertCommune
    @CommuneName = N'Tên xã',
    @DistrictID = 1;
go
--#########################################################################INSERT ADDRESS#####################################################################################
CREATE PROCEDURE InsertAddress
    @CommuneID INT,
    @HouseNumber NVARCHAR(255),
    @Note NVARCHAR(MAX)
AS
BEGIN
    -- Bắt đầu một giao dịch
    BEGIN TRANSACTION;

    DECLARE @NewID TABLE (ID INT);

    -- Chèn bản ghi mới vào bảng Address và lấy ID vào bảng tạm @NewID
    INSERT INTO Address (CommuneID, HouseNumber, Note)
    OUTPUT INSERTED.AddressID INTO @NewID(ID)
    VALUES (@CommuneID, @HouseNumber, @Note);

    -- Kết thúc giao dịch
    COMMIT TRANSACTION;

    -- Trả về ID vừa chèn
    SELECT ID FROM @NewID;
END;

--PHân quyền
GRANT EXEC ON OBJECT::dbo.InsertAddress TO  Customer;
GRANT EXEC ON OBJECT::dbo.InsertAddress TO  OrderApprover;
GRANT EXEC ON OBJECT::dbo.InsertAddress TO  Moderator;
GRANT EXEC ON OBJECT::dbo.InsertAddress TO  WarehouseEmployee;
--RUN
go
--#########################################################################GET ADDRESS By Commmune, Note, HouseNumber#####################################################################################
CREATE PROCEDURE GetAddressID
    @CommuneID INT,
    @HouseNumber NVARCHAR(30),
    @Note NVARCHAR(50)
AS
BEGIN
    -- Chọn AddressID từ bảng Address dựa trên các điều kiện đầu vào
    SELECT AddressID
    FROM Address
    WHERE CommuneID = @CommuneID
      AND HouseNumber = @HouseNumber
      AND Note = @Note;
END;

--PHân quyền
GRANT EXEC ON OBJECT::dbo.GetAddressID TO  Customer;
GRANT EXEC ON OBJECT::dbo.GetAddressID TO  OrderApprover;
GRANT EXEC ON OBJECT::dbo.GetAddressID TO  Moderator;
GRANT EXEC ON OBJECT::dbo.GetAddressID TO  WarehouseEmployee;
--RUN
EXEC GetAddressID @CommuneID = 15,  @HouseNumber = N'String', @Note = N'string'

