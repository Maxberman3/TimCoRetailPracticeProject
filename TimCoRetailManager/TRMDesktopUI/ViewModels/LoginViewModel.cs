﻿using Caliburn.Micro;
using System;

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
		public void LogIn()
		{
			Console.WriteLine();
		}
	}
}