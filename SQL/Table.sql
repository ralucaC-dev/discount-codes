CREATE TABLE DiscountCodes (
    Code VARCHAR(8) NOT NULL
);

CREATE UNIQUE INDEX IX_DiscountCodes_Code
ON DiscountCodes (Code);