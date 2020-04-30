using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly ILoggedInUserModel loggedOnUser;
        private readonly IAPIHelper aPIHelper;

        public IEventAggregator Events { get; }
        public SalesViewModel SalesViewModel { get; }
        public SimpleContainer SimpleContainer { get; }

        public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel, SimpleContainer simpleContainer, ILoggedInUserModel loggedOnUser, IAPIHelper aPIHelper)
        {
            Events = events;
            SalesViewModel = salesViewModel;
            SimpleContainer = simpleContainer;
            this.loggedOnUser = loggedOnUser;
            this.aPIHelper = aPIHelper;
            events.Subscribe(this);
            ActivateItem(SimpleContainer.GetInstance<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(SalesViewModel);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            TryClose();
        }
        public void LogOut()
        {
            loggedOnUser.ResetUserModel();
            aPIHelper.LogOffUser();
            NotifyOfPropertyChange(() => IsLoggedIn);
            ActivateItem(SimpleContainer.GetInstance<LoginViewModel>());
        }
        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (string.IsNullOrEmpty(loggedOnUser.Id) == false)
                {
                    output = true;
                }
                return output;
            }
        }
    }
}
