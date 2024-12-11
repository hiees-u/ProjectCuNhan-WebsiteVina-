CREATE DATABASE CAFFEE_VINA_DB
go

USE CAFFEE_VINA_DB

go

--User
CREATE TABLE Users (
	AccountName varchar(25) NOT NULL PRIMARY KEY,
	password varchar(15) NOT NULL,
	role_id int NOT NULL,
	Type_customer_id int,
	CreateAt DateTime NOT NULL,
)

--Cart
CREATE TABLE Cart (
	AccountName varchar(25) NOT NULL,
	product_id int NOT NULL,
	quantity int Default 0,
	Constraint PK_Cart PRIMARY KEY(AccountName, product_id) --PK
);

GO


--Product
CREATE TABLE PRODUCT (
	product_id int IDENTITY(1,1) NOT NULL Primary key,
	product_name nvarchar(50) NOT NULL,
	image varchar(50),
	totalQuantity int default 0,
	Category_id int NOT NULL, --fk
	Manufacturer int  NOT NULL --fk
);

GO

-- User_Info
CREATE TABLE User_Info (
    AccountName varchar(25) PRIMARY KEY, --FK
    full_name NVARCHAR(50),
    email NVARCHAR(50),
    address_id int NOT NULL, --FK
    phone VARCHAR(11),
    gender NVARCHAR(4),
	Employ_ID int, --FK
);

GO

--Roles
CREATE TABLE Roles (
    role_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    role_name NVARCHAR(30) NOT NULL
);

--Customer_type
CREATE TABLE CustomerType (
    type_customer_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    type_customer_name NVARCHAR(30) NOT NULL
);

GO

--Category
CREATE TABLE Category (
    category_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    category_name NVARCHAR(30) NOT NULL,
    Image VARCHAR(50)
);

--ORDER
CREATE TABLE ORDER_(
	Order_ID INT IDENTITY(1,1) PRIMARY KEY,
	Phone NVARCHAR(11) NOT NULL,
	Adress_ID int NOT NULL, -- fk
	Name_Recipient NVARCHAR(50) NOT NULL,
	CreateBy VARCHAR(25) NOT NULL, --FK account_name
	Total_Payment DECIMAL(10, 0) NOT NULL,
	Create_At DATETIME NOT NULL,
	State nvarchar(20) NOT NULL,
	Check (State in 
		(N'chờ xác nhận', N'đã xác nhận', N'đang chuẩn bị hàng', N'đang vận chuyển', N'đã giao hàng', N'đã hủy')
	) -- Chỉ cho phép các giá trị này
);

GO

--ORDER_DETAIL
CREATE TABLE ORDER_DETAIL(
	Order_Id INT, --FK
	Product_Id INT, -- FK
	Quantity INT NOT NULL,
	CONSTRAINT FK_ORDER_DETAIL PRIMARY KEY(Order_Id, Product_Id)
);

GO

--Transaction
CREATE TABLE Transactions (
	LocalTransactionId INT IDENTITY(1,1) PRIMARY KEY,
	OrderId INT NOT NULL, -- FK
	PaymentMethod NVARCHAR(255) NOT NULL,
	Information NVARCHAR(500) NOT NULL,
	TransactionId NVARCHAR(255) NOT NULL,
	State NVARCHAR(255) NOT NULL,
	CreateAt DateTime NOT NULL,
)

Create table Employee 
(
	EmployeeID int IDENTITY(1,1) PRIMARY KEY,
	EmployeeTypeID int Not null, -- FK
	DepartmentID int NOT NULL -- FK
);

GO

Create table Department 
(
	DepartmentID int IDENTITY(1,1) PRIMARY KEY,
	DepartmentName nvarchar(30) Not null
);

GO

Create table EmployeeType 
(
	EmployeeTypeID int IDENTITY(1,1) PRIMARY KEY,
	EmployeeTypeName nvarchar(30) Not null
);

GO

Create table Address 
(
	AddressID int IDENTITY(1,1) PRIMARY KEY,
	ProvinceID int NOT NULL, -- FK
	DistrictID int NOT NULL, --FK
	CommuneID int NOT NULL, --FK
	HouseNumber nvarchar(10) NOT NULL,
	Note nvarchar(50)
);

GO

Create table Manufacturer 
(
	ManufacturerID int IDENTITY(1,1) PRIMARY KEY,
	ManufacturerName nvarchar(30) NOT NULL,
	AddressID int NOT NULL, --fk
);

