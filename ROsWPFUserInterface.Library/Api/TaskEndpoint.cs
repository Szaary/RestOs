using ROsWPFUserInterface.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Api
{
    public class TaskEndpoint : ITaskEndpoint
    {
        private IAPIHelper _aPIHelper;
        public TaskEndpoint(IAPIHelper aPIHelper)
        {
            _aPIHelper = aPIHelper;
        }

        public async Task<TaskModel> Get()
        {
            using (HttpResponseMessage response = await _aPIHelper.ApiClient.GetAsync("/api/Sale"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<TaskModel>();
                    return result;
                }
                else throw new Exception(response.ReasonPhrase);
            }
        }






    }
}
