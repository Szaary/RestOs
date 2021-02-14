using AutoMapper;
using Caliburn.Micro;
using ROsWPFUserInterface.Library.Api;
using ROsWPFUserInterface.Library.Helpers;
using ROsWPFUserInterface.Library.Models;
using ROsWPFUserInterface.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ROsWPFUserInterface.ViewModels
{
    public class SalesViewModel : Screen
	{ 

		IProductEndpoint _productEndpoint;
		IConfigHelper _configHelper;
		ISaleEndPoint _saleEndPoint;
		IMapper _mapper;
		private readonly StatusInfoViewModel _status;
		private readonly IWindowManager _window;

		public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint, IMapper mapper, StatusInfoViewModel status, IWindowManager window)
		{
			_productEndpoint = productEndpoint;
			_configHelper = configHelper;
			_saleEndPoint = saleEndPoint;
			_mapper = mapper;
			_status = status;
			_window = window;
		}


		/// Override OnViewLoaded for Caliburn Micro

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			try
			{
				await LoadProducts();
			}
			catch (Exception ex)
			{
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.ResizeMode = ResizeMode.NoResize;
				settings.Title = "System Error";

				// var info = IoC.Get<StatusInfoViewModel>(); if you want more than one popup
			if (ex.Message == "Unauthorized")
				{
					_status.UpdateMessage("Unauthorized Acccess", "You do not have permission to interact with the Sales Form");
					await _window.ShowDialogAsync(_status, null, settings);
				}
				else
				{
					_status.UpdateMessage("Fatal Exception", ex.Message);
					await _window.ShowDialogAsync(_status, null, settings);
				}
				TryCloseAsync();
			}
		}


		private async Task LoadProducts()
		{
			var productList = await _productEndpoint.GetAll();
			// Mapping Model from User interface Display model, to product model from api endopoint. They are different: notify or property change etc.
			var products = _mapper.Map<List<ProductDisplayModel>>(productList);
			Products = new BindingList<ProductDisplayModel>(products);
		}
	

		private async Task ResetSalesViewModel()
		{
			Cart = new BindingList<CartItemDisplayModel>();
			// Add clearing a selected cart item if does not it itself
			await LoadProducts();


			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);


		}
		

		private BindingList<ProductDisplayModel> _products;
		public BindingList<ProductDisplayModel> Products
		{
			get { return _products; }
			set
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
				
			}
		}


		private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
		public BindingList<CartItemDisplayModel> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}


		private ProductDisplayModel _selectedProduct;

		public ProductDisplayModel SelectedProduct
		{
			get { return _selectedProduct; }
			set
			{
				_selectedProduct = value;
				NotifyOfPropertyChange(()=>SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		private CartItemDisplayModel _selectedCartItem;

		public CartItemDisplayModel SelectedCartItem
		{
			get { return _selectedCartItem; }
			set
			{
				_selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
			}
		}




		//  Shopping card Summary Options

		private int _itemQuantity = 1;
		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set
			{ 
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
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
			decimal subTotal = 0;
			foreach (var item in Cart)
			{
				subTotal += item.Product.RetailPrice * item.QuantityInCart;
			}
			return subTotal;
		}


		public string Total
		{
			get
			{
				decimal total = CalculateSubTotal() + CalculateTax();
				return total.ToString("C");
			}
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
			decimal taxRate = _configHelper.GetTaxRate()/100;

			taxAmount = Cart
				.Where(x => x.Product.IsTaxable)
				.Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

			return taxAmount;
		}



		// Buttons logic on Sales page
		public bool CanAddToCart
		{

			get
			{
				bool output = false;
				// Make sure something is selected, and there is item quantity
				if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}
				return output;
			}

		}
		public void AddToCart()
		{
			CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

			if(existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
			}
			else
			{
				CartItemDisplayModel item = new CartItemDisplayModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};
				Cart.Add(item);
			}

			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
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
		public void RemoveFromCart()
		{
			
			
			SelectedCartItem.Product.QuantityInStock += 1;
			if (SelectedCartItem.QuantityInCart>1)
			{
				SelectedCartItem.QuantityInCart -= 1;
			}
			else
			{				
				Cart.Remove(SelectedCartItem);
			}


			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
			NotifyOfPropertyChange(() => CanAddToCart);
		}


		public bool CanCheckOut
		{

			get
			{
				bool output = false;
				// Make sure something is in the cart
				if (Cart.Count > 0)
				{
					output = true; 
				}

				return output;
			}

		}
		public async Task CheckOut()
		{
			//Create sale mdel and post to api
			SaleModel sale = new SaleModel();
			foreach (var item in Cart)
			{
				sale.SaleDetails.Add(new SaleDetailModel
				{
					ProductId = item.Product.Id,
					Quantity = item.QuantityInCart
				});

				
			}
			await _saleEndPoint.PostSale(sale);

			await ResetSalesViewModel();
			// ADD EXCEPTION HANDLING
		}

	}
}
