using System.Collections.Generic;

namespace TRMDesktopUI.Library.Models
{
    public class SaleModel
    {
        public List<SaleDetailModel> saleDetails { get; set; }
        public SaleModel()
        {
            saleDetails = new List<SaleDetailModel>();
        }
    }
}
