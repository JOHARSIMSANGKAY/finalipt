CREATE PROCEDURE [dbo].[UpdateSuppliers]
	@SuppliersId INT = NULL,
	@SuppliersName NVARCHAR (50) = NULL, 
    @ContactNumber NCHAR(20) = NULL, 
    @Email NCHAR(10) = NULL, 
    @Product NCHAR(10) = NULL
AS
BEGIN
    UPDATE dbo.[Supplier]
    SET [SuppliersName] = @SuppliersName,
        [ContactNumber] = @ContactNumber,
        [Email] = @Email,
        [Product] = @Product
    WHERE [SuppliersId] = @SuppliersId
END