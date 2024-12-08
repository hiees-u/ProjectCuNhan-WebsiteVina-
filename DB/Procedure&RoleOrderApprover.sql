--DROP PROC SP_getOrderOrderApprover

CREATE PROCEDURE SP_getOrderOrderApprover
AS
BEGIN
   SELECT 
		o.Order_ID AS N'Mã đơn hàng',
		o.Phone AS N'SDT nhận hàng', 
		o.Name_Recipient AS N'Tên người nhận',
		o.Total_Payment AS N'Tổng tiền',
		o.Create_At AS N'Thời gian đặt',
		u.AccountName AS N'Người đặt',
		a.Note + '/' + a.HouseNumber + ' , ' + co.CommuneName + ' , ' + d.DistrictName + ' , ' + p.ProvinceName AS 'Address'
	FROM 
		[Order] o
		LEFT JOIN Customer c ON o.CreateBy = c.customerId
		LEFT JOIN UserInfo u ON c.customerId = u.customer_Id
		LEFT JOIN Address a ON o.Adress_ID = a.AddressID
		LEFT JOIN Commune co ON a.CommuneID = co.CommuneID
		LEFT JOIN District d ON co.DistrictID = d.DistrictID
		LEFT JOIN Province p ON d.ProvinceID = p.ProvinceID
	WHERE 
		o.State = 1
	ORDER BY 
		o.Create_At;
END;

EXEC SP_getOrderOrderApprover

GRANT EXEC ON OBJECT::dbo.SP_getOrderOrderApprover TO OrderApprover;

go

--DROP PROC SP_getOrderDetails
CREATE PROCEDURE SP_getOrderDetails
    @OrderId INT
AS
BEGIN
    SELECT 
		od.priceHistoryId AS 'PriceHistory',
        p.product_name AS 'Name', 
        p.image AS 'Image', 
        ph.price AS 'Gia', 
        SUM(c.Quantity) AS 'SoLuongTon',  -- Tính tổng số lượng tồn kho
        od.Quantity AS 'SoLuongMua'
    FROM OrderDetail od
    LEFT JOIN PriceHistory ph ON od.priceHistoryId = ph.priceHistoryId
    LEFT JOIN Product p ON p.product_id = ph.product_id
    LEFT JOIN Cells c ON c.product_id = ph.product_id
    WHERE od.Order_Id = @OrderId and od.State != 0
    GROUP BY od.priceHistoryId, p.product_name, p.image, ph.price, od.Quantity;
END;

GRANT EXEC ON OBJECT::dbo.SP_getOrderDetails TO OrderApprover;

EXEC SP_getOrderDetails @OrderId = 33;

go
--DROP PROC SP_updateOrder
CREATE PROCEDURE SP_updateStateOrder2
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE [Order]
        SET State = 2
        WHERE Order_ID = @OrderId;

        UPDATE OrderDetail
        SET State = 2
        WHERE Order_Id = @OrderId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        -- Handle error, e.g., logging
        THROW;
    END CATCH;
END;

go
GRANT EXEC ON OBJECT::dbo.SP_updateStateOrder2 TO OrderApprover;

GO

--GET FULL NAME
SELECT SUSER_NAME()
go
CREATE PROC SP_getFullName
AS
BEGIN
	SELECT full_name
	FROM UserInfo
	WHERE AccountName = SUSER_NAME();
END

GRANT EXEC ON OBJECT::dbo.SP_getFullName TO OrderApprover;



