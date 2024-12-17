--create role TransportStaff
Create ROLE TransportStaff;

go-- P -- R -- O -- C -- E -- D -- U -- R -- E -- 
DROP PROC UpdateOrderState
CREATE PROCEDURE UpdateOrderState
    @OrderID INT
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Update State in OrderDetail table
        UPDATE OrderDetail
        SET State = 4 -- đã giao
        WHERE Order_Id = @OrderID;

        -- Update State in Order table
        UPDATE [Order]
        SET State = 4
        WHERE Order_ID = @OrderID;

        -- Commit the transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback the transaction if there's an error
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
go
GRANT EXECUTE ON OBJECT::dbo.UpdateOrderState TO TransportStaff;

GO--###########
--DROP PROC sp_GetOrderTS
CREATE PROCEDURE sp_GetOrderTS
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Tính toán số bản ghi bỏ qua
    DECLARE @Offset INT;
    SET @Offset = (@PageNumber - 1) * @PageSize;

    SELECT 
        O.Order_ID AS ID,
		O.Name_Recipient AS NAME,
        O.Phone AS Phone, 
        A.Note + '/' + A.HouseNumber + ', ' + C.CommuneName + ', ' + D.DistrictName + ', ' + P.ProvinceName AS Addres, 
        O.Total_Payment AS Totalpayment, 
        O.Create_At, 
        O.paymentStatus
    FROM [Order] O
    JOIN Address A ON O.Adress_ID = A.AddressID
    JOIN Commune C ON A.CommuneID = C.CommuneID
    JOIN District D ON C.DistrictID = D.DistrictID
    JOIN Province P ON D.ProvinceID = P.ProvinceID
	WHERE State = 3
    ORDER BY O.Create_At
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;

GRANT EXECUTE ON OBJECT::dbo.sp_GetOrderTS TO TransportStaff;

EXEC sp_GetOrderTS 1, 10

select * From Roles