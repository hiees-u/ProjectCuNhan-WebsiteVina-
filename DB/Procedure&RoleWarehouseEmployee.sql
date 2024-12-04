DROP PROC getOrderWarehouseEmp
go
CREATE PROCEDURE getOrderWarehouseEmp
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

EXEC getOrderWarehouseEmp

GRANT EXEC ON OBJECT::dbo.getOrderWarehouseEmp TO WarehouseEmployee;