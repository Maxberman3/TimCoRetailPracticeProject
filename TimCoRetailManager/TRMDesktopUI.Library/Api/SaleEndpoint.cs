using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private readonly IAPIHelper aPIHelper;

        public SaleEndpoint(IAPIHelper aPIHelper)
        {
            this.aPIHelper = aPIHelper;
        }
        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage response = await aPIHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {
                    //Log successfull call?
                }
                else
                {
                    throw new System.Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
