using ROsDataManager.Library.Internal.DataAccess;
using ROsDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROsDataManager.Library.DataAccess
{
    public class ProductTypeData
    {
        public List<ProductTypeModel> GetProductTypes()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductTypeModel, dynamic>("dbo.spProductTypeGetAll", new { }, "ROsData");

            return output;

        }

  

    }
}
