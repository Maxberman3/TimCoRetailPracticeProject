using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

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
		private readonly bool isErrorVisible;

		public bool IsErrorVisible
		{
			get
			{
				bool output = false;
				if (ErrorMessage.Length > 0)
				{
					output = true;
				}
				return output;
			}
		}
		private string errorMessage;

		public string ErrorMessage
		{
			get => errorMessage;
			set
			{
				errorMessage = value;
				NotifyOfPropertyChange(() => ErrorMessage);
				NotifyOfPropertyChange(() => IsErrorVisible);
			}
		}


		public LoginViewModel(IAPIHelper iAPIHelper)
		{
			this.iAPIHelper = iAPIHelper;
		}
		public async Task LogIn()
		{
			try
			{
				ErrorMessage = "";
				AuthenticatedUser result = await iAPIHelper.AuthenticateAsync(Username, Password);
				//Capture more information about the user
				await iAPIHelper.GetLoggedInUserInfo(result.Access_Token);
			}
			catch (Exception e)
			{
				ErrorMessage = e.Message;
			}
		}
	}
}