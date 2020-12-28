using ROsDataManager.Library.Internal.DataAccess;
using ROsDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ROsDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();

            var taxRate = ConfigHelper.GetTaxRate()/100;


            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity

                };

                // get information about this product

                var productInfo = products.GetProductById(detail.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {detail.ProductId} could not be found in the database.");
                }
                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }


                details.Add(detail);
                
            }
            // Create sale model
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };
            sale.Total = sale.SubTotal + sale.Tax;



            
            using(SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTranaction("ROsData");

                    // Save the sale model
                    sql.SaveDataInTransaction("dbo.spSaleInsert", sale);


                    // Get Id from sale mode
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSaleLookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                    // finish filling in the sale detali model
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;

                        // save the sale detail models
                        sql.SaveDataInTransaction("dbo.spSaleDetailInsert", item);

                    }


                    sql.CommitTranaction(); //- using statement should do it 
                }
                catch
                {
                    sql.RollbackTranaction();
                    throw;
                }
            }
            

        }
    }
}
