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
    public class InventoryData
    {
        private readonly IConfiguration _config;

        public InventoryData(IConfiguration config)
        {
            _config = config;
        }
        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventoryGetAll", new { }, "ROsData");
            return output;
        }
        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            sql.SaveData("dbo.spInventoryInsert", item, "ROsData");
        }
      
    }

}
