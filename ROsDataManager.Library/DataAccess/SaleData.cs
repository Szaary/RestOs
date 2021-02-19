using Microsoft.Extensions.Configuration;
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
    public class SaleData : ISaleData
    {
        private readonly IProductData _productData;
        private readonly ISqlDataAccess _sqlDataAccess;

        public SaleData(IProductData productData, ISqlDataAccess sqlDataAccess)
        {
            _productData = productData;
            _sqlDataAccess = sqlDataAccess;
        }

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                // get information about this product
                var productInfo = _productData.GetProductById(detail.ProductId);
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


            try
            {
                _sqlDataAccess.StartTranaction("ROsData");
                // Save the sale model
                _sqlDataAccess.SaveDataInTransaction("dbo.spSaleInsert", sale);
                // Get Id from sale mode
                sale.Id = _sqlDataAccess.LoadDataInTransaction<int, dynamic>("spSaleLookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();
                // finish filling in the sale detali model
                foreach (var item in details)
                {
                    item.SaleId = sale.Id;
                    // save the sale detail models
                    _sqlDataAccess.SaveDataInTransaction("dbo.spSaleDetailInsert", item);

                }
                _sqlDataAccess.CommitTranaction();
            }
            catch
            {
                _sqlDataAccess.RollbackTranaction();
                throw;
            }

        }

        public List<SaleReportModel> GetSalesReport()
        {
            var output = _sqlDataAccess.LoadData<SaleReportModel, dynamic>("dbo.spSaleSaleReport", new { }, "ROsData");
            return output;
        }
    }
}