GO

Create table Warehouse
(
	WarehouseID int IDENTITY(1,1) PRIMARY KEY,
	WarehouseName nvarchar(30) NOT NULL,
	AddressID int NOT NULL, --fk
);

GO

Create table Province
(
	ProvinceID int IDENTITY(1,1) PRIMARY KEY,
	ProvinceName nvarchar(30) NOT NULL,
);

GO

Create table District 
(
	DistrictID int IDENTITY(1,1) PRIMARY KEY,
	DistrictName nvarchar(30) NOT NULL,
	ProvinceID int NOT NULL, --FK
);

GO

Create table Commune
(
	CommuneID int IDENTITY(1,1) PRIMARY KEY,
	CommuneName nvarchar(30) NOT NULL,
	DistrictID int NOT NULL, -- FK
);

GO

Create table Shelve 
(
	ShelvesID int IDENTITY(1,1) PRIMARY KEY,
	ShelvesName nvarchar(30) NOT NULL,
	WarehouseID int NOT NULL, -- FK
);


GO

Create table Cells
(
	CellID int IDENTITY(1,1) PRIMARY KEY,
	CellName nvarchar(30) NOT NULL,
	ShelvesID int NOT NULL,
	Quantity int Default 0,
	product_id int NOT NULL, -- FK
);


--FK
go
alter table Users add constraint fk_User_Roles foreign key (role_id) references Roles(role_id);
alter table Users add constraint fk_User_type_customer foreign key (Type_customer_id) references CustomerType(type_customer_id);
alter table ORDER_DETAIL add constraint fk_Product_ORDER_DETAIL foreign key (Product_Id) references PRODUCT(product_id);
alter table ORDER_DETAIL add constraint fk_Order_ORDER_DETAIL foreign key (Order_Id) references ORDER_(Order_Id);
alter table Employee add constraint fk_Employee_EmployeeType foreign key (EmployeeTypeID) references EmployeeType(EmployeeTypeID);
alter table Employee add constraint fk_Employee_Department foreign key (DepartmentID) references Department(DepartmentID);
alter table Commune add constraint fk_Commune_DistrictID foreign key (DistrictID) references District(DistrictID);
alter table District add constraint fk_District_ProvinceID foreign key (ProvinceID) references Province(ProvinceID);
alter table Address add constraint fk_Address_ProvinceID foreign key (ProvinceID) references Province(ProvinceID);
alter table Address add constraint fk_Address_DistrictID foreign key (DistrictID) references District(DistrictID);
alter table Address add constraint fk_Address_CommuneID foreign key (CommuneID) references Commune(CommuneID);
alter table Manufacturer add constraint fk_Manufacturer_Address foreign key (AddressID) references Address(AddressID);
alter table Warehouse add constraint fk_Warehouse_AddressID foreign key (AddressID) references Address(AddressID);
alter table Shelve add constraint fk_Shelve_WarehouseID foreign key (WarehouseID) references Warehouse(WarehouseID);

alter table Cells add constraint fk_Cells_ProductID foreign key (product_id) references Product(product_id);
alter table ORDER_ add constraint fk_user_ORDER_ foreign key (CreateBy) references Users(AccountName);
alter table ORDER_ add constraint fk_Adress_ORDER foreign key (Adress_ID) references Address(AddressID);
alter table ORDER_ drop constraint fk_user_Adress;
alter table User_Info add constraint fk_user_if_Address foreign key (address_id) references Address(AddressID);
alter table User_Info add constraint fk_user_if_Employee foreign key (Employ_ID) references Employee(EmployeeID);
alter table User_Info add constraint fk_user_if_user foreign key (AccountName) references Users(AccountName);
alter table PRODUCT add constraint fk_Category_PRODUCT foreign key (category_id) references Category(category_id);
alter table Cart add constraint fk_product_cart foreign key (product_id) references PRODUCT(product_id)
alter table Cart add constraint fk_user_cart foreign key (AccountName) references Users(AccountName)

--PurchaseOrder 
Create table PurchaseOrder (
	PurchaseOrderID int IDENTITY(1,1) PRIMARY KEY,
	AccountName varchar(25), 
	CreateAt DATE NOT NULL Default GETDATE(), -- ngày đặt default ngày hiện tại
    DeliveryDate DATE NOT NULL, -- ngày giao
    State NVARCHAR(30) NOT NULL 
		CHECK (State IN (N'Đã Giao', N'Đã Đặt', N'Chưa Giao Đủ', N'Đã Hủy')) 
		Default N'Đã Đặt'
)

