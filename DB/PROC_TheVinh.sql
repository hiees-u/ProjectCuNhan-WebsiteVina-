--===========================================================================================
--================================== Quản Lý Kho ============================================
--===========================================================================================
--CREATE LOGIN TheVinhWarehouseEmployee WITH PASSWORD = '123';

USE CAFFEE_VINA_DBv1;

--CREATE USER TheVinhWarehouseEmployee FOR LOGIN TheVinhWarehouseEmployee;

--==============================
USE CAFFEE_VINA_DBv1;

--ALTER ROLE WarehouseEmployee ADD MEMBER VinhWarehouseEmployee;
--==============================================================
SELECT m.name AS MemberName, r.name AS RoleName
FROM sys.database_role_members rm
    JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
    JOIN sys.database_principals m ON rm.member_principal_id = m.principal_id
WHERE
    r.name = 'WarehouseEmployee';
--======================================================================
--create role Warehouse Employee
--Create ROLE WarehouseEmployee;

Grant
Select On dbo.Users to WarehouseEmployee
Grant
Select,
Update,
Insert
    On dbo.UserInfo to WarehouseEmployee
Grant
Select On dbo.Province to WarehouseEmployee
Grant
Select,
Insert
    On dbo.District to WarehouseEmployee
Grant
Select,
Insert
    On dbo.Commune to WarehouseEmployee
Grant
Select,
Insert
    On dbo.Address to WarehouseEmployee
Grant
Select,
Update,
Insert
,
    Delete On dbo.Warehouse to WarehouseEmployee

Grant
Select,
Update,
Insert
    On dbo.DeliveryNote to WarehouseEmployee
Grant
Select,
Update,
Insert
    On dbo.DeliveryNoteDetail to WarehouseEmployee

Grant
Select,
Update,
Insert
,
    Delete On dbo.WarehouseReceipt to WarehouseEmployee
Grant
Select,
Update,
Insert
,
    Delete On dbo.WarehouseReceiptDetail to WarehouseEmployee

Grant
Select,
Update,
Insert
    On dbo.PurchaseOrder to WarehouseEmployee
Grant
Select,
Update,
Insert
    On dbo.PurchaseOrderDetail to WarehouseEmployee

Grant
Select,
Update,
Insert
,
    Delete On dbo.Shelve to WarehouseEmployee
Grant
Select,
Update,
Insert
,
    Delete On dbo.Cells to WarehouseEmployee

Grant
Select On dbo.Product to WarehouseEmployee --bổ sung 18/11/2024
Grant
Select On dbo.PriceHistory to WarehouseEmployee

Grant
Select On dbo.Employee to WarehouseEmployee
    --Thu hồi quyền từ một user:
    --REVOKE SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo FROM HiuWarehouseEmployee;
    --Thu hồi quyền từ một vai trò:
    --REVOKE SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo FROM WarehouseEmployee;
--############################################# GET ALL WAREHOUSE #####################################################################################
go
--drop proc GetAllWarehouses
CREATE PROCEDURE GetAllWarehouses AS BEGIN
SELECT
    w.WarehouseID,
    w.WarehouseName,
    w.AddressID,
    COALESCE(a.Note, '') + N', Xã ' + COALESCE(c.CommuneName, '') + N', Huyện ' + COALESCE(d.DistrictName, '') + N', Tỉnh ' + COALESCE(p.ProvinceName, '') AS FullAddress,
    w.ModifiedBy,
    w.CreateTime,
    w.ModifiedTime
FROM
    Warehouse w
    LEFT JOIN Address a ON w.AddressID = a.AddressID
    LEFT JOIN Commune c ON a.CommuneID = c.CommuneID
    LEFT JOIN District d ON c.DistrictID = d.DistrictID
    LEFT JOIN Province p ON d.ProvinceID = p.ProvinceID
WHERE
    w.DeleteTime IS NULL
ORDER BY w.CreateTime DESC;

END;

GO GO
--Lấy thông tin kho
EXEC GetAllWarehouses
--PHân quyền
GRANT EXEC ON OBJECT::dbo.GetAllWarehouses TO WarehouseEmployee;

--############################################### GET WAREHOUSE BY ID #####################################################################################
go

CREATE PROCEDURE GetWarehouseByID
    @WarehouseID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Kiểm tra xem Warehouse có tồn tại hay không
    IF NOT EXISTS (SELECT 1 FROM Warehouse WHERE WarehouseID = @WarehouseID)
    BEGIN
        SET @Message = N'Không tìm thấy Warehouse với ID đã cho';
        RETURN;
    END

    -- Truy vấn thông tin Warehouse cùng với địa chỉ đầy đủ
    SELECT 
        w.WarehouseID,
        w.WarehouseName,
        w.AddressID,
        COALESCE(a.Note, '') + N', Xã ' + COALESCE(c.CommuneName, '') + N', Huyện ' + COALESCE(d.DistrictName, '') + N', Tỉnh ' + COALESCE(p.ProvinceName, '') AS FullAddress,
        w.ModifiedBy,
        w.CreateTime,
        w.ModifiedTime
    FROM 
        Warehouse w
    LEFT JOIN Address a ON w.AddressID = a.AddressID
    LEFT JOIN Commune c ON a.CommuneID = c.CommuneID
    LEFT JOIN District d ON c.DistrictID = d.DistrictID
    LEFT JOIN Province p ON d.ProvinceID = p.ProvinceID
    WHERE 
        w.WarehouseID = @WarehouseID AND w.DeleteTime IS NULL;

    -- Đặt thông báo thành công
    SET @Message = N'Đã lấy thông tin kho thành công!';
END;
GO

--GO

--Lấy thông tin kho với id
DECLARE @ResultMessage NVARCHAR(100);
EXEC GetWarehouseByID
    @WarehouseID = 2, 
    @Message = @ResultMessage OUTPUT;
PRINT @ResultMessage;

--PHân quyền
GRANT EXEC ON OBJECT::dbo.GetWarehouseByID TO  WarehouseEmployee;

