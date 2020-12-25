using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using ROsWPFUserInterface.EventModels;

namespace ROsWPFUserInterface.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesViewModel;
        public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel)
        {
            _events = events;
            _salesViewModel = salesViewModel;

            
            _events.Subscribe(this);           
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesViewModel);


        }
    }
}