--PurchaseOrder Detail
CREATE TABLE PurchaseOrderDetail (
    PurchaseOrderID int,
    productID int,
    quantity int, -- lớn hơn 0
    Address int, --"trigger address phải trùng với address của kho."
	State NVARCHAR(30) NOT NULL 
		CHECK (State IN (N'Đã Giao', N'Chưa Giao', N'Da Giao Mot Pham', N'Đã Hủy')) 
		Default  N'Chưa Giao',
	QuantityDelivered int check(QuantityDelivered >= 0) Default 0, -- số lượng đã giao
    CONSTRAINT PK_PurchaseOrderDetail PRIMARY KEY (PurchaseOrderID, productID)
);

alter table PurchaseOrderDetail
add constraint FK_PurchaseOrderDetail_PurchaseOrder foreign key (PurchaseOrderID) references PurchaseOrder(PurchaseOrderID)
alter table PurchaseOrderDetail
add constraint FK_PurchaseOrderDetail_Product foreign key (productID) references Product(product_id);

go

alter table PurchaseOrderDetail add constraint ckc_quantity_0 check (quantity > 0)

--Phiếu nhập kho
Create table WarehouseReceipt 
(
	WarehouseReceiptID int IDENTITY(1,1) PRIMARY KEY,
	AccountName varchar(25) NOT NULL,
	CreateAt Date NOT NULL Default GETDATE(), -- mặc định là ngày hiện tại
	WarehouseID int NOT NULL, --Nhập vào kho nào
);

alter table WarehouseReceipt add constraint fk_WarehouseReceipt_Warehouse foreign key (WarehouseID) references Warehouse(WarehouseID);

go

Create table WarehouseReceiptDetail
(
	WarehouseReceiptID int NOT NULL, -- FK PK 
	CellID int NOT NULL, -- PK FK  -- trigger để sản phẩm ở ô nào CELL ID phải thuộc mã kho = WarehouseReceipt.WarehouseID
	AccountName varchar(25) NOT NULL, -- người nhập vào 
	product_id int NOT NULL, -- FK
	quantity int default 0 CHECK(quantity > 0),
);

Create table SubCategory
(
	SubCategoryID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	SubCategoryName nvarchar(30) NOT NULL,
	Image varchar(50)
)

ALTER TABLE Product
ADD SubCategoryID int NOT NULL;

ALTER TABLE Product
ADD CONSTRAINT FK_PRODUCT_SubCategory FOREIGN KEY (SubCategoryID) REFERENCES SubCategory(SubCategoryID);

alter table WarehouseReceiptDetail add constraint fk_WarehouseReceiptDetail_Cells foreign key (CellID) references Cells(CellID);

alter table WarehouseReceiptDetail add constraint pk_WarehouseReceiptDetail primary key(WarehouseReceiptID, CellID)
alter table WarehouseReceiptDetail 
add constraint fk_WarehouseReceiptDetail_WarehouseReceipt 
foreign key (WarehouseReceiptID) references WarehouseReceipt(WarehouseReceiptID)
alter table WarehouseReceiptDetail add constraint fk_WarehouseReceiptDetail_Product foreign key (product_id) references Product(product_id);


alter table PRODUCT add constraint fk_Manufacturer_PRODUCT foreign key (Manufacturer) references Manufacturer(ManufacturerID);
--Manufacturer
alter table PurchaseOrderDetail add constraint fk_PurchaseOrderDetail_Address foreign key (Address) references Address(AddressID);

alter table Address drop constraint fk_Address_ProvinceID
alter table Address drop constraint fk_Address_DistrictID

alter table PurchaseOrder add constraint fk_PurchaseOrder_User foreign key (AccountName) references Users(AccountName);
alter table WarehouseReceipt add constraint fk_WarehouseReceipt_User foreign key (AccountName) references Users(AccountName);

go
--##########################################TRIGGER##########################################
--1.	Update Quantity Product
create trigger updateQuantityProduct
on Cells
after insert, update
as
begin
	update p
	set p.totalQuantity = (
		select sum(c.Quantity) from Cells c
		where c.product_id = p.product_id
	)
	from PRODUCT p
	where p.product_id in (
		select DISTINCT i.product_id
		from Inserted i
		UNION
		select Distinct d.product_id
		from Deleted d
	);
