using ROsWPFUserInterface.Models;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}