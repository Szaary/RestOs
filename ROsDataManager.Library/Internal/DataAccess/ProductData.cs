using ROsDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROsDataManager.Library.Internal.DataAccess
{
   public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetAll", new { }, "ROsData");

            return output;

        }
    }
}
