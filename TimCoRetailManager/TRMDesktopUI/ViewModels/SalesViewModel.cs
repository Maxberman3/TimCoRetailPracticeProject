using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
	class SalesViewModel : Screen
	{

		private readonly IProductEndpoint productEndpoint;
		private readonly IConfigHelper configHelper;

		public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
		{
			this.productEndpoint = productEndpoint;
			this.configHelper = configHelper;
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
		private ProductModel selectedProduct;

		public ProductModel SelectedProduct
		{
			get { return selectedProduct; }
			set
			{
				selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		private int itemQuantity = 1;

		public int ItemQuantity
		{
			get => itemQuantity;
			set
			{
				itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}
		private BindingList<CartItemModel> shoppingCart = new BindingList<CartItemModel>();

		public BindingList<CartItemModel> ShoppingCart
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

				return CalculateSubTotal().ToString("C");
			}
		}
		private decimal CalculateSubTotal()
		{
			decimal subtotal = 0;
			foreach (CartItemModel item in ShoppingCart)
			{
				subtotal += item.Product.RetailPrice * item.QuantityInCart;
			}
			return subtotal;
		}
		public string Tax
		{
			get
			{
				return CalculateTax().ToString("C");
			}
		}
		private decimal CalculateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = Convert.ToDecimal(configHelper.GetTaxRate());
			foreach (CartItemModel item in ShoppingCart)
			{
				if (item.Product.IsTaxable)
				{
					taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate);
				}
			}
			return taxAmount;
		}
		public string Total
		{
			get
			{
				return (CalculateSubTotal() + CalculateTax()).ToString("C");
			}
		}

		public bool CanAddToCart
		{
			get
			{
				bool output = false;
				if (ItemQuantity > 0 & SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}
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
		public void AddToCart()
		{
			CartItemModel existingItem = ShoppingCart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
				ShoppingCart.Remove(existingItem);
				ShoppingCart.Add(existingItem);
			}
			else
			{
				CartItemModel cartItemModel = new CartItemModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};
				ShoppingCart.Add(cartItemModel);
			}
			SelectedProduct.QuantityInStock -= itemQuantity;
			itemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => ShoppingCart);
			NotifyOfPropertyChange(() => Tax);
		}
		public void RemoveFromCart()
		{
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => ShoppingCart);
			NotifyOfPropertyChange(() => Tax);

		}
		public void CheckOut()
		{

		}

	}
}