--############################################### GET WAREHOUSE BY NAME #####################################################################################
go
CREATE PROCEDURE GetWarehouseByName
    @WarehouseName NVARCHAR(100),
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Kiểm tra xem có bất kỳ Warehouse nào phù hợp không
    IF NOT EXISTS (SELECT 1 FROM Warehouse WHERE WarehouseName LIKE '%' + @WarehouseName + '%')
    BEGIN
        SET @Message = N'Không tìm thấy kho với tên đã cho.';
        RETURN;
    END

    -- Truy vấn thông tin các kho phù hợp với tên
    SELECT 
        w.WarehouseID,
        w.WarehouseName,
        w.AddressID,
        COALESCE(a.Note, '') + N', Xã ' + COALESCE(c.CommuneName, '') + N', Huyện ' + COALESCE(d.DistrictName, '') + N', Tỉnh ' + COALESCE(p.ProvinceName, '') AS FullAddress,
        w.ModifiedBy,
        w.CreateTime,
        w.ModifiedTime
    FROM 
        Warehouse w
    LEFT JOIN Address a ON w.AddressID = a.AddressID
    LEFT JOIN Commune c ON a.CommuneID = c.CommuneID
    LEFT JOIN District d ON c.DistrictID = d.DistrictID
    LEFT JOIN Province p ON d.ProvinceID = p.ProvinceID
    WHERE 
        w.WarehouseName LIKE '%' + @WarehouseName + '%' AND w.DeleteTime IS NULL;

    -- Đặt thông báo thành công
    SET @Message = N'Đã lấy danh sách kho thành công!';
END;
GO

--Phân quyền
GRANT EXEC ON OBJECT::dbo.GetWarehouseByName TO  WarehouseEmployee;

--Thực thi
DECLARE @Message NVARCHAR(100);
-- Gọi stored procedure với từ khóa tìm kiếm
EXEC GetWarehouseByName 
    @WarehouseName = N'Kho',
    @Message = @Message OUTPUT;

-- Xem thông báo
PRINT @Message;

--########################################## INSERT WAREHOUSE #####################################################################################
go
CREATE PROCEDURE AddWarehouse
    @WarehouseName NVARCHAR(30),
	@AddressID INT,
    @Message NVARCHAR(100) OUTPUT -- Thêm tham số OUTPUT
AS
BEGIN  
    IF EXISTS (SELECT 1 FROM Warehouse WHERE WarehouseName = @WarehouseName)
    BEGIN
        SET @Message = N'Tên kho đã tồn tại, vui lòng chọn tên khác.'; -- Gán thông báo vào biến OUTPUT
        RETURN;
    END

    BEGIN TRANSACTION;

    BEGIN TRY
        INSERT INTO Warehouse (WarehouseName, AddressID, ModifiedBy, CreateTime)
        VALUES (@WarehouseName, @AddressID, SUSER_NAME(), GETDATE());

        -- Commit giao dịch nếu không có lỗi
        COMMIT TRANSACTION;

        SET @Message = N'Kho mới đã được thêm thành công!';
    END TRY
    BEGIN CATCH
        -- Rollback giao dịch nếu có lỗi xảy ra
        ROLLBACK TRANSACTION;
        SET @Message = N'Đã xảy ra lỗi trong quá trình thêm kho!';
    END CATCH
END;
GO
--============================================================

--Phân quyền
GRANT EXEC ON OBJECT::dbo.AddWarehouse TO  WarehouseEmployee;

--Thêm Kho
DECLARE @Message NVARCHAR(100);
EXEC AddWarehouse
    @WarehouseName = N'Thêm Kho Test 1',
	@AddressID = 1,
    @Message = @Message OUTPUT;
-- Hiển thị giá trị của thông báo
SELECT @Message AS Message;
--################################################### UPDATE WAREHOUSE WITH FULL ADDRESS #####################################################################################
go
CREATE PROCEDURE UpdateWarehouse
    @WarehouseID INT,
    @WarehouseName NVARCHAR(30),
	@AddressID INT,
    @OutputMessage NVARCHAR(100) OUTPUT
AS
BEGIN  
    BEGIN TRANSACTION;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Warehouse WHERE WarehouseID = @WarehouseID)
        BEGIN
            SET @OutputMessage = N'WarehouseID không tồn tại';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Warehouse WHERE WarehouseName = @WarehouseName AND WarehouseID != @WarehouseID)
        BEGIN
            SET @OutputMessage = N'Tên kho đã tồn tại, vui lòng chọn tên khác';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        UPDATE Warehouse
        SET 
            WarehouseName = @WarehouseName,
            AddressID = @AddressID,
            ModifiedBy = SUSER_NAME(),
            ModifiedTime = GETDATE()
        WHERE WarehouseID = @WarehouseID;

        -- Commit giao dịch nếu không có lỗi
        COMMIT TRANSACTION;
        SET @OutputMessage = N'Cập nhật kho thành công!';
    END TRY
    BEGIN CATCH
        -- Rollback giao dịch nếu có lỗi xảy ra
        ROLLBACK TRANSACTION;
        SET @OutputMessage = N'Đã xảy ra lỗi trong quá trình cập nhật kho: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

--PHân quyền
GRANT EXEC ON OBJECT::dbo.UpdateWarehouse TO  WarehouseEmployee;

--==== Cập nhật thông tin kho với WarehouseID = 3
DECLARE @OutputMessage NVARCHAR(100);
EXEC UpdateWarehouse
    @WarehouseID = 7,
    @WarehouseName = N'Kho D',
	@AddressID = 2,
    @OutputMessage = @OutputMessage OUTPUT;
-- display alert
SELECT @OutputMessage AS N'Result';

--################################################ DELETE WAREHOUSE #####################################################################################
go

--====PROC UPDATE 02-12-2024
CREATE PROCEDURE DeleteWarehouse
    @WarehouseID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    DECLARE @count_id INT;
    DECLARE @product_count INT;

    -- Check if the warehouse exists
    SELECT @count_id = COUNT(*) 
    FROM Warehouse 
    WHERE WarehouseID = @WarehouseID;
    
    IF @count_id = 0 
    BEGIN
        SET @Message = N'WarehouseID không tồn tại';
        RETURN;
    END

    -- Check if there are any products in the warehouse
    SELECT @product_count = COUNT(*)
    FROM Product p
    JOIN Cells c ON p.product_id = c.product_id
    JOIN Shelve s ON c.ShelvesID = s.ShelvesID
    WHERE s.WarehouseID = @WarehouseID;

    IF @product_count > 0
    BEGIN
        SET @Message = N'Kho còn sản phẩm, không thể xóa';
        RETURN;
    END

    -- Update DeleteTime instead of physical deletion
    UPDATE Warehouse 
    SET DeleteTime = GETDATE()
    WHERE WarehouseID = @WarehouseID;

    -- Check if the warehouse was successfully deleted
    IF @@ROWCOUNT > 0
    BEGIN
        SET @Message = N'Xóa thành công';
    END
    ELSE
    BEGIN
        SET @Message = N'Có lỗi xảy ra khi xóa kho';
    END
END;
GO

-- Phân quyền
GRANT EXEC ON OBJECT::dbo.DeleteWarehouse TO WarehouseEmployee;

-- Xóa kho với WarehouseID = 4
DECLARE @ResultMessage NVARCHAR(100);
EXEC DeleteWarehouse 
    @WarehouseID = 26,
    @Message = @ResultMessage OUTPUT;

-- Xem thông báo kết quả
SELECT @ResultMessage AS Result;


--===========================================================================================
--================================== Quản Lý Kệ =============================================
--===========================================================================================

--################################################ GET SHELEVE OF WAREHOUSE#####################################################################################
go
CREATE PROCEDURE GetShelveByWarehouseID
    @WarehouseID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Shelve WHERE WarehouseID = @WarehouseID)
    BEGIN
        SET @Message = N'Không tìm thấy Kệ với Mã Kho đã cho!';
        RETURN;
    END

    -- Lấy thông tin của kệ
    SELECT 
        s.ShelvesID,
        s.ShelvesName,
        s.WarehouseID,
        s.ModifiedBy,
        s.CreateTime,
        s.ModifiedTime,
        s.DeleteTime
    FROM Shelve s
    WHERE 
        s.WarehouseID = @WarehouseID AND s.DeleteTime IS NULL;

    SET @Message = N'Đã lấy thông tin kệ theo kho thành công!';
END;
GO

--PHân quyền
GRANT EXEC ON OBJECT::dbo.GetShelveByWarehouseID TO  WarehouseEmployee;

--Lấy thông tin kệ với id kho
DECLARE @ResultMessage NVARCHAR(100);
EXEC GetShelveByWarehouseID
    @WarehouseID = 2, 
    @Message = @ResultMessage OUTPUT;
-- Xem thông báo
PRINT @ResultMessage;

--########################################## INSERT SHELVE #####################################################################################

--================================
--========Bổ sung Shelve==========
--================================

-- Xóa UNI_ShelveName trong Index Shelve
-- Bổ sung trigger kiểm tra tên trùng theo kho
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

--============================================
GO
CREATE PROCEDURE AddShelves
    @ShelvesName NVARCHAR(30),
    @WarehouseID INT,
    @Message NVARCHAR(100) OUTPUT -- Tham số OUTPUT để trả về thông báo
AS
BEGIN  
    -- Kiểm tra nếu tên kệ đã tồn tại trong cùng WarehouseID
    IF EXISTS (SELECT 1 FROM Shelve WHERE ShelvesName = @ShelvesName AND WarehouseID = @WarehouseID)
    BEGIN
        SET @Message = N'Tên kệ đã tồn tại trong kho, vui lòng chọn tên khác.'; -- Gán thông báo vào biến OUTPUT
        RETURN;
    END

    -- Bắt đầu giao dịch
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Thêm kệ vào bảng Shelve
        INSERT INTO Shelve (ShelvesName, WarehouseID, ModifiedBy, CreateTime)
        VALUES (@ShelvesName, @WarehouseID, SUSER_NAME(), GETDATE());

        -- Commit giao dịch nếu không có lỗi
        COMMIT TRANSACTION;

        SET @Message = N'Kệ mới đã được thêm thành công!'; -- Gán thông báo vào biến OUTPUT
    END TRY
    BEGIN CATCH
        -- Rollback giao dịch nếu có lỗi xảy ra
        ROLLBACK TRANSACTION;
        SET @Message = N'Đã xảy ra lỗi trong quá trình thêm kệ!'; -- Gán thông báo lỗi vào biến OUTPUT
    END CATCH
END;
GO

--Phân quyền
GRANT EXEC ON OBJECT::dbo.AddShelves TO  WarehouseEmployee;

--Thêm kệ
DECLARE @ResultMessage NVARCHAR(100);
EXEC AddShelves 
    @ShelvesName = N'Test kệ',
    @WarehouseID = 8,
    @Message = @ResultMessage OUTPUT;
-- In ra thông báo
PRINT @ResultMessage;


--########################################## UPDATE SHELVE #####################################################################################

GO
CREATE PROCEDURE UpdateShelve
    @ShelvesID INT,
    @ShelvesName NVARCHAR(30),
    @WarehouseID INT,
    @OutputMessage NVARCHAR(100) OUTPUT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Shelve WHERE ShelvesID = @ShelvesID)
        BEGIN
            SET @OutputMessage = N'ShelvesID không tồn tại';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Shelve 
                   WHERE ShelvesName = @ShelvesName 
                     AND WarehouseID = @WarehouseID 
                     AND ShelvesID != @ShelvesID)
        BEGIN
            SET @OutputMessage = N'Tên kệ đã tồn tại trong kho, vui lòng chọn tên khác';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        UPDATE Shelve
        SET 
            ShelvesName = @ShelvesName,
            WarehouseID = @WarehouseID,
            ModifiedBy = SUSER_NAME(),
            ModifiedTime = GETDATE()
        WHERE ShelvesID = @ShelvesID;

        COMMIT TRANSACTION;
        SET @OutputMessage = N'Cập nhật kệ thành công!';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @OutputMessage = N'Đã xảy ra lỗi trong quá trình cập nhật kệ: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

