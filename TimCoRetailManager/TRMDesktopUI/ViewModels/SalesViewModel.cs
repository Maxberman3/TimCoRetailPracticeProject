using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
	class SalesViewModel : Screen
	{

		private readonly IProductEndpoint productEndpoint;
		private readonly IConfigHelper configHelper;
		private readonly ISaleEndpoint saleEndpoint;
		private readonly IMapper mapper;

		public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint, IMapper mapper)
		{
			this.productEndpoint = productEndpoint;
			this.configHelper = configHelper;
			this.saleEndpoint = saleEndpoint;
			this.mapper = mapper;
		}
		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProductsAsync();
		}
		public async Task LoadProductsAsync()
		{
			List<ProductModel> productsList = await productEndpoint.GetAllAsync();
			List<ProductDisplayModel> products = mapper.Map<List<ProductDisplayModel>>(productsList);
			Products = new BindingList<ProductDisplayModel>(products);
		}
		private BindingList<ProductDisplayModel> products;

		public BindingList<ProductDisplayModel> Products
		{
			get => products;
			set
			{
				products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}
		private ProductDisplayModel selectedProduct;

		public ProductDisplayModel SelectedProduct
		{
			get { return selectedProduct; }
			set
			{
				selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}
		private CartItemDisplayModel selectedCartItem;

		public CartItemDisplayModel SelectedCartItem
		{
			get { return selectedCartItem; }
			set
			{
				selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
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
		private BindingList<CartItemDisplayModel> shoppingCart = new BindingList<CartItemDisplayModel>();

		public BindingList<CartItemDisplayModel> ShoppingCart
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
			foreach (CartItemDisplayModel item in ShoppingCart)
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
			taxAmount = ShoppingCart.Where(x => x.Product.IsTaxable).Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);
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
				if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
				{
					output = true;
				}
				return output;
			}
		}
		public bool CanCheckOut
		{
			get
			{
				bool output = false;
				if (ShoppingCart.Count > 0)
				{
					output = true;
				}
				return output;
			}
		}
		public void AddToCart()
		{
			CartItemDisplayModel existingItem = ShoppingCart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
			}
			else
			{
				CartItemDisplayModel cartItemModel = new CartItemDisplayModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};
				ShoppingCart.Add(cartItemModel);
			}
			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => ShoppingCart);
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}
		public void RemoveFromCart()
		{
			SelectedCartItem.Product.QuantityInStock += 1;
			if (SelectedCartItem.QuantityInCart > 1)
			{
				SelectedCartItem.QuantityInCart -= 1;
			}
			else
			{
				ShoppingCart.Remove(SelectedCartItem);
			}
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => ShoppingCart);
			NotifyOfPropertyChange(() => CanCheckOut);
			NotifyOfPropertyChange(() => CanAddToCart);
		}
		public async Task CheckOutAsync()
		{
			SaleModel saleModel = new SaleModel();
			foreach (CartItemDisplayModel item in ShoppingCart)
			{
				saleModel.saleDetails.Add(new SaleDetailModel
				{
					Id = item.Product.Id,
					Quantity = item.QuantityInCart
				});
			}
			await saleEndpoint.PostSale(saleModel);
			await ResetSalesViewModel();
		}
		private async Task ResetSalesViewModel()
		{
			ShoppingCart = new BindingList<CartItemDisplayModel>();
			await LoadProductsAsync();

			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}
	}
}
