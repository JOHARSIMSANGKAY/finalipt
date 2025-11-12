CREATE PROCEDURE [dbo].[DeleteSuppliers]
	@SuppliersId NVARCHAR (40) = NULL
AS
BEGIN
     DELETE FROM dbo.[Supplier] WHERE SuppliersId = @SuppliersId
END
