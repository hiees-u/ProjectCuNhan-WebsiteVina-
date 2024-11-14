insert into CustomerType (type_customer_name)
values (N'Mới'), (N'Thân Thiết'), (N'Doanh Nghiệp');

select * from CustomerType

insert into Roles (role_name)
values (N'Customer'), (N'Moderator'), (N'Warehouse Employee');

select * from Roles

update Roles set role_name = N'Customer' where role_id = 1

select * from Users

INSERT INTO Category (category_name, ModifiedBy)
VALUES 
    ('Cà phê hòa tan', 'hieu1'),
    ('Cà phê phin', 'hieu1'),
    ('Cà phê espresso', 'hieu1'),
    ('Cà phê cold brew', 'hieu1');


select * from Category

insert into Address (CommuneID, HouseNumber)
values (10, N'30/12 Đường DX 01')

insert into District (DistrictName, ProvinceID)
values (N'Tân Uyên', 48)

insert into Commune(CommuneName, DistrictID) 
values (N'Tân Vĩnh Hiệp', 8)

select * from Address

select * from District

select * from Province

select * from Commune

insert into Supplier (SupplierName, AddressID)
values (N'Buôn Mê Coffee', 3)

insert into Supplier (SupplierName, AddressID)
values (N'Drai Farm', 6)

insert into Supplier (SupplierName, AddressID)
values (N'Cà phê Brown', 9)

select * from Supplier

alter table Address
alter column HouseNumber nvarchar(30) NOT NULL

insert into SubCategory(SubCategoryName)
values (N'Cà phê decaf'), (N'Cascara')

select * from SubCategory

alter table SubCategory
add constraint DF_SubCategory_CreateTime Default GETDATE() FOR CreateTime


--
select * from Warehouse

insert into Warehouse (WarehouseName, AddressID, ModifiedBy) 
values (N'Kho A', 10, N'HiuDEV')
--
select * from Shelve

insert into Shelve(ShelvesName, WarehouseID, ModifiedBy) values (N'Kệ A1', 2, N'HiuDEV')
--
select * from Cells

insert into Cells (CellName, ShelvesID, ModifiedBy)
values ('Ô 1 - 1', 3, N'HiuDEV')
--
insert into Roles (role_name)
values (N'Development')
--
insert into Users (AccountName, role_id) values (N'HiuDEV',9)
--
insert into UserInfo (AccountName, full_name, email, address_id, phone, gender, Employ_ID)
values(N'HiuDEV', N'Nguyễn Minh Hiếu', 'nguyenminhhieuuuu.31018@gmail.com', 10, '0393370172', 1, 4);
--
insert into Address (CommuneID, HouseNumber,Note)
values (1, N'F4MR+J74, ĐT865', N'Đối diện Đại lý Bia Văn Hiệp')
--
insert into Employee (EmployeeTypeID)
values (2)
--
insert into EmployeeType (EmployeeTypeName)
values(N'DEV DataBase')
--
insert into Employee(EmployeeTypeID)
values (2)
--
insert into PurchaseOrder(EmployeeID, SupplierID, DeliveryDate)
values(4, 5, '2024/10/25')

select * from PurchaseOrder

select * from PurchaseOrder p, Employee e, Supplier s
where p.EmployeeID = e.EmployeeID and s.SupplierID = p.SupplierID 

select e.EmployeeID, et.EmployeeTypeName, uf.AccountName From Employee e, UserInfo uf, EmployeeType et
where uf.Employ_ID = e.EmployeeID and et.EmployeeTypeID = e.EmployeeTypeID

--
select * from Product

select * from PriceHistory

insert into PriceHistory(product_id, price, ModifiedBy)
values (3, 18000, N'HiuDEV')

update PriceHistory
set isActive = 0
where priceHistoryId = 1

select p.*,  ph.price from Product p, PriceHistory ph
where p.product_id = ph.product_id and ph.isActive = 0 and p.DeleteTime Is NULL and p.totalQuantity != 0

--
alter table PurchaseOrder
add SupplierID int 

