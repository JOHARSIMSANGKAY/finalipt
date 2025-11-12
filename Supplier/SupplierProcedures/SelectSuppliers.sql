CREATE PROCEDURE [dbo].[SelectSuppliers]
	@SuppliersId NVARCHAR (40) = NULL
AS
BEGIN
     SELECT * FROM dbo.[Supplier] AS a WHERE a.[SuppliersId] = @SuppliersId
END
