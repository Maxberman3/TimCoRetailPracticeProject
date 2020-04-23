using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public class ProductEndpoint : IProductEndpoint
    {
        private readonly IAPIHelper aPIHelper;

        public ProductEndpoint(IAPIHelper aPIHelper)
        {
            this.aPIHelper = aPIHelper;
        }
        public async Task<List<ProductModel>> GetAllAsync()
        {
            using (HttpResponseMessage response = await aPIHelper.ApiClient.GetAsync("/api/Product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<ProductModel> result = await response.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
