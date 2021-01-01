using AutoMapper;
using Caliburn.Micro;
using ROsWPFFlowManager.EventModels;
using ROsWPFUserInterface.Library.Api;
using ROsWPFUserInterface.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROsWPFFlowManager.ViewModels
{
    public class TaskViewModel : Screen, IHandle<TypeSelectedEvent>
	{
		IConfigHelper _configHelper;
		IMapper _mapper;
		IEventAggregator _events;
		private ITaskEndpoint _taskEndpoint;
		int _selectedType;
		

		public TaskViewModel(IConfigHelper configHelper, IMapper mapper, IEventAggregator events, ITaskEndpoint tasks)
		{
			_configHelper = configHelper;
			_mapper = mapper;
			_events = events;
			_taskEndpoint=tasks;


		_events.Subscribe(this);
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			//await LoadTaskType();

		}

		private async Task LoadTaskType()
		{
			var taskType = await _taskEndpoint.Get();

			//// Automapper - need to check up
			//var productTypes = _mapper.Map<List<ProductTypeDisplayModel>>(productTypeList);
			//ProductTypes = new BindingList<ProductTypeDisplayModel>(productTypes);
		}



		public void Handle(TypeSelectedEvent message)
		{
			_selectedType = message.SelectedItem;
		}



		public async void GetTasks()
		{
			await LoadTaskType();

		}




	}
}
