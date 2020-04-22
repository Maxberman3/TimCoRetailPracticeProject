using Caliburn.Micro;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
    class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        public IEventAggregator Events { get; }
        public SalesViewModel SalesViewModel { get; }
        public SimpleContainer SimpleContainer { get; }

        public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel, SimpleContainer simpleContainer)
        {
            Events = events;
            SalesViewModel = salesViewModel;
            SimpleContainer = simpleContainer;
            events.Subscribe(this);
            ActivateItem(SimpleContainer.GetInstance<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(SalesViewModel);
        }
    }
}