alter table PurchaseOrder
ADD CONSTRAINT FK_PurchaseOrderSupplierID FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)
--
select * from Supplier

--
--alter 

select * From Users u, Roles r where u.role_id = r.role_id

--####
--insert EmployeeType:
insert into EmployeeType(EmployeeTypeName, ModifiedBy, ModifiedTime)
values ('Warehouse Employee', 'HiuDEV', GETDATE())

insert into EmployeeType(EmployeeTypeName, ModifiedBy, ModifiedTime)
values ('Order Approver', 'HiuDEV', GETDATE())

insert into EmployeeType(EmployeeTypeName, ModifiedBy, ModifiedTime)
values ('Moderator', 'HiuDEV', GETDATE())

select * From EmployeeType

select * from Employee

select *From UserInfo

alter table UserInfo
alter column address_id int NULL

alter table UserInfo
alter column gender int NULL

select * From CustomerType

select * from Department

ALTER TABLE EmployeeType
ADD CONSTRAINT DF_EmployeeType_ModifiedTime DEFAULT GETDATE() FOR ModifiedTime;

insert into Department (ModifiedBy, DepartmentName)
values ('HiuDEV', N'Phòng IT')

select * from EmployeeType

select * from Employee e, EmployeeType et where e.EmployeeTypeID = et.EmployeeTypeID

select * from Warehouse

insert into Department(DepartmentName, ModifiedBy)
values(N'Kho A', 'HiuDEV')

insert into Department(DepartmentName, ModifiedBy)
values(N'Kinh Doanh A1', 'HiuDEV')

select * from Roles

select * From Department

--
select * from PriceHistory

select * from Product

insert into PriceHistory(product_id, price, ModifiedBy)
values	(5, 10000, 'HiuDev'),
		(4, 150000, 'HiuDev')

update PriceHistory
set isActive = 0
where priceHistoryId = 2

update PriceHistory
set isActive = 0
where priceHistoryId = 3

select * from PurchaseOrderDetail

insert into PurchaseOrderDetail (PurchaseOrderID, priceHistoryId, quantity, Address)
values  (1, 2, 10, 10),
		(1, 3, 15, 10)

select * from PriceHistory ph, Product p where p.product_id = ph.product_id

select * From WarehouseReceipt

insert into WarehouseReceipt (EmployeeID, WarehouseID)
values ( 13, 2)

select * From WarehouseReceiptDetail

insert into WarehouseReceiptDetail(WarehouseReceiptID, CellID, product_id, PurchaseOrderID, priceHistoryId, quantity, UpdateBy)
values	(4,11, 5, 1, 2, 1, 9),
		--(4,12, 4, 1, 3, 1, 9)

Select * from Cells

insert into Cells (CellName, ShelvesID, ModifiedBy)
values(N'Ô 1- 2', 3, 'HiuDev')

insert into Cells (CellName, ShelvesID, ModifiedBy)
values(N'Ô 1- 3', 3, 'HiuDev')

select * from Users u, Roles r, UserInfo uf, Employee e
where u.role_id = r.role_id and uf.AccountName =u.AccountName and e.EmployeeID = uf.Employ_ID

--
GO
CREATE TRIGGER trg_UniqueShelvesNamePerWarehouse
ON Shelve
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra nếu có tên kệ trùng lặp trong cùng một WarehouseID
    IF EXISTS (
        SELECT 
            s.ShelvesName, s.WarehouseID
        FROM 
            Shelve s
        JOIN 
            inserted i ON s.ShelvesName = i.ShelvesName 
                       AND s.WarehouseID = i.WarehouseID
                       AND s.ShelvesID <> i.ShelvesID
    )
    BEGIN
        -- Nếu phát hiện trùng lặp, rollback transaction và thông báo lỗi
        ROLLBACK TRANSACTION;
        RAISERROR (N'Tên kệ đã tồn tại trong kho này, vui lòng chọn tên khác.', 16, 1);
    END
END;
GO


--alter
alter table [Order]
add paymentStatus bit default 0