--Phân quyền
GRANT EXEC ON OBJECT::dbo.UpdateShelve TO  WarehouseEmployee;

--Cập nhật kệ
DECLARE @ResultMessage NVARCHAR(100);

EXEC UpdateShelve 
    @ShelvesID = 1,
    @ShelvesName = N'Kệ A2',
    @WarehouseID = 1,
    @OutputMessage = @ResultMessage OUTPUT;

-- In ra thông báo
PRINT @ResultMessage;

--########################################## DELETE SHELVE #####################################################################################

GO
CREATE PROCEDURE DeleteShelve
    @ShelvesID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    DECLARE @count_id INT;

    -- Kiểm tra ShelvesID có tồn tại không
    SELECT @count_id = COUNT(*) 
    FROM Shelve 
    WHERE ShelvesID = @ShelvesID;

    IF @count_id = 0 
    BEGIN
        SET @Message = N'ShelvesID không tồn tại';
        RETURN;
    END

    -- Xóa mềm kệ (cập nhật DeleteTime thay vì xóa vật lý)
    UPDATE Shelve 
    SET DeleteTime = GETDATE()
    WHERE ShelvesID = @ShelvesID;

    -- Kiểm tra xem kệ đã được xóa thành công chưa
    IF @@ROWCOUNT > 0
    BEGIN
        SET @Message = N'Xóa kệ thành công';
    END
    ELSE
    BEGIN
        SET @Message = N'Có lỗi xảy ra khi xóa kệ';
    END
END;
GO

--Phân quyền
GRANT EXEC ON OBJECT::dbo.DeleteShelve TO  WarehouseEmployee;

DECLARE @ResultMessage NVARCHAR(100);

EXEC DeleteShelve 
    @ShelvesID = 1,
    @Message = @ResultMessage OUTPUT;
-- In ra thông báo
PRINT @ResultMessage;

--===========================================================================================
--================================== Quản Lý Ô ==============================================
--===========================================================================================

--########################################## Get Product By ShelveID #####################################################################################

GO
CREATE PROCEDURE GetProductByShelveID
    @ShelvesID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Kiểm tra xem kệ có tồn tại hay không
    IF NOT EXISTS (SELECT 1 FROM Shelve WHERE ShelvesID = @ShelvesID)
    BEGIN
        SET @Message = N'Không tìm thấy Kệ với Mã Kệ đã cho!';
        RETURN;
    END

    -- Lấy thông tin sản phẩm của các ô theo kệ
    SELECT 
        c.CellID,
        c.CellName,
        c.Quantity,
        p.product_id,
        p.product_name,
        p.image,
        p.totalQuantity,
        p.ExpriryDate,
        c.ModifiedBy,
        c.CreateTime,
        c.ModifiedTime,
        c.DeleteTime
    FROM Cells c
    INNER JOIN Product p ON c.product_id = p.product_id
    WHERE 
        c.ShelvesID = @ShelvesID 
        AND c.DeleteTime IS NULL 
        AND p.DeleteTime IS NULL;

    SET @Message = N'Đã lấy thông tin sản phẩm theo kệ thành công!';
END;
GO

DECLARE @Message NVARCHAR(100);
EXEC GetProductByShelveID @ShelvesID = 4, @Message = @Message OUTPUT;
PRINT @Message;

GRANT EXEC ON OBJECT::dbo.GetProductByShelveID TO  WarehouseEmployee;

go
--===================================================Lấy sản phẩm theo kho======================================
CREATE PROCEDURE GetProductsByWarehouseID
    @WarehouseID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Kiểm tra xem Warehouse có tồn tại hay không
    IF NOT EXISTS (SELECT 1 FROM Warehouse WHERE WarehouseID = @WarehouseID)
    BEGIN
        SET @Message = N'Không tìm thấy Kho với Mã Kho đã cho!';
        RETURN;
    END

    -- Truy vấn thông tin kệ, ô và sản phẩm
    SELECT 
        w.WarehouseName,
        s.ShelvesName,
        c.CellName,
        p.product_name,
        p.image,
        c.Quantity,
        p.totalQuantity,
        p.ExpriryDate,
        c.ModifiedBy AS CellModifiedBy,
        c.CreateTime AS CellCreateTime,
        c.ModifiedTime AS CellModifiedTime,
        c.DeleteTime AS CellDeleteTime
    FROM Warehouse w
    INNER JOIN Shelve s ON w.WarehouseID = s.WarehouseID
    INNER JOIN Cells c ON s.ShelvesID = c.ShelvesID
    INNER JOIN Product p ON c.product_id = p.product_id
    WHERE 
        w.WarehouseID = @WarehouseID 
        AND s.DeleteTime IS NULL 
        AND c.DeleteTime IS NULL 
        AND p.DeleteTime IS NULL
    ORDER BY s.ShelvesName, c.CellName;

    SET @Message = N'Đã lấy thông tin sản phẩm theo Kho thành công!';
END;
GO


GRANT EXEC ON OBJECT::dbo.GetProductsByWarehouseID TO  WarehouseEmployee;

DECLARE @Message NVARCHAR(100);
EXEC GetProductsByWarehouseID @WarehouseID = 2, @Message = @Message OUTPUT;
PRINT @Message;

--=======================================================================================

--########################################## INSERT CELL #####################################################################################
GO
CREATE PROCEDURE AddCell
    @CellName NVARCHAR(30),
    @ShelvesID INT,
    @Quantity INT,
    @product_id INT,
    @Message NVARCHAR(100) OUTPUT -- Tham số OUTPUT để trả về thông báo
