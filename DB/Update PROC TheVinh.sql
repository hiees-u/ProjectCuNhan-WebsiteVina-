CREATE PROCEDURE SP_GetOrderDetailsWE
    @OrderID INT
AS
BEGIN
    -- Tạo bảng tạm để lưu trữ kết quả
    CREATE TABLE #TempOrderDetails (
        product_id INT,
        product_name NVARCHAR(255),
        order_quantity INT,
        CellID INT,
        CellName NVARCHAR(255),
        WarehouseID INT,
        WarehouseName NVARCHAR(255),
        priceHistoryId INT
    )

    -- Duyệt qua từng sản phẩm trong đơn hàng
    DECLARE @current_product_id INT
    DECLARE product_cursor CURSOR FOR
    SELECT DISTINCT p.product_id
    FROM OrderDetail od
    JOIN PriceHistory ph ON od.priceHistoryId = ph.priceHistoryId
    JOIN Product p ON ph.product_id = p.product_id
    WHERE od.Order_Id = @OrderID

    OPEN product_cursor
    FETCH NEXT FROM product_cursor INTO @current_product_id

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Chèn thông tin chi tiết sản phẩm vào bảng tạm từ ô đầu tiên có đủ số lượng yêu cầu
        INSERT INTO #TempOrderDetails
        SELECT TOP 1
            p.product_id,
            p.product_name,
            od.Quantity AS order_quantity,
            c.CellID,
            c.CellName,
            w.WarehouseID,
            w.WarehouseName,
            od.priceHistoryId
        FROM OrderDetail od
        JOIN PriceHistory ph ON od.priceHistoryId = ph.priceHistoryId
        JOIN Product p ON ph.product_id = p.product_id
        JOIN Cells c ON p.product_id = c.product_id
        JOIN Shelve s ON c.ShelvesID = s.ShelvesID
        JOIN Warehouse w ON s.WarehouseID = w.WarehouseID
        WHERE 
            od.Order_Id = @OrderID
            AND p.product_id = @current_product_id
            AND c.Quantity >= od.Quantity
        ORDER BY c.CellID

        FETCH NEXT FROM product_cursor INTO @current_product_id
    END

    CLOSE product_cursor
    DEALLOCATE product_cursor

    -- Trả về kết quả từ bảng tạm
    SELECT * FROM #TempOrderDetails

    -- Xóa bảng tạm
    DROP TABLE #TempOrderDetails
END
GO

GRANT EXEC ON OBJECT::dbo.SP_GetOrderDetailsWE TO  WarehouseEmployee;
go

--==================================================================
CREATE PROCEDURE sp_GetPurchaseOrderDetails
    @PurchaseOrderID INT
AS
BEGIN
    -- Tạo bảng tạm để lưu trữ kết quả
    CREATE TABLE #TempOrderDetails (
        product_id INT,
        product_name NVARCHAR(255),
        QuantityOrdered INT,
        QuantityDelivered INT,
        CellID INT,
        CellName NVARCHAR(255),
        priceHistoryId INT,
        price DECIMAL(18, 2)
    )

    -- Duyệt qua từng sản phẩm trong đơn đặt hàng
    DECLARE @current_product_id INT
    DECLARE product_cursor CURSOR FOR
    SELECT DISTINCT PH.product_id
    FROM PurchaseOrderDetail POD
    JOIN PriceHistory PH ON POD.priceHistoryId = PH.priceHistoryId
    WHERE POD.PurchaseOrderID = @PurchaseOrderID

    OPEN product_cursor
    FETCH NEXT FROM product_cursor INTO @current_product_id

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Chèn thông tin chi tiết sản phẩm vào bảng tạm từ ô đầu tiên có đủ số lượng yêu cầu
        INSERT INTO #TempOrderDetails
        SELECT TOP 1
            P.product_id,
            P.product_name,
            POD.quantity AS QuantityOrdered,
            POD.QuantityDelivered,
            C.CellID,
            C.CellName,
            POD.priceHistoryId,
            PH.price
        FROM PurchaseOrderDetail POD
        INNER JOIN PriceHistory PH ON POD.priceHistoryId = PH.priceHistoryId
        INNER JOIN Product P ON PH.product_id = P.product_id
        LEFT JOIN Cells C ON C.product_id = P.product_id
        WHERE POD.PurchaseOrderID = @PurchaseOrderID
          AND PH.product_id = @current_product_id
          AND POD.QuantityDelivered < POD.quantity
        ORDER BY C.CellID

        FETCH NEXT FROM product_cursor INTO @current_product_id
    END

    CLOSE product_cursor
    DEALLOCATE product_cursor

    -- Trả về kết quả từ bảng tạm
    SELECT * FROM #TempOrderDetails

    -- Xóa bảng tạm
    DROP TABLE #TempOrderDetails
END;
GO

GRANT EXEC ON OBJECT::dbo.sp_GetPurchaseOrderDetails TO  WarehouseEmployee;

GO

--==============================Product ExpriryDate=====================================
CREATE PROCEDURE GetProductsExpiringInNextMonth
AS
BEGIN
    SELECT 
        product_id,
        product_name,
        image,
        totalQuantity,
        Category_id,
        Supplier,
        SubCategoryID,
        ExpriryDate,
        Description,
        ModifiedBy,
        CreateTime,
        ModifiedTime,
        DeleteTime
    FROM 
        Product
    WHERE 
        DATEDIFF(day, GETDATE(), ExpriryDate) <= 30
        AND ExpriryDate > GETDATE()
        AND DeleteTime IS NULL
    ORDER BY 
        ExpriryDate ASC
END

GRANT EXEC ON OBJECT::dbo.GetProductsExpiringInNextMonth TO  WarehouseEmployee;

EXEC GetProductsExpiringInNextMonth
go

--==================Get Info Products By ProductID In Warehouse===============================
--Get List product
CREATE PROCEDURE GetProductList
AS
BEGIN
    SELECT 
        product_id AS ProductId,
        product_name AS ProductName
    FROM 
        Product
    WHERE 
        DeleteTime IS NULL
    ORDER BY 
        product_name ASC
END

GRANT EXEC ON OBJECT::dbo.GetProductList TO  WarehouseEmployee;

exec GetProductList
go
--==================Get Info Products By ProductID===============================
CREATE PROCEDURE GetInfoProductsByProductID
    @product_id INT,
    @Message NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Kiểm tra xem sản phẩm có tồn tại hay không
    IF NOT EXISTS (SELECT 1 FROM Product WHERE product_id = @product_id)
    BEGIN
        SET @Message = N'Không tìm thấy sản phẩm với Mã sản phẩm đã cho!';
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
        p.product_id = @product_id
        AND s.DeleteTime IS NULL 
        AND c.DeleteTime IS NULL 
        AND p.DeleteTime IS NULL
    ORDER BY s.ShelvesName, c.CellName;

    SET @Message = N'Đã lấy thông tin sản phẩm theo Mã sản phẩm thành công!';
END;
GO

GRANT EXEC ON OBJECT::dbo.GetInfoProductsByProductID TO  WarehouseEmployee;


DECLARE @Message NVARCHAR(100);
EXEC GetInfoProductsByProductID
    @product_id = 3,  -- Replace this with the actual product_id
    @Message = @Message OUTPUT;

-- Display the output message
SELECT @Message AS 'OutputMessage';
go

