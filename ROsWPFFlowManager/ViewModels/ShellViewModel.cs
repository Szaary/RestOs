using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using ROsWPFFlowManager.EventModels;
using ROsWPFUserInterface.Library.Api;

namespace ROsWPFFlowManager.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        
        private IEventAggregator _events;
        private TaskSelectionViewModel _taskSelectionViewModel;
        private TaskViewModel _taskViewModel;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;

        public ShellViewModel(IEventAggregator events, TaskSelectionViewModel taskSelectionViewModel, ILoggedInUserModel user, IAPIHelper apiHelper, TaskViewModel taskViewModel)
        {
            _events = events;
            _taskSelectionViewModel = taskSelectionViewModel;
            _user = user;
            _apiHelper = apiHelper;
            _taskViewModel = taskViewModel;


            _events.Subscribe(this);
            ActivateItem(IoC.Get<LoginViewModel>());
        }
        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (string.IsNullOrWhiteSpace(_user.Token) == false) output = true;
                return output;
            }
        }


        public void Handle(LogOnEvent message)
        {
            if(_taskSelectionViewModel.IsActive==false)
                ActivateItem(_taskSelectionViewModel);
            else ActivateItem(_taskViewModel);


            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();


            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
