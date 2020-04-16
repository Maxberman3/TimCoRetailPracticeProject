using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        public List<UserModel> GetById()
        {
            string id = RequestContext.Principal.Identity.GetUserId();
            UserData userData = new UserData();
            return userData.GetUserById(id);

        }
    }
}
