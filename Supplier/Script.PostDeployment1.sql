/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
--use Suppliers
--go 
 --EXEC SelectSuppliers
--select * From Supplier

--exec SelectSuppliers
--INSERT INTO Supplier ([SuppliersId], [SuppliersName], [ContactNumber], [Email], [Product])
--VALUES (116, N'joharsim', N'092536253', N'pogi', N'redhorse')
--SELECT * FROM Suppliers

