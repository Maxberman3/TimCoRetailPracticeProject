using System.Collections.Generic;
using System.Linq;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            List<ProductModel> output = sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");
            return output;
        }
        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            ProductModel output = sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TRMData").FirstOrDefault();
            return output;
        }
    }
}
