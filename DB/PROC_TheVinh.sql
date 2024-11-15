﻿--===========================================================================================
--================================== Quản Lý Kho ============================================
--===========================================================================================
--create role Warehouse Employee
Create ROLE WarehouseEmployee;

Grant Select On dbo.Users to WarehouseEmployee
Grant Select, Update, Insert On dbo.UserInfo to WarehouseEmployee
Grant Select On dbo.Province to WarehouseEmployee
Grant Select, Insert On dbo.District to WarehouseEmployee
Grant Select, Insert On dbo.Commune to WarehouseEmployee
Grant Select, Insert On dbo.Address to WarehouseEmployee
Grant Select, Update, Insert, Delete On dbo.Warehouse to WarehouseEmployee

Grant Select, Update, Insert On dbo.DeliveryNote to WarehouseEmployee
Grant Select, Update, Insert On dbo.DeliveryNoteDetail to WarehouseEmployee

Grant Select, Update, Insert On dbo.WarehouseReceipt to WarehouseEmployee
Grant Select, Update, Insert On dbo.WarehouseReceiptDetail to WarehouseEmployee

Grant Select, Update, Insert, Delete On dbo.Shelve to WarehouseEmployee
Grant Select, Update, Insert, Delete On dbo.Cells to WarehouseEmployee
--############################################# GET ALL WAREHOUSE #####################################################################################
go
CREATE PROCEDURE GetAllWarehouses
AS
BEGIN
    SELECT 
        w.WarehouseID,
        w.WarehouseName,
        w.AddressID,
        w.ModifiedBy,
        w.CreateTime,
        w.ModifiedTime,
        w.DeleteTime
    FROM Warehouse w
	WHERE w.DeleteTime IS NULL
    ORDER BY 
        w.CreateTime DESC;
END;
GO

--Lấy thông tin kho
EXEC GetAllWarehouses

--PHân quyền
GRANT EXEC ON OBJECT::dbo.GetAllWarehouses TO  WarehouseEmployee;

--############################################### GET WAREHOUSE BY ID #####################################################################################
go
CREATE PROCEDURE GetWarehouseByID
    @WarehouseID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Warehouse WHERE WarehouseID = @WarehouseID)
    BEGIN
        SET @Message = N'Không tìm thấy Warehouse với ID đã cho';
        RETURN;
    END

    SELECT 
        w.WarehouseID,
        w.WarehouseName,
        w.AddressID,
        w.ModifiedBy,
        w.CreateTime,
        w.ModifiedTime,
        w.DeleteTime
    FROM 
        Warehouse w
    WHERE 
        w.WarehouseID = @WarehouseID AND W.DeleteTime IS NULL;

    SET @Message = N'Đã lấy thông tin kho thành công!';
END;
GO

--Lấy thông tin kho với id
DECLARE @ResultMessage NVARCHAR(100);
EXEC GetWarehouseByID
    @WarehouseID = 25, 
    @Message = @ResultMessage OUTPUT;
PRINT @ResultMessage;

--PHân quyền
GRANT EXEC ON OBJECT::dbo.GetWarehouseByID TO  WarehouseEmployee;

--########################################## INSERT WAREHOUSE #####################################################################################
go
CREATE PROCEDURE AddWarehouse
    @WarehouseName NVARCHAR(30),
	@AddressID INT,
    @ModifiedBy VARCHAR(25),
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
        VALUES (@WarehouseName, @AddressID, @ModifiedBy, GETDATE());

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
--Phân quyền
GRANT EXEC ON OBJECT::dbo.AddWarehouse TO  WarehouseEmployee;

--Thêm Kho
DECLARE @Message NVARCHAR(100);
EXEC AddWarehouse
    @WarehouseName = N'Thêm Kho Test 5 ',
	@AddressID = 1,
    @ModifiedBy = 'Vinh',
    @Message = @Message OUTPUT;
-- Hiển thị giá trị của thông báo
SELECT @Message AS Message;
--################################################### UPDATE WAREHOUSE WITH FULL ADDRESS #####################################################################################
go
CREATE PROCEDURE UpdateWarehouse
    @WarehouseID INT,
    @WarehouseName NVARCHAR(30),
	@AddressID INT,
    @ModifiedBy VARCHAR(25),
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
            ModifiedBy = @ModifiedBy,
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
    @ModifiedBy = 'Vinh',
    @OutputMessage = @OutputMessage OUTPUT;
-- display alert
SELECT @OutputMessage AS N'Result';

--################################################ DELETE WAREHOUSE #####################################################################################
go
CREATE PROCEDURE DeleteWarehouse
    @WarehouseID INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    DECLARE @count_id INT;

    SELECT @count_id = COUNT(*) 
    FROM Warehouse 
    WHERE WarehouseID = @WarehouseID;
    
    IF @count_id = 0 
    BEGIN
        SET @Message = N'WarehouseID không tồn tại';
        RETURN;
    END

    -- Xóa kho (cập nhật DeleteTime thay vì xóa vật lý)
    UPDATE Warehouse 
    SET DeleteTime = GETDATE()
    WHERE WarehouseID = @WarehouseID;

    -- Kiểm tra xem kho đã được xóa thành công chưa
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
--================================== Quản Lý Kệ ============================================
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
    @ModifiedBy VARCHAR(25),
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
        VALUES (@ShelvesName, @WarehouseID, @ModifiedBy, GETDATE());

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
    @WarehouseID = 2,
    @ModifiedBy = 'TheVinh',
    @Message = @ResultMessage OUTPUT;
-- In ra thông báo
PRINT @ResultMessage;


--########################################## UPDATE SHELVE #####################################################################################

GO
CREATE PROCEDURE UpdateShelve
    @ShelvesID INT,
    @ShelvesName NVARCHAR(30),
    @WarehouseID INT,
    @ModifiedBy VARCHAR(25),
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
            ModifiedBy = @ModifiedBy,
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
    @ModifiedBy = 'TheVinh',
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

