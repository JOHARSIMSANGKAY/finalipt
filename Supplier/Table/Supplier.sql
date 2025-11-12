CREATE TABLE [dbo].[Supplier]
(
	[SuppliersId] INT NOT NULL PRIMARY KEY,
	[SuppliersName] NVARCHAR (50), 
    [ContactNumber] NCHAR(20) NULL, 
    [Email] NCHAR(10) NULL, 
    [Product] NCHAR(10) NULL
)
