using Microsoft.Extensions.Configuration;
using ROsDataManager.Library.Internal.DataAccess;
using ROsDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROsDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public UserData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public List<UserModel> GetUserById(string Id)
        {
            var output = _sqlDataAccess.LoadData<UserModel, dynamic>("dbo.spUserLookup", new { Id }, "ROsData");
            return output;
        }
    }
}
