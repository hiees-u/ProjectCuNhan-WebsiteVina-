--create role TransportStaff
Create ROLE TransportStaff;

go-- P -- R -- O -- C -- E -- D -- U -- R -- E -- 

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