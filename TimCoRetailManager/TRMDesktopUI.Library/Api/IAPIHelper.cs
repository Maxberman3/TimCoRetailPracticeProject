﻿using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
        Task<AuthenticatedUser> AuthenticateAsync(string username, string password);
        Task GetLoggedInUserInfo(string token);
        void LogOffUser();
    }
}