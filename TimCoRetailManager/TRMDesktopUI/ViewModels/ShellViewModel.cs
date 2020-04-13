using Caliburn.Micro;

namespace TRMDesktopUI.ViewModels
{
    class ShellViewModel : Conductor<object>
    {

        private readonly LoginViewModel loginViewModel;
        public ShellViewModel(LoginViewModel loginViewModel)
        {
            this.loginViewModel = loginViewModel;
            ActivateItem(loginViewModel);
        }
    }
}
