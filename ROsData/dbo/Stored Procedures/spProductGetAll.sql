CREATE PROCEDURE [dbo].[spProductGetAll]
	/* @Id nvarchar(100) */
AS
begin
	set nocount on;

	SELECT [Id], [ProductName], [Description], [RetailPrice], [QantityInStock]
	from [dbo].[Product]
	order by ProductName
end