end;
go
go
--3.	trg_CheckAddressID
CREATE TRIGGER trg_CheckAddressID
ON PurchaseOrderDetail
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        LEFT JOIN Warehouse w ON i.Address = w.AddressID
        WHERE w.AddressID IS NULL
    )
    BEGIN
        RAISERROR('AddressID không hợp lệ. Vui lòng kiểm tra lại.', 16, 1)
        ROLLBACK TRANSACTION
    END
END


--##############################
-- 1. Drop old CHECK constraint (replace 'CK_PurchaseOrderDetail_State' with the actual name if necessary)
ALTER TABLE PurchaseOrderDetail
DROP CONSTRAINT CK__PurchaseO__State__55F4C372;
ALTER TABLE PurchaseOrderDetail
DROP CONSTRAINT DF__PurchaseO__State__56E8E7AB;

-- 2. Modify column data type from NVARCHAR to int
ALTER TABLE PurchaseOrderDetail
ALTER COLUMN State int NOT NULL;

-- 3. Add new CHECK constraint for the integer values of State
ALTER TABLE PurchaseOrderDetail
ADD CONSTRAINT CK_PurchaseOrderDetail_State
CHECK (State IN (0, 1, 2, 3));

-- Optional: Update the default value for State if needed
ALTER TABLE PurchaseOrderDetail
ADD CONSTRAINT DF_PurchaseOrderDetail_State
DEFAULT 1 FOR State;


ALTER TABLE PurchaseOrder
DROP CONSTRAINT DF__PurchaseO__State__531856C7;
ALTER TABLE PurchaseOrder
DROP CONSTRAINT CK__PurchaseO__State__5224328E;
ALTER TABLE PurchaseOrder
DROP COLUMN State;

--ORDER_

select * From [Order]

ALTER TABLE [Order]
ALTER COLUMN State int NOT NULL;

ALTER TABLE [Order]
ADD CONSTRAINT CK_Order_State
CHECK (State IN (0, 1, 2, 3, 4));

--WarehouseReceiptDetail

alter table WarehouseReceiptDetail
add PurchaseOrderID int NOT NULL

ALTER TABLE WarehouseReceiptDetail
ADD CONSTRAINT FK_WarehouseReceiptDetail_PurchaseOrder FOREIGN KEY (PurchaseOrderID, product_id) REFERENCES PurchaseOrderDetail(PurchaseOrderID, productID);

go

