CREATE PROCEDURE [dbo].[spSaleDetailInsert]
	@ProductId int,
    @PurchasePrice money,
	@Quantity int,
	@SaleId int,
	@Tax money

AS
begin
	set nocount on;

	insert into dbo.SaleDetail(SaleId, ProductId, Quantity, PurchasePrice, Tax)
	
	values (@SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax);

end