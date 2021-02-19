using Microsoft.Extensions.Configuration;
using ROsDataManager.Library.Internal.DataAccess;
using ROsDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROsDataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ProductData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public List<ProductModel> GetProducts()
        {
            var output = _sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProductGetAll", new { }, "ROsData");
            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            var output = _sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProductGetById", new { Id = productId }, "ROsData").FirstOrDefault();
            return output;
        }
    }
}
