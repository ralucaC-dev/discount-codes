CREATE OR ALTER PROCEDURE GenerateCodes
    @Number INT,
    @Length INT,
    @Success BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Number < 1 OR @Number > 2000
        BEGIN
            PRINT 'Number must be between 1 and 2000.';
            SET @Success = 0;
            RETURN;
        END

        IF @Length < 7 OR @Length > 8
        BEGIN
            PRINT 'Length must be 7 or 8.';
            SET @Success = 0;
            RETURN;
        END

        DECLARE @RandomValue VARCHAR(8000);
        DECLARE @Count INT = 0;

        WHILE @Count < @Number
        BEGIN
            SET @RandomValue = (
                SELECT TOP 1
                    SUBSTRING(CONVERT(VARCHAR(36), NEWID()), 1, @Length)
            );

            IF NOT EXISTS (SELECT 1 FROM DiscountCodes WHERE Code = @RandomValue)
            BEGIN
                INSERT INTO DiscountCodes (Code) VALUES (@RandomValue);
                SET @Count = @Count + 1;
            END
        END

        SET @Success = 1;
    END TRY
    BEGIN CATCH
        PRINT 'An error occurred: ' + ERROR_MESSAGE();
        SET @Success = 0;
    END CATCH

    SET NOCOUNT OFF;
END;
GO