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

        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);
        }


    }
}