---
CREATE TRIGGER trg_WarehouseReceipt_CreateAt
ON WarehouseReceipt
AFTER INSERT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted i
               JOIN PurchaseOrder p ON i.AccountName = p.AccountName
               WHERE i.CreateAt < p.CreateAt)
    BEGIN
        RAISERROR('Ngày nhập kho phải lớn hơn hoặc bằng ngày đặt hàng.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END

--UNIQUE

alter table Commune
add constraint uni_CommuneName unique (CommuneName);

alter table Province
add constraint uni_ProvinceName unique (ProvinceName);

alter table District
add constraint uni_DistrictName unique (DistrictName);

alter table Manufacturer
add constraint uni_ManufacturerName unique (ManufacturerName);

alter table Category
add constraint uni_category_name unique (category_name);

alter table Product
add constraint uni_product_name unique (product_name);

alter table SubCategory 
add constraint uni_SubCategoryName unique (SubCategoryName);

alter table Cells  
add constraint uni_CellName unique (CellName);

alter table Roles   
add constraint uni_role_name unique (role_name);

alter table Product
add ExpriryDate Date 

go
create table DeliveryNote 
(
	DeliveryNoteID int IDENTITY(1,1) PRIMARY KEY,
	CreateAt Date Default GETDATE(),
	CreateBy varchar(25) NOT NULL,
	Customer varchar(25) NOT NULL,
	WarehouseId int NOT NULL, -- xuất tại kho nào
	Note nvarchar(50),
	constraint fk_DeliveryNote_CreateBy foreign key (CreateBy) references Users(AccountName),
	constraint fk_DeliveryNote_Warehouse foreign key (WarehouseId) references Warehouse(WarehouseID),
	constraint fk_DeliveryNote_Customer foreign key (Customer) references Users(AccountName),
)
go
Create table DeliveryNoteDetail
(
	DeliveryNote int NOT NULL,
	product_id int NOT NULL,
	CellID int NOT NULL,
	quantity int NOT NULL Default 1,
	constraint pk_DeliveryNoteDetail primary key (DeliveryNote, CellID),
	constraint fk_DeliveryNoteDetail_CellID foreign key (CellID) references Cells(CellID),
	constraint fk_DeliveryNoteDetail_DeliveryNote foreign key (DeliveryNote) references DeliveryNote(DeliveryNoteID),
	constraint fk_DeliveryNoteDetail_product foreign key (product_id) references Product(product_id),
)

drop table DeliveryNoteDetail;
drop table DeliveryNote;

--insert into
select * From Address

insert into Manufacturer (ManufacturerName, AddressID)
values (N'Hiếu Test',1);

insert into Manufacturer (ManufacturerName, AddressID)
values (N'Không Xác Định',1);

select * from Manufacturer

select m.ManufacturerName From Manufacturer m, Address a, District d, Commune c, Province p 
where m.AddressID = a.AddressID and a.CommuneID =c.CommuneID and c.DistrictID = d.DistrictID and d.ProvinceID = p.ProvinceID and p.ProvinceName = N'Tiền Giang'

--Category
insert into Category (category_name, Image) values (N'Hieu Test Cate', 'Deo co hinh anh');
insert into Category (category_name, Image) values (N'Không Xác Định', 'Deo co hinh anh');

select * from Category;

--Category
insert into SubCategory (SubCategoryName, Image) Values (N'Sub Cate Test', 'Deo co hinh anh');
insert into SubCategory (SubCategoryName, Image) Values (N'Không Xác Định', 'Deo co hinh anh');

select * from SubCategory
--Product
DBCC CHECKIDENT('Product', RESEED, -1);

insert into Product (product_name, image, Category_id, SubCategoryID, Manufacturer)
values (N'Không Xác Định', 'Khong Xac Dinh', 2, 2, 3);

select * from Product;

--Warehouse
insert into Warehouse (WarehouseName, AddressID) 
values (N'Kho Test đó', 1)

select * from Warehouse

--Shelves
insert into Shelve(ShelvesName, WarehouseID) 
values (N'Kệ 1A', 1)

select * from Shelve
--
insert into Cells (CellName, product_id, ShelvesID, Quantity)
values (N'Ô 1A-8', 0, 1, 1)

delete from Cells where CellID = 5

select * from Cells

--Roles
DBCC CHECKIDENT('Roles', RESEED, -1);
insert into Roles (role_name) values (N'Không Xác Định')
select * from Roles
--Account
insert into Users(AccountName, password, Type_customer_id, role_id)
values ('HieuTest', '123@', 0, 0 )

select * from Users

go
--CustomerType
DBCC CHECKIDENT('CustomerType', RESEED, -1);
insert into CustomerType (type_customer_name) values (N'Không Xác Định')
select * from CustomerType
go

--UserInfo
insert into UserInfo (AccountName, full_name, email, address_id, phone, gender,Employ_ID)
values ('HieuTest', N'Nguyễn Minh Hiếu', 'nguyenminhhieu.31018@gmail.com', 1, '0393370172', N'Nam', 1);

select * from UserInfo;

--EmployeeType 
insert into EmployeeType(EmployeeTypeName) 
values (N'Không Xác Định');
select * from EmployeeType;

--Department
insert into Department (DepartmentName)
values (N'Không Xác Định');

select * from Department;

--Employee
insert into Employee (EmployeeTypeID, DepartmentID)
values (1, 1)

select * From Employee

-- lấy địa chỉ
select a.HouseNumber, c.CommuneName, d.DistrictName, p.ProvinceName from Address a, Commune c, Province p, District d
where a.CommuneID = c.CommuneID and c.DistrictID = d.DistrictID and d.ProvinceID = p.ProvinceID

--cart
select *From Address

--PurchaseOrder
insert into PurchaseOrder(AccountName, DeliveryDate) values ('HieuTest', '2024/11/10')

select * From PurchaseOrder
select * from Product
--PurchaseOrderDetail
insert into PurchaseOrderDetail(PurchaseOrderID, productID, Address) values (2, 0, 2);

select * from PurchaseOrderDetail

insert into Address (CommuneID, HouseNumber) values (1, '18/11');

insert into WarehouseReceipt (AccountName, WarehouseID) 
values ('HieuTest', 1)

--PurchaseOrder
insert into PurchaseOrder (AccountName, DeliveryDate) values ('HieuTest', '2024/11/11')

select * from PurchaseOrder
select * from Product
select * from Address

--PurchaseOrderDetail
insert into PurchaseOrderDetail (PurchaseOrderID, productID, Address, State)
values (1, 0, 2, 1);

select * from PurchaseOrderDetail

select * from WarehouseReceiptDetail
select * from WarehouseReceipt
select * from Warehouse

select * from Cells

insert into WarehouseReceipt(AccountName, CreateAt, WarehouseID) 
values ('HieuTest', '2021/1/1', 1)

insert into WarehouseReceiptDetail(WarehouseReceiptID, CellID, product_id, quantity, PurchaseOrderID)
values (1, 6, 0, 9, 1);










drop trigger trg_WarehouseReceipt_CreateAt

go

CREATE TRIGGER TR_CheckWarehouseReceiptDetailCreateAt
ON WarehouseReceiptDetail
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @WarehouseReceiptCreateAt DATE;
    DECLARE @PurchaseOrderCreateAt DATE;
    DECLARE @WarehouseReceiptID INT;
    DECLARE @PurchaseOrderID INT;

    -- Lấy mã nhập kho
    SELECT @WarehouseReceiptID = i.WarehouseReceiptID
    FROM inserted i;

	-- Lấy đặt hàng
	select @PurchaseOrderID = i.PurchaseOrderID
	from inserted i;

    -- Lấy ngày nhập kho
    SELECT @WarehouseReceiptCreateAt = wr.CreateAt
    FROM WarehouseReceipt wr
    WHERE wr.WarehouseReceiptID = @WarehouseReceiptID;

    -- Lấy ngày đặt
    SELECT @PurchaseOrderCreateAt = po.CreateAt
    FROM PurchaseOrder po
    WHERE po.PurchaseOrderID = @PurchaseOrderID;

    IF @WarehouseReceiptCreateAt < @PurchaseOrderCreateAt
    BEGIN
        RAISERROR('Thời gian nhập kho phải lớn hơn hoặc bằng thời gian đặt hàng.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;

go


--trg_validate_cell_for_warehouse

/*
Kiểm tra Chi tiết xuất kho.
CellId có nằm trong kho được nhập ở Phiếu xuất kho hay không, 
kiểm tra cell đó có lưu sản phẩm và số lượng cần xuất không 
*/
go

create trigger trg_validate_cell_for_warehouse
on DeliveryNoteDetail
AFTER insert, update
as
begin
	
	 IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN DeliveryNote dn ON i.DeliveryNote = dn.DeliveryNoteId
		INNER JOIN Warehouse w ON w.WarehouseID = dn.WarehouseID
		INNER JOIN Shelve s ON s.WarehouseID = w.WarehouseID
        INNER JOIN Cells c ON 
			i.CellId = c.CellId and 
			c.ShelvesID = s.ShelvesID and 
			c.product_id = i.product_id and
			c.Quantity >= i.quantity
        WHERE w.WarehouseId = dn.WarehouseId
    )
    BEGIN
        RAISERROR('Kiểm tra lại DeliveryNoteDetail!', 16, 1);
        ROLLBACK TRANSACTION;
    END

end

--triger Update lại Số lượng sản phẩm đã giao của phiếu đặt khi insert update phiếu nhập kho:
create trigger trg_up_QuanttityDelivered
on WarehouseReceiptDetail
after UPDATE, INSERT
as 
begin
	
	SET NOCOUNT ON;

	DECLARE	@QuantityDelivered int;

	select @QuantityDelivered = SUM(w.quantity) 
	from WarehouseReceiptDetail w
	join inserted i on w.PurchaseOrderID = i.PurchaseOrderID and w.priceHistoryId = i.priceHistoryId;

	UPDATE pod
    SET pod.QuantityDelivered = @QuantityDelivered
    FROM PurchaseOrderDetail pod
    INNER JOIN inserted i ON pod.PurchaseOrderID = i.PurchaseOrderID;

end


--################# cập nhật loại khách hàng thân thuộc nếu tổng giá trị đơn hàng >= 10000000
--DROP TRIGGER trg_UpdateCustomerType
CREATE TRIGGER trg_UpdateCustomerType
ON [Order]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CustomerId INT;

    -- Get the CustomerId from the inserted or updated orders
    SELECT @CustomerId = CreateBy
    FROM inserted;

    IF (
        SELECT SUM(Total_Payment)
        FROM [Order]
        WHERE CreateBy = @CustomerId
    ) >= 10000000
    BEGIN
        UPDATE Customer
        SET type_customer_id = 2
        WHERE customerId = @CustomerId;
    END
END;
