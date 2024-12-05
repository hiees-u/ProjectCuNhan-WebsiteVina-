CREATE PROCEDURE SP_getOrderOrderApprover
AS
BEGIN
    SELECT 
        Order_ID AS N'Mã đơn hàng',
		o.Phone AS N'SDT nhận hàng', 
        Name_Recipient AS N'Tên người nhận',
        Total_Payment AS N'Tổng tiền',
        Create_At AS N'Thời gian đặt',
		u.AccountName as N'Người đặt'
    FROM [Order] o, UserInfo u, Customer c
    WHERE State = 1 and o.CreateBy = c.customerId and c.customerId = u.customer_Id
    ORDER BY Create_At;
END;

EXEC SP_getOrderOrderApprover

GRANT EXEC ON OBJECT::dbo.SP_getOrderOrderApprover TO OrderApprover;

go

CREATE PROCEDURE SP_getOrderDetails
    @OrderId INT
AS
BEGIN
    SELECT 
        p.product_name AS 'Name', 
        p.image AS 'Image', 
        ph.price AS 'Gia', 
        SUM(c.Quantity) AS 'SoLuongTon',  -- Tính tổng số lượng tồn kho
        od.Quantity AS 'SoLuongMua'
    FROM OrderDetail od
    LEFT JOIN PriceHistory ph ON od.priceHistoryId = ph.priceHistoryId
    LEFT JOIN Product p ON p.product_id = ph.product_id
    LEFT JOIN Cells c ON c.product_id = ph.product_id
    WHERE od.Order_Id = @OrderId
    GROUP BY p.product_name, p.image, ph.price, od.Quantity;
END;

GRANT EXEC ON OBJECT::dbo.SP_getOrderDetails TO OrderApprover;

EXEC SP_getOrderDetails @OrderId = 30;
