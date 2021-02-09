using ROsWPFUserInterface.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
    }
}