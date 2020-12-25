using ROsWPFUserInterface.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}