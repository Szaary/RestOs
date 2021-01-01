using Microsoft.AspNet.Identity;
using ROsDataManager.Library.DataAccess;
using ROsDataManager.Library.Models;
using ROsDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ROsDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        TaskModel _taskModel;


        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();

            string userId = RequestContext.Principal.Identity.GetUserId();

            _taskModel = data.SaveSale(sale, userId);
        }

        public TaskModel Get()
        {
            return _taskModel;
        }

    }
}
