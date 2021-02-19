using ROsDataManager.Library.Models;
using System.Collections.Generic;

namespace ROsDataManager.Library.DataAccess
{
    public interface IInventoryData
    {
        List<InventoryModel> GetInventory();
        void SaveInventoryRecord(InventoryModel item);
    }
}