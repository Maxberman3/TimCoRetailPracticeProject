using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using TRMDesktopUI.Helpers;

namespace TRMDesktopUI.ViewModels
{
	public class LoginViewModel : Screen
	{
		private string _username;

		public string Username
		{
			get => _username;
			set
			{
				_username = value;
				NotifyOfPropertyChange(() => Username);
				NotifyOfPropertyChange(() => CanLogIn);
			}
		}
		private string _password;

		public string Password
		{
			get => _password;
			set
			{
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogIn);
			}
		}
		public bool CanLogIn
		{
			get
			{
				bool output = false;
				if (Username?.Length > 0 & Password?.Length > 0)
				{
					output = true;
				}
				return output;
			}
		}
		private readonly IAPIHelper iAPIHelper;
		public LoginViewModel(IAPIHelper iAPIHelper)
		{
			this.iAPIHelper = iAPIHelper;
		}
		public async Task LogIn()
		{
			try
			{
				Models.AuthenticatedUser result = await iAPIHelper.AuthenticateAsync(Username, Password);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}