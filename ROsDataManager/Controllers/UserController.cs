using Microsoft.AspNet.Identity;
using ROsDataManager.Library.DataAccess;
using ROsDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;


namespace ROsDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        public List<UserModel> GetById()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            UserData data = new UserData();
            return data.GetUserById(userId);
        }
    }
}
