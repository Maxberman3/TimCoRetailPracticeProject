using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TRMDesktopUI
{
    class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
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
            container.Instance(container);

            container.
                Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();
            GetType().Assembly.GetTypes().Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => container.RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));
        }
    }
}
