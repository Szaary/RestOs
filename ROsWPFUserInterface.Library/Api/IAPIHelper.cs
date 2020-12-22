using ROsWPFUserInterface.Models;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Api
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}