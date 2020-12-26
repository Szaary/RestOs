using ROsWPFUserInterface.Library.Models;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Api
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel sale);
    }
}