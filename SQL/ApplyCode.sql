CREATE OR ALTER PROCEDURE ApplyCode
    @Code VARCHAR(8),
    @Result TINYINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF LEN(@Code) < 7 OR LEN(@Code) > 8
    BEGIN
        PRINT 'Invalid code length. Code must be between 7 and 8 characters.';
        RETURN;
    END

    BEGIN TRANSACTION;

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM DiscountCodes WHERE Code = @Code)
        BEGIN
            DELETE FROM DiscountCodes WHERE Code = @Code;

            SET @Result = 1;
        END
        ELSE
        BEGIN
            SET @Result = 0;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        PRINT 'An error occurred: ' + ERROR_MESSAGE();
        SET @Result = 0;
    END CATCH;

    SET NOCOUNT OFF;
END;
GO