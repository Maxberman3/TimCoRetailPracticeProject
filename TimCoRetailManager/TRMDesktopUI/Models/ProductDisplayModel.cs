using System.ComponentModel;

namespace TRMDesktopUI.Models
{
    class ProductDisplayModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        public bool IsTaxable { get; set; }

        private int quantityInStock;

        public int QuantityInStock
        {
            get { return quantityInStock; }
            set
            {
                quantityInStock = value;
                CallPropertyChanged(nameof(QuantityInStock));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
