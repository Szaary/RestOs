using AutoMapper;
using Caliburn.Micro;
using ROsWPFUserInterface.Library.Api;
using ROsWPFUserInterface.Library.Helpers;
using ROsWPFUserInterface.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ROsWPFFlowManager.Models;
using ROsWPFFlowManager.EventModels;
using ROsWPFFlowManager.Views;

namespace ROsWPFFlowManager.ViewModels
{


    public class TaskSelectionViewModel : Screen
    {

		private IConfigHelper _configHelper;
		private IMapper _mapper;
		private IProductTypeEndpoint _productTypeEndpoint;
		private IEventAggregator _events;

		public TaskSelectionViewModel(IProductTypeEndpoint productTypeEndpoint, IConfigHelper configHelper, IMapper mapper, IEventAggregator events)
		{
			_configHelper = configHelper;
			_mapper = mapper;
			_productTypeEndpoint = productTypeEndpoint;
			_events = events;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProductTypes();
		}

		private async Task LoadProductTypes()
		{
			var productTypeList = await _productTypeEndpoint.GetAll();


			// Automapper - need to check up
			var productTypes = _mapper.Map<List<ProductTypeDisplayModel>>(productTypeList);



			ProductTypes = new BindingList<ProductTypeDisplayModel>(productTypes);
		}

		private BindingList<ProductTypeDisplayModel> _productTypes;
		public BindingList<ProductTypeDisplayModel> ProductTypes
		{
			get { return _productTypes; }
			set
			{
				_productTypes = value;
				NotifyOfPropertyChange(() => ProductTypes);

			}
		}




		private ProductTypeDisplayModel _selectedProductType;
		public ProductTypeDisplayModel SelectedProductType
		{
			get { return _selectedProductType; }
			set
			{
				_selectedProductType = value;
				NotifyOfPropertyChange(() => _selectedProductType);
				NotifyOfPropertyChange(() => CanSelectThisTasks);
			}
		}


		public bool CanSelectThisTasks
		{

			get
			{
				bool output = false;
				// Make sure something is selected
				// in future - check if role is already taken if can't be multiple 
				if (SelectedProductType != null)
				{
					output = true;
				}
				return output;
			}
		}

		public void SelectThisTasks()
		{
			TypeSelectedEvent typeSelectedEvent = new TypeSelectedEvent();
			typeSelectedEvent.SelectedItem = _selectedProductType.Id;
			_events.PublishOnUIThread(new LogOnEvent());
			_events.PublishOnUIThread(typeSelectedEvent);

		}



	}
}
