using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using ROsWPFUserInterface.EventModels;
using ROsWPFUserInterface.Library.Api;

namespace ROsWPFUserInterface.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesViewModel;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel, ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            _events = events;
            _salesViewModel = salesViewModel;
            _user = user;
            _apiHelper = apiHelper;


            _events.SubscribeOnPublishedThread(this);           
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }


        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (string.IsNullOrWhiteSpace(_user.Token)==false) output = true;
                return output;
            }
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>());           
        }

        public void Handle(LogOnEvent message)
        {

        }

        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public async Task LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();


            await ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
