using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ROsDataManager.Library.DataAccess;
using ROsDataManager.Library.Models;
using ROsDataManager.Models;
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
        [HttpGet]
        public UserModel GetById()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            UserData data = new UserData();
            return data.GetUserById(userId).First();
        }


        // How to get all users, roles from Default api database
        [Authorize(Roles ="Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            using (var context = new ApplicationDbContext()) { 
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();


                // Populating Application UserModel Roles
                foreach (var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id, 
                        Email = user.Email
                    };
                    foreach (var r in user.Roles)
                    {
                        u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).First().Name);
                    }
                    output.Add(u);
                }

            }
            return output;
        }
    }
}
