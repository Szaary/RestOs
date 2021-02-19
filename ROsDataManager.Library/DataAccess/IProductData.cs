using ROsDataManager.Library.Models;
using System.Collections.Generic;

namespace ROsDataManager.Library.DataAccess
{
    public interface IProductData
    {
        ProductModel GetProductById(int productId);
        List<ProductModel> GetProducts();
    }
}