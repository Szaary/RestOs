using ROsDataManager.Library.Models;
using System.Collections.Generic;

namespace ROsDataManager.Library.DataAccess
{
    public interface ISaleData
    {
        List<SaleReportModel> GetSalesReport();
        void SaveSale(SaleModel saleInfo, string cashierId);
    }
}