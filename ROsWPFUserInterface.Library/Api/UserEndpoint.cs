using ROsWPFUserInterface.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Api
{
    public class UserEndpoint : IUserEndpoint
    {
        private readonly IAPIHelper _aPIHelper;

        public UserEndpoint(IAPIHelper aPIHelper)
        {
            _aPIHelper = aPIHelper;
        }

        public async Task<List<UserModel>> GetAll()
        {
            using (HttpResponseMessage response = await _aPIHelper.ApiClient.GetAsync("/api/User/Admin/GetAllUsers"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<UserModel>>();
                    return result;
                }
                else throw new Exception(response.ReasonPhrase);
            }

        }
    }
}
