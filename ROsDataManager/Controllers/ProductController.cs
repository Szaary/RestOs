using ROsDataManager.Library.DataAccess;
using ROsDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ROsDataManager.Controllers
{
    [Authorize(Roles ="Cashier")]         // Example, how to add custom roles for users
    public class ProductController : ApiController
    { 
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData();
            return data.GetProducts(); 
        }
    }
}