AS
BEGIN
    -- Kiểm tra nếu tên ô đã tồn tại trong cùng ShelvesID
    IF EXISTS (SELECT 1 FROM Cells WHERE CellName = @CellName AND ShelvesID = @ShelvesID)
    BEGIN
        SET @Message = N'Tên ô đã tồn tại trong kệ, vui lòng chọn tên khác.'; -- Gán thông báo vào biến OUTPUT
        RETURN;
    END

    -- Bắt đầu giao dịch
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Thêm ô vào bảng Cells
        INSERT INTO Cells (CellName, ShelvesID, Quantity, product_id, ModifiedBy, CreateTime)
        VALUES (@CellName, @ShelvesID, @Quantity, @product_id, SUSER_NAME(), GETDATE());

        -- Commit giao dịch nếu không có lỗi
        COMMIT TRANSACTION;

        SET @Message = N'Ô mới đã được thêm thành công!'; -- Gán thông báo vào biến OUTPUT
    END TRY
    BEGIN CATCH
        -- Rollback giao dịch nếu có lỗi xảy ra
        ROLLBACK TRANSACTION;
        SET @Message = N'Đã xảy ra lỗi trong quá trình thêm ô!'; -- Gán thông báo lỗi vào biến OUTPUT
    END CATCH
END;
GO

GRANT EXEC ON OBJECT::dbo.AddCell TO  WarehouseEmployee;

DECLARE @Message NVARCHAR(100);
EXEC AddCell 
    @CellName = N'test cell 1', 
    @ShelvesID = 5, 
    @Quantity = 20, 
    @product_id = 4,
    @Message = @Message OUTPUT;
PRINT @Message;

--============================Xóa ràng buộc CellName====================
alter table Cells
drop constraint uni_CellName
--======================================================================

--########################################## INSERT CELL #####################################################################################

GO
CREATE PROCEDURE UpdateCell
    @CellID INT,
    @CellName NVARCHAR(30),
    @ShelvesID INT,
    @Quantity INT,
    @product_id INT,
    @OutputMessage NVARCHAR(100) OUTPUT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Cells WHERE CellID = @CellID)
        BEGIN
            SET @OutputMessage = N'CellID không tồn tại';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM Cells 
                   WHERE CellName = @CellName 
                     AND ShelvesID = @ShelvesID 
                     AND CellID != @CellID)
        BEGIN
            SET @OutputMessage = N'Tên ô đã tồn tại trong kệ, vui lòng chọn tên khác';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        UPDATE Cells
        SET 
            CellName = @CellName,
            ShelvesID = @ShelvesID,
            Quantity = @Quantity,
            product_id = @product_id,
            ModifiedBy = SUSER_NAME(),
            ModifiedTime = GETDATE()
        WHERE CellID = @CellID;
        COMMIT TRANSACTION;
        SET @OutputMessage = N'Cập nhật ô thành công!';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @OutputMessage = N'Đã xảy ra lỗi trong quá trình cập nhật ô: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

GRANT EXEC ON OBJECT::dbo.UpdateCell TO  WarehouseEmployee;

DECLARE @OutputMessage NVARCHAR(100);
EXEC UpdateCell 
    @CellID = 14,
    @CellName = N'Sửa ô',
    @ShelvesID = 3,
    @Quantity = 60,
    @product_id = 4,
    @OutputMessage = @OutputMessage OUTPUT;
PRINT @OutputMessage;

--########################################## DELETE CELL #####################################################################################
GO
CREATE PROCEDURE DeleteCell
    @CellID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    DECLARE @count_id INT;
    SELECT @count_id = COUNT(*) 
    FROM Cells 
    WHERE CellID = @CellID;

    IF @count_id = 0 
    BEGIN
        SET @Message = N'CellID không tồn tại';
        RETURN;
    END
    UPDATE Cells 
    SET DeleteTime = GETDATE()
    WHERE CellID = @CellID;

    IF @@ROWCOUNT > 0
    BEGIN
        SET @Message = N'Xóa ô thành công';
    END
    ELSE
    BEGIN
        SET @Message = N'Có lỗi xảy ra khi xóa ô';
    END
END;
GO

GRANT EXEC ON OBJECT::dbo.DeleteCell TO  WarehouseEmployee;

DECLARE @Message NVARCHAR(100);
EXEC DeleteCell 
    @CellID = 10,
    @Message = @Message OUTPUT;
PRINT @Message;
go
--===============
--====trigger====
--===============
--Trigger cho thao tác nhập kho (khi có thay đổi trong WarehouseReceiptDetail). thêm tổng số lượng sản phẩm khi nhập kho
CREATE TRIGGER trg_UpdateTotalQuantity_OnInsertWarehouseReceipt
ON WarehouseReceiptDetail
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Khi thêm hoặc cập nhật phiếu nhập kho
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Cập nhật tổng số lượng sản phẩm dựa trên bảng WarehouseReceiptDetail
        UPDATE Product
        SET totalQuantity = totalQuantity + ISNULL(i.quantity, 0) - ISNULL(d.quantity, 0)
        FROM Product p
        LEFT JOIN inserted i ON p.product_id = i.product_id
        LEFT JOIN deleted d ON p.product_id = d.product_id;
    END
END;
GO
--Trigger cho thao tác xuất kho (khi có thay đổi trong DeliveryNoteDetail). Tự động cập nhật số lướng
CREATE TRIGGER trg_UpdateTotalQuantity_OnInsertDeliveryNote
ON DeliveryNoteDetail
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Khi thêm hoặc cập nhật phiếu xuất kho
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Cập nhật tổng số lượng sản phẩm dựa trên bảng DeliveryNoteDetail
        UPDATE Product
        SET totalQuantity = totalQuantity + ISNULL(i.quantity, 0) - ISNULL(d.quantity, 0)
        FROM Product p
        LEFT JOIN inserted i ON p.product_id = i.product_id
        LEFT JOIN deleted d ON p.product_id = d.product_id;
    END
END;
GO
--===========================================================================================
--================================== Quản Lý Nhập Kho =======================================
--===========================================================================================

--===============Thêm Phiếu Nhập========================
-- Bảng tạm để truyền danh sách chi tiết nhập kho
go
CREATE TYPE dbo.ReceiptDetailType AS TABLE (
    ProductID INT,
    CellID INT,
    Quantity INT,
    PurchaseOrderID INT
);
GO

CREATE PROCEDURE sp_InsertWarehouseReceipt 
    @WarehouseID INT,
    @ReceiptDetails dbo.ReceiptDetailType READONLY
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Step 1: Add WarehouseReceipt
        DECLARE @WarehouseReceiptID INT;
        DECLARE @EmployeeID INT;

        SELECT @EmployeeID = UF.Employ_ID 
        FROM UserInfo UF 
        WHERE UF.AccountName = SUSER_NAME();

        INSERT INTO WarehouseReceipt (EmployeeID, CreateAt, WarehouseID)
        VALUES (@EmployeeID, GETDATE(), @WarehouseID);

        -- Get ID WarehouseReceipt
        SET @WarehouseReceiptID = SCOPE_IDENTITY();

        -- Step 2: Add WarehouseReceiptDetail
        INSERT INTO WarehouseReceiptDetail (WarehouseReceiptID, CellID, quantity, PurchaseOrderID, UpdateTime, UpdateBy, priceHistoryId)
        SELECT @WarehouseReceiptID, RD.CellID, RD.Quantity, RD.PurchaseOrderID, GETDATE(), @EmployeeID, PH.priceHistoryId
        FROM @ReceiptDetails RD
        INNER JOIN PriceHistory PH ON PH.product_id = RD.ProductID;

		-- Step 3: Update QuantityDelivered of PurchaseOrderDetail
		UPDATE POD
		SET POD.QuantityDelivered = 
			CASE 
				-- Nếu chưa nhập kho (QuantityDelivered là NULL hoặc 0), cập nhật số lượng nhập kho
				WHEN POD.QuantityDelivered IS NULL OR POD.QuantityDelivered = 0 THEN RD.Quantity
				-- Nếu số lượng đã giao hiện tại cộng thêm số lượng nhập kho không vượt quá số lượng đã đặt
				WHEN POD.QuantityDelivered + RD.Quantity <= POD.quantity THEN POD.QuantityDelivered + RD.Quantity
				-- Nếu số lượng đã giao cộng thêm số lượng nhập kho vượt quá số lượng đã đặt, chỉ cập nhật bằng số lượng đã đặt
				ELSE POD.quantity
			END
		FROM PurchaseOrderDetail POD
		INNER JOIN PriceHistory PH 
			ON POD.priceHistoryId = PH.priceHistoryId
		INNER JOIN @ReceiptDetails RD 
			ON PH.product_id = RD.ProductID
			AND POD.PurchaseOrderID = RD.PurchaseOrderID
		WHERE POD.PurchaseOrderID = RD.PurchaseOrderID
			AND PH.product_id = RD.ProductID; -- Điều kiện này đảm bảo chỉ cập nhật đúng sản phẩm


        -- Step 4: Update Quantity in Cells
        UPDATE C 
        SET C.Quantity = C.Quantity + RD.Quantity
        FROM Cells C
        INNER JOIN @ReceiptDetails RD ON C.CellID = RD.CellID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

GRANT EXECUTE ON TYPE::dbo.ReceiptDetailType TO WarehouseEmployee;
GRANT EXEC ON OBJECT::dbo.sp_InsertWarehouseReceipt TO  WarehouseEmployee;

DECLARE @ReceiptDetails dbo.ReceiptDetailType;
INSERT INTO @ReceiptDetails (ProductID, CellID, Quantity, PurchaseOrderID)
VALUES
	(3, 10, 1, 1),
    (4, 12, 1, 1),
    (5, 11, 1, 1);
SELECT * FROM @ReceiptDetails
DECLARE @WarehouseID INT = 2;
EXEC sp_InsertWarehouseReceipt
    @WarehouseID = @WarehouseID,
    @ReceiptDetails = @ReceiptDetails;
go
--======================================================================
go
CREATE PROCEDURE sp_GetUndeliveredPurchaseOrders
AS
BEGIN
    SELECT DISTINCT POD.PurchaseOrderID
    FROM PurchaseOrderDetail POD
    WHERE POD.QuantityDelivered < POD.Quantity;
END;
GO

GRANT EXEC ON OBJECT::dbo.sp_GetUndeliveredPurchaseOrders TO  WarehouseEmployee;

exec sp_GetUndeliveredPurchaseOrders

--=========================================
CREATE PROCEDURE sp_GetPurchaseOrderDetails
    @PurchaseOrderID INT
AS
BEGIN
    SELECT 
        P.product_id,
        P.product_name,
        POD.quantity AS QuantityOrdered,
        POD.QuantityDelivered,
        C.CellID,
        C.CellName,
        POD.priceHistoryId,
        PH.price
    FROM PurchaseOrderDetail POD
    INNER JOIN PriceHistory PH 
        ON POD.priceHistoryId = PH.priceHistoryId
    INNER JOIN Product P 
        ON PH.product_id = P.product_id
    LEFT JOIN Cells C 
        ON C.product_id = P.product_id
    WHERE POD.PurchaseOrderID = @PurchaseOrderID
      AND POD.QuantityDelivered < POD.quantity;
END;
GO

EXEC sp_GetPurchaseOrderDetails @PurchaseOrderID = 1;

GRANT EXEC ON OBJECT::dbo.sp_GetPurchaseOrderDetails TO  WarehouseEmployee;

--===============Xóa Phiếu Nhập========================
CREATE PROCEDURE sp_DeleteWarehouseReceipt
    @WarehouseReceiptID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Kiểm tra xem WarehouseReceiptID có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM WarehouseReceipt WHERE WarehouseReceiptID = @WarehouseReceiptID)
        BEGIN
            SET @Message = N'Phiếu nhập kho không tồn tại';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Step 1: Xóa chi tiết nhập kho
        DELETE FROM WarehouseReceiptDetail
        WHERE WarehouseReceiptID = @WarehouseReceiptID;

        -- Step 2: Xóa phiếu nhập kho
        DELETE FROM WarehouseReceipt
        WHERE WarehouseReceiptID = @WarehouseReceiptID;

        SET @Message = N'Xóa thành công';

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Message = N'Xóa không thành công: ' + ERROR_MESSAGE();
    END CATCH;
