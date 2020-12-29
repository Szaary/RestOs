using ROsWPFUserInterface.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Api
{
    public class ProductTypeEndpoint : IProductTypeEndpoint
    {
        private IAPIHelper _aPIHelper;
        public ProductTypeEndpoint(IAPIHelper aPIHelper)
        {
            _aPIHelper = aPIHelper;
        }

        public async Task<List<ProductTypeModel>> GetAll()
        {
            using (HttpResponseMessage response = await _aPIHelper.ApiClient.GetAsync("/api/ProductType"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ProductTypeModel>>();
                    return result;
                }
                else throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
