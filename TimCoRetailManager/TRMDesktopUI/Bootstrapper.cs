using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI
{
    class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ViewModels.ShellViewModel>();
        }
        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }
        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
        protected override void Configure()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
             {
                 cfg.CreateMap<ProductModel, ProductDisplayModel>();
                 cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
             });
            IMapper mapper = config.CreateMapper();
            container.Instance(mapper);
            container.Instance(container);
            container.PerRequest<IProductEndpoint, ProductEndpoint>();
            container.PerRequest<ISaleEndpoint, SaleEndpoint>();

            container.
                Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IAPIHelper, APIHelper>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper, ConfigHelper>();
            GetType().Assembly.GetTypes().Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => container.RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));
        }
    }
}
