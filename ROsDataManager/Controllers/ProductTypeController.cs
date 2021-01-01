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
    public class ProductTypeController : ApiController
    {
        public List<ProductTypeModel> Get()
        {
            ProductTypeData data = new ProductTypeData();
            return data.GetProductTypes();
        }



    }
}
