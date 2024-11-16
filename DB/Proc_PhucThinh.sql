use CAFFEE_VINA_DBv1

CREATE PROCEDURE sp_ThongKeHangBanTrongNgay
    @NgayNhapVao DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.product_id AS MaSanPham,
        p.product_name AS TenSanPham,
        SUM(od.Quantity) AS TongSoLuongBan,
        SUM(od.Quantity * ph.price) AS TongDoanhThu
    FROM dbo.[OrderDetail] od
    INNER JOIN dbo.[Order] o ON od.Order_Id = o.Order_ID
    INNER JOIN dbo.[PriceHistory] ph ON od.priceHistoryId = ph.priceHistoryId
    INNER JOIN dbo.[Product] p ON ph.product_id = p.product_id
    WHERE CONVERT(DATE, o.Create_At) = @NgayNhapVao
    GROUP BY p.product_id, p.product_name
    ORDER BY TongSoLuongBan DESC;
END;





