use CAFFEE_VINA_DBv1

--########################################################Thong ke ban ra trong ngay cu the#####################################################
CREATE PROCEDURE SP_GetDailySalesReport
    @InputDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
		@InputDate AS InputDate,
        p.product_id AS MaSanPham,
        p.product_name AS TenSanPham,
        SUM(od.Quantity) AS TongSoLuongBan,
        SUM(od.Quantity * ph.price) AS TongDoanhThu
    FROM dbo.[OrderDetail] od
    INNER JOIN dbo.[Order] o ON od.Order_Id = o.Order_ID
    INNER JOIN dbo.[PriceHistory] ph ON od.priceHistoryId = ph.priceHistoryId
    INNER JOIN dbo.[Product] p ON ph.product_id = p.product_id
    WHERE CONVERT(DATE, o.Create_At) = @InputDate
    GROUP BY p.product_id, p.product_name
    ORDER BY TongSoLuongBan DESC;
END;

exec SP_GetDailySalesReport @InputDate='11-11-2024'

EXEC SP_GetDailySalesReport @InputDate = '2024-11-11';
--#####################################################################################thong ke so luong ban ra trong 1 khoang thoi gian cu the#############################
CREATE PROCEDURE SP_GetWeeklySalesReport
    @StartDate DATE, 
    @EndDate DATE    
AS
BEGIN
    SET NOCOUNT ON;


    IF @StartDate > @EndDate
    BEGIN
        PRINT 'Start Date not can higher than End Date';
        RETURN;
    END;

    SELECT 
		@StartDate AS StartDate,
		@EndDate AS EndDate,
        P.product_id AS MaSanPham,
        P.product_name AS TenSanPham,
        SUM(OD.Quantity) AS TongSoLuongBanRa,
        SUM(OD.Quantity * PH.price) AS TongDoanhThu
    FROM 
        dbo.[Order] O
    INNER JOIN 
        dbo.OrderDetail OD ON O.Order_ID = OD.Order_Id
    INNER JOIN 
        dbo.PriceHistory PH ON OD.priceHistoryId = PH.priceHistoryId
    INNER JOIN 
        dbo.Product P ON PH.product_id = P.product_id
    WHERE 
        O.Create_At >= @StartDate 
        AND O.Create_At <= @EndDate 
        AND O.State = 1 
    GROUP BY 
        P.product_id, P.product_name
    ORDER BY 
        TongSoLuongBanRa DESC; 
END;
GO

EXEC SP_GetWeeklySalesReport @StartDate = '2024-11-01', @EndDate = '2024-11-12';

--####################################################################thong ke so luong ban ra trong 1 thang#############################################
CREATE PROCEDURE SP_GetMonthlySalesReport
    @Month INT, 
    @Year INT  
AS
BEGIN
    SET NOCOUNT ON;
    IF @Month NOT BETWEEN 1 AND 12
    BEGIN
        PRINT 'Month values must in 1 to 12.';
        RETURN;
    END;

    IF @Year < 1900 OR @Year > YEAR(GETDATE())
    BEGIN
        PRINT 'Year is not correct';
        RETURN;
    END;
    SELECT
		@Month AS Thang,
		@Year AS Nam,
        P.product_id AS MaSanPham,
        P.product_name AS TenSanPham,
        SUM(OD.Quantity) AS TongSoLuongBanRa,
        SUM(OD.Quantity * PH.price) AS TongDoanhThu
    FROM 
        dbo.[Order] O
    INNER JOIN 
        dbo.OrderDetail OD ON O.Order_ID = OD.Order_Id
    INNER JOIN 
        dbo.PriceHistory PH ON OD.priceHistoryId = PH.priceHistoryId
    INNER JOIN 
        dbo.Product P ON PH.product_id = P.product_id
    WHERE 
        MONTH(O.Create_At) = @Month 
        AND YEAR(O.Create_At) = @Year 
        AND O.State = 1
    GROUP BY 
        P.product_id, P.product_name
    ORDER BY 
        TongSoLuongBanRa DESC;
END;
GO
--############################################################################################thong ke so luong ban ra trong 1 nam#######################
	CREATE PROCEDURE SP_GetSalesReportInYear
		@Year INT 
	AS
	BEGIN
		SET NOCOUNT ON;
		IF @Year < 1900 OR @Year > YEAR(GETDATE())
		BEGIN
			PRINT 'Incorrect value Year. The Year must between 1900 to now :))';
			RETURN;
		END;
		SELECT
			@Year AS Nam,
			P.product_id AS MaSanPham,
			P.product_name AS TenSanPham,
			SUM(OD.Quantity) AS TongSoLuongBanRa,
			SUM(OD.Quantity * PH.price) AS TongDoanhThu
		FROM 
			dbo.[Order] O
		INNER JOIN 
			dbo.OrderDetail OD ON O.Order_ID = OD.Order_Id
		INNER JOIN 
			dbo.PriceHistory PH ON OD.priceHistoryId = PH.priceHistoryId
		INNER JOIN 
			dbo.Product P ON PH.product_id = P.product_id
		WHERE 
			YEAR(O.Create_At) = @Year  
			AND O.State = 1
		GROUP BY 
			P.product_id, P.product_name
		ORDER BY 
			TongSoLuongBanRa DESC;
	END;
	GO

EXEC SP_GetSalesReportInYear
    @Year = 2024;