END;
GO

-- Gán quyền 
GRANT EXEC ON OBJECT::dbo.sp_DeleteWarehouseReceipt TO  WarehouseEmployee;

-- Thực thi
DECLARE @ResultMessage NVARCHAR(100);
EXEC sp_DeleteWarehouseReceipt 
    @WarehouseReceiptID = 12,
    @Message = @ResultMessage OUTPUT;

SELECT @ResultMessage AS ResultMessage;

go
--===============Xem Thông Tin Phiếu Nhập========================
CREATE PROCEDURE sp_GetWarehouseReceiptInfo
    @WarehouseReceiptID INT
AS
BEGIN
    -- Trả về thông tin phiếu nhập kho
    SELECT 
        WR.WarehouseReceiptID,
        WR.EmployeeID,
        WR.CreateAt,
        WR.WarehouseID
    FROM 
        WarehouseReceipt WR
    WHERE 
        WR.WarehouseReceiptID = @WarehouseReceiptID;

    -- Trả về chi tiết phiếu nhập kho
    SELECT 
        WRD.WarehouseReceiptID,
        WRD.CellID,
        WRD.product_id,
        WRD.quantity,
        WRD.PurchaseOrderID,
        WRD.UpdateTime,
        WRD.UpdateBy
    FROM 
        WarehouseReceiptDetail WRD
    WHERE 
        WRD.WarehouseReceiptID = @WarehouseReceiptID;
END;
GO


-- Gán quyền 
GRANT EXEC ON OBJECT::dbo.sp_GetWarehouseReceiptInfo TO  WarehouseEmployee;

-- Thực thi
EXEC sp_GetWarehouseReceiptInfo @WarehouseReceiptID = 8;
go
--===============Xem Thông Tin Phiếu Nhập Theo Kho========================

CREATE PROCEDURE sp_GetWarehouseReceiptsByWarehouse
    @WarehouseID INT
AS
BEGIN
    -- Trả về thông tin các phiếu nhập theo kho cùng tên nhân viên và tên kho
    SELECT 
        WR.WarehouseReceiptID,
        E.EmployeeID,
        WR.CreateAt,
        W.WarehouseName
    FROM 
        WarehouseReceipt WR
    JOIN 
        Employee E ON WR.EmployeeID = E.EmployeeID
    JOIN 
        Warehouse W ON WR.WarehouseID = W.WarehouseID
    WHERE 
        WR.WarehouseID = @WarehouseID;
END;
GO

-- Gán quyền 
GRANT EXEC ON OBJECT::dbo.sp_GetWarehouseReceiptsByWarehouse TO  WarehouseEmployee;

-- Thực thi
EXEC sp_GetWarehouseReceiptsByWarehouse @WarehouseID = 2;



--====================== XUẤT KHO =======================================
-- Trigger
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

--DROP TRIGGER [dbo].[trg_validate_cell_for_warehouse]
-- Tạo lại trigger
CREATE TRIGGER [dbo].[trg_validate_cell_for_warehouse]
ON [dbo].[DeliveryNoteDetail]
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN DeliveryNote dn ON i.DeliveryNote = dn.DeliveryNoteId
        INNER JOIN Warehouse w ON w.WarehouseID = dn.WarehouseID
        INNER JOIN Shelve s ON s.WarehouseID = w.WarehouseID
        INNER JOIN Cells c ON 
            i.CellId = c.CellId AND 
            c.ShelvesID = s.ShelvesID AND 
            c.product_id = i.product_id AND
            c.Quantity < i.quantity -- Điều kiện kiểm tra đúng
        WHERE w.WarehouseId = dn.WarehouseId
    )
    BEGIN
        RAISERROR(N'Kiểm tra lại DeliveryNoteDetail!', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

GO

CREATE TYPE dbo.DeliveryOrderDetailType AS TABLE (
    OrderID INT,
    priceHistoryId INT,
    Quantity INT,
    CellID INT
);
GO

CREATE PROCEDURE sp_ExportWarehouseGoodsByOrder
    @WarehouseID INT,
	@Note NVARCHAR(50),
    @OrderDetails dbo.DeliveryOrderDetailType READONLY
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Step 1: Add DeliveryNote
        DECLARE @DeliveryNoteID INT;
        DECLARE @EmployeeID INT;

        SELECT @EmployeeID = UF.Employ_ID 
        FROM UserInfo UF 
        WHERE UF.AccountName = SUSER_NAME();

        -- Lấy OrderID từ @OrderDetails
        DECLARE @OrderID INT;
        SELECT TOP 1 @OrderID = OrderID FROM @OrderDetails;

        INSERT INTO DeliveryNote (CreateAt, CreateBy, OrderId, WarehouseId, Note)
        VALUES (GETDATE(), @EmployeeID, @OrderID, @WarehouseID, @Note);

        -- Get ID of the newly created DeliveryNote
        SET @DeliveryNoteID = SCOPE_IDENTITY();

        -- Step 2: Add DeliveryNoteDetail based on OrderDetails
        INSERT INTO DeliveryNoteDetail (DeliveryNote, product_id, CellID, quantity)
        SELECT 
            @DeliveryNoteID, 
            PH.product_id, 
            OD.CellID, 
            OD.Quantity
        FROM 
            @OrderDetails OD
        JOIN 
            PriceHistory PH ON OD.priceHistoryId = PH.priceHistoryId;

        -- Step 3: Update Quantity in Cells and ensure quantity does not go below 0
        UPDATE C
        SET C.Quantity = C.Quantity - OD.Quantity
        FROM Cells C
        INNER JOIN @OrderDetails OD ON C.CellID = OD.CellID
        INNER JOIN PriceHistory PH ON OD.priceHistoryId = PH.priceHistoryId AND C.product_id = PH.product_id
        WHERE C.Quantity >= OD.Quantity;

        -- Check for any rows where quantity would go below 0
        IF EXISTS (SELECT 1 FROM Cells C 
                   INNER JOIN @OrderDetails OD 
                   ON C.CellID = OD.CellID 
                   INNER JOIN PriceHistory PH 
                   ON OD.priceHistoryId = PH.priceHistoryId AND C.product_id = PH.product_id
                   WHERE C.Quantity - OD.Quantity < 0)
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('Quantity cannot go below 0.', 16, 1);
            RETURN;
        END

		-- Step 4: Update the state of the order to 3 (Completed)
        UPDATE [Order]
        SET State = 3
        WHERE Order_ID = @OrderID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
--=============================================================
GRANT EXECUTE ON TYPE::dbo.DeliveryOrderDetailType TO WarehouseEmployee;
GRANT EXEC ON OBJECT::dbo.sp_ExportWarehouseGoodsByOrder TO WarehouseEmployee;

--===================THUC THI========================================
-- Tạo bảng tạm @OrderDetails và thêm dữ liệu vào
DECLARE @OrderDetails dbo.DeliveryOrderDetailType;

-- Thêm dữ liệu vào bảng tạm
INSERT INTO @OrderDetails (OrderID, priceHistoryId, Quantity, CellID)
VALUES
    (24, 1, 1, 10),
    (24, 2, 2, 11),
    (24, 3, 2, 12);

-- Thực thi thủ tục lưu trữ
EXEC sp_ExportWarehouseGoodsByOrder
    @WarehouseID = 2,
    @Note = N'Xuất hàng theo đơn hàng',
    @OrderDetails = @OrderDetails;

--=======================XUẤT KHO======================================
--========Get ORDER IDs==================
CREATE PROCEDURE SP_GetOrderIDs
AS
BEGIN
    SELECT Order_ID
    FROM [Order]
	WHERE State = 2;
END

GRANT EXEC ON OBJECT::dbo.SP_GetOrderIDs TO  WarehouseEmployee;


--GET DETAIL ORDER BY DELIVERY NOTE
GO

CREATE PROCEDURE SP_GetOrderDetailsWE
    @OrderID INT
AS
BEGIN
    SELECT 
        p.product_id,
        p.product_name,
        od.Quantity AS order_quantity,
        c.CellID,
        c.CellName,
        w.WarehouseID,
        w.WarehouseName,
        od.priceHistoryId
    FROM 
        OrderDetail od
    JOIN 
        PriceHistory ph ON od.priceHistoryId = ph.priceHistoryId
    JOIN 
        Product p ON ph.product_id = p.product_id
    JOIN 
        Cells c ON p.product_id = c.product_id
    JOIN 
        Shelve s ON c.ShelvesID = s.ShelvesID
    JOIN 
        Warehouse w ON s.WarehouseID = w.WarehouseID
    WHERE 
        od.Order_Id = @OrderID
        AND c.Quantity >= od.Quantity
END


GRANT EXEC ON OBJECT::dbo.SP_GetOrderDetailsWE TO  WarehouseEmployee;

EXEC SP_GetOrderDetailsWE @OrderID = 26;


--================GÁN QUYỀN==========================================================
--====================================================================================
GRANT EXECUTE ON OBJECT::dbo.SP_GetCommunesByDistrictID TO  WarehouseEmployee;

GRANT EXECUTE ON OBJECT::dbo.SP_GetDistrict TO  WarehouseEmployee;

GRANT EXECUTE ON OBJECT::dbo.SP_GetDistrictByProvinceID TO  WarehouseEmployee;

GRANT EXECUTE ON OBJECT::dbo.SP_GetAddressById TO  WarehouseEmployee;

GRANT EXECUTE ON OBJECT::dbo.SP_GetProvinces TO  WarehouseEmployee;

GRANT EXEC ON OBJECT::dbo.InsertCommune TO  WarehouseEmployee;

GRANT EXEC ON OBJECT::dbo.InsertAddress TO  WarehouseEmployee;

GRANT EXEC ON OBJECT::dbo.GetAddressID TO  WarehouseEmployee;

GRANT EXECUTE ON OBJECT::dbo.SP_GetFullAddress TO  WarehouseEmployee;

--Bổ sung PROC
CREATE PROCEDURE GetWarehouseByName
    @WarehouseName NVARCHAR(100),
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Kiểm tra xem có bất kỳ Warehouse nào phù hợp không
    IF NOT EXISTS (SELECT 1 FROM Warehouse WHERE WarehouseName LIKE '%' + @WarehouseName + '%')
    BEGIN
        SET @Message = N'Không tìm thấy kho với tên đã cho.';
        RETURN;
    END

    -- Truy vấn thông tin các kho phù hợp với tên
    SELECT 
        w.WarehouseID,
        w.WarehouseName,
        w.AddressID,
        COALESCE(a.Note, '') + N', Xã ' + COALESCE(c.CommuneName, '') + N', Huyện ' + COALESCE(d.DistrictName, '') + N', Tỉnh ' + COALESCE(p.ProvinceName, '') AS FullAddress,
        w.ModifiedBy,
        w.CreateTime,
        w.ModifiedTime
    FROM 
        Warehouse w
    LEFT JOIN Address a ON w.AddressID = a.AddressID
    LEFT JOIN Commune c ON a.CommuneID = c.CommuneID
    LEFT JOIN District d ON c.DistrictID = d.DistrictID
    LEFT JOIN Province p ON d.ProvinceID = p.ProvinceID
    WHERE 
        w.WarehouseName LIKE '%' + @WarehouseName + '%' AND w.DeleteTime IS NULL;

    -- Đặt thông báo thành công
    SET @Message = N'Đã lấy danh sách kho thành công!';
END;
GO

--Phân quyền
GRANT EXEC ON OBJECT::dbo.GetWarehouseByName TO  WarehouseEmployee;
