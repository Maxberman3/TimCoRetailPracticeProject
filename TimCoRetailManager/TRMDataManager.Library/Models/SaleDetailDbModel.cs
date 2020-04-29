namespace TRMDataManager.Library.Models
{
    class SaleDetailDbModel
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Tax { get; set; }
        public int Quantity { get; set; }
    }
}
