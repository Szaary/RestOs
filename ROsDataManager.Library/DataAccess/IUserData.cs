using ROsDataManager.Library.Models;
using System.Collections.Generic;

namespace ROsDataManager.Library.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string Id);
    }
}