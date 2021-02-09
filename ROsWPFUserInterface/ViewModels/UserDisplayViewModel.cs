using Caliburn.Micro;
using ROsWPFUserInterface.Library.Api;
using ROsWPFUserInterface.Library.Models;
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
    class UserDisplayViewModel : Screen
    {
		private readonly StatusInfoViewModel _status;
		private readonly IWindowManager _window;
		private readonly IUserEndpoint _userEndpoint;

		public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, IUserEndpoint userEndpoint)
		{
			_status = status;
			_window = window;
			_userEndpoint = userEndpoint;
		}

		BindingList<UserModel> _users;
		public BindingList<UserModel> Users
		{
			get
			{
				return _users;
			}
			set
			{
				_users = value;
				NotifyOfPropertyChange(() => Users);
			}
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			try
			{
				await LoadUsers();
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
					_window.ShowDialog(_status, null, settings);
				}
				else
				{
					_status.UpdateMessage("Fatal Exception", ex.Message);
					_window.ShowDialog(_status, null, settings);
				}
				TryClose();
			}
		}


		private async Task LoadUsers()
		{
			var userList = await _userEndpoint.GetAll();
			Users = new BindingList<UserModel>(userList);
		}




	}
}
