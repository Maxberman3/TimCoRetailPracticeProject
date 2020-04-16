using System.Collections.Generic;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { Id = id };
            List<UserModel> output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");
            return output;
        }
    }
}
