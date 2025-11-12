CREATE PROCEDURE [dbo].[CreateSuppliers]
	@SuppliersId INT = NULL,
	@SuppliersName NVARCHAR (50) = NULL, 
    @ContactNumber NCHAR(20) = NULL, 
    @Email NCHAR(10) = NULL, 
    @Product NCHAR(10) = NULL
AS
BEGIN
     INSERT INTO dbo.[Supplier] ([SuppliersId], [SuppliersName], [ContactNumber], [Email], [Product])
     VALUES (@SuppliersId, @SuppliersName, @ContactNumber, @Email, @Product)
END
