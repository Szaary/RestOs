using ROsWPFUserInterface.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Api
{
    public interface IProductTypeEndpoint
    {
        Task<List<ProductTypeModel>> GetAll();
    }
}