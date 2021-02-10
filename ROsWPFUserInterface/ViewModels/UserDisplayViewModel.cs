﻿using Caliburn.Micro;
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


		private UserModel _selectedUser;
		public UserModel SelectedUser
		{
			get { return _selectedUser; }
			set 
			{
				_selectedUser = value;
				SelectedUserName = value.Email;
				UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
				LoadRoles();
				NotifyOfPropertyChange(() => SelectedUser);
			}
		}


		private string _selectedUserName;
		public string SelectedUserName
		{
			get { return _selectedUserName; }
			set
			{ 
				_selectedUserName = value;
				NotifyOfPropertyChange(() => SelectedUserName);
			}
		}


		private BindingList<string> _userRoles = new BindingList<string>();
		public BindingList<string> UserRoles 
		{
			get { return _userRoles; }
			set 
			{ 
				_userRoles = value;
				NotifyOfPropertyChange(() => UserRoles);
			}
		}


		private BindingList<string> _availableRoles = new BindingList<string>();
		public BindingList<string> AvailableRoles
		{
			get { return _availableRoles; }
			set
			{
				_availableRoles = value;
				NotifyOfPropertyChange(() => AvailableRoles);
			}
		}

		private string _selectedAvailableRole;
		public string SelectedAvailableRole
		{
			get { return _selectedAvailableRole; }
			set
			{
				_selectedAvailableRole = value;
				NotifyOfPropertyChange(() => SelectedAvailableRole);
			}
		}

		private string _selectedUserRole;
		public string SelectedUserRole
		{
			get { return _selectedUserRole; }
			set 
			{ 
				_selectedUserRole = value;
				NotifyOfPropertyChange(() => SelectedUserRole);
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

		private async Task LoadRoles()
		{
			var roles = await _userEndpoint.GetAllRoles();
			foreach(var role in roles)
			{
				if (UserRoles.IndexOf(role.Value) < 0)//IndexOf - if dont find first occurance return -1 
				{
					AvailableRoles.Add(role.Value);
				}
			}
		}

		public async void AddSelectedRole()
		{
			//SelectedRoleToAdd
			await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);

			UserRoles.Add(SelectedAvailableRole);
			AvailableRoles.Remove(SelectedAvailableRole);
		}
		public async void RemoveSelectedRole()
		{
			//SelectedRoleToRemove
			await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);

			AvailableRoles.Add(SelectedUserRole);
			UserRoles.Remove(SelectedUserRole);
		}

	}
}