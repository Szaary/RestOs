using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ROsDataManager.Library.DataAccess;
using ROsDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ROsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IConfiguration _config;

        public InventoryController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData(_config);
            return data.GetInventory();
        }

        //[Authorize(Roles = "Manager")] // if you need to have both roles(not one of it), to do something use another authorize
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post(InventoryModel item)
        {
            InventoryData data = new InventoryData(_config);
            data.SaveInventoryRecord(item);
        }


    }
}
