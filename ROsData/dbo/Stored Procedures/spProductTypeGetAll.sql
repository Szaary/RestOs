CREATE PROCEDURE [dbo].[spProductTypeGetAll]
AS
begin
	set nocount on;

	SELECT [Id], [ProductType]
	from [dbo].[ProductType]
	order by ProductType
end