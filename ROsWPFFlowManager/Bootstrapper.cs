using AutoMapper;
using Caliburn.Micro;
using ROsWPFFlowManager.Helpers;
using ROsWPFFlowManager.Models;
using ROsWPFFlowManager.ViewModels;
using ROsWPFUserInterface.Library.Api;
using ROsWPFUserInterface.Library.Helpers;
using ROsWPFUserInterface.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ROsWPFFlowManager
{
    class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        { 
            Initialize();



            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
          
        }

        private IMapper ConfigureAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductTypeModel, ProductTypeDisplayModel>();
             //   cfg.CreateMap<TaskModel, TaskDisplayModel>();

            });
            var output = config.CreateMapper();
            return output;
        }

        protected override void Configure()
        {
            _container.Instance(ConfigureAutomapper());

            _container.Instance(_container)
                .PerRequest<IProductTypeEndpoint, ProductTypeEndpoint>()
                .PerRequest<ITaskEndpoint, TaskEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper, ConfigHelper>()
                .Singleton<IAPIHelper, APIHelper>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)  // Override startup methods
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

    }


    
}
