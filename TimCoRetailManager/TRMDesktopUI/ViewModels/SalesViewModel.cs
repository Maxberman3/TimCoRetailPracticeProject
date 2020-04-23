using Caliburn.Micro;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	class SalesViewModel : Screen
	{

		private readonly IProductEndpoint productEndpoint;
		public SalesViewModel(IProductEndpoint productEndpoint)
		{
			this.productEndpoint = productEndpoint;
		}
		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProductsAsync();
		}
		public async Task LoadProductsAsync()
		{
			List<ProductModel> products = await productEndpoint.GetAllAsync();
			Products = new BindingList<ProductModel>(products);
		}
		private BindingList<ProductModel> products;

		public BindingList<ProductModel> Products
		{
			get => products;
			set
			{
				products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}
		private int itemQuantity;

		public int ItemQuantity
		{
			get => itemQuantity;
			set
			{
				itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
			}
		}
		private BindingList<string> shoppingCart;

		public BindingList<string> ShoppingCart
		{
			get => shoppingCart;
			set
			{
				shoppingCart = value;
				NotifyOfPropertyChange(() => ShoppingCart);
			}
		}
		public string SubTotal
		{
			get
			{
				//TODO: replace with calculation
				return "$0.00";
			}
		}
		public string Tax
		{
			get
			{
				//TODO: replace with calculation
				return "$0.00";
			}
		}
		public string Total
		{
			get
			{
				//TODO: replace with calculation
				return "$0.00";
			}
		}

		public bool CanAddToCart
		{
			get
			{
				bool output = false;
				//make sure eligible to add
				return output;
			}
		}
		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;
				//make sure something is selected/can be removed
				return output;
			}
		}
		public bool CanCheckOut
		{
			get
			{
				bool output = false;
				//make sure something is in the cart
				return output;
			}
		}
		public void CheckOut()
		{

		}

	}
}
