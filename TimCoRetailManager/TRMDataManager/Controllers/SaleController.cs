using Microsoft.AspNet.Identity;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    //[Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
            SaleData Data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();
            Data.SaveSale(sale, userId);
        }
    }
}
