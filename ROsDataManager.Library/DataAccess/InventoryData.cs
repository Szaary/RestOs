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
    public class InventoryData : IInventoryData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public InventoryData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public List<InventoryModel> GetInventory()
        {
            var output = _sqlDataAccess.LoadData<InventoryModel, dynamic>("dbo.spInventoryGetAll", new { }, "ROsData");
            return output;
        }
        public void SaveInventoryRecord(InventoryModel item)
        {
            _sqlDataAccess.SaveData("dbo.spInventoryInsert", item, "ROsData");
        }

    }

}
