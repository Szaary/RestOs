using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ROsDataManager.Library.DataAccess;
using ROsDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ROsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SaleController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData(_config);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // old way - RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(sale, userId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReports()
        {

            // Different options by role 
            //if (RequestContext.Principal.IsInRole("Admin"))
            //{
            //    // Do admin stuff
            //}
            //else if (RequestContext.Principal.IsInRole("Manager"))
            //{
            //    // Do manager stuff
            //}


            SaleData data = new SaleData(_config);
            return data.GetSalesReport();
        }
    }
}
