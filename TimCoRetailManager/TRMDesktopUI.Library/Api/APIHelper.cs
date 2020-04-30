using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public class APIHelper : IAPIHelper
    {
        public HttpClient ApiClient { get; set; }
        private ILoggedInUserModel LoggedInUser { get; set; }

        private void InitializeClient()
        {
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["api"])
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
        public APIHelper(ILoggedInUserModel loggedInUser)
        {
            LoggedInUser = loggedInUser;
            InitializeClient();
        }
        public async Task<AuthenticatedUser> AuthenticateAsync(string username, string password)
        {
            FormUrlEncodedContent data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string,string>("password",password),
            });
            using (HttpResponseMessage response = await ApiClient.PostAsync("/token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    AuthenticatedUser result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            using (HttpResponseMessage response = await ApiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<LoggedInUserModel> result = await response.Content.ReadAsAsync<List<LoggedInUserModel>>();
                    LoggedInUser.Id = result[0].Id;
                    LoggedInUser.LastName = result[0].LastName;
                    LoggedInUser.FirstName = result[0].FirstName;
                    LoggedInUser.CreatedDate = result[0].CreatedDate;
                    LoggedInUser.EmailAddress = result[0].EmailAddress;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public void LoggOffUser()
        {
            ApiClient.DefaultRequestHeaders.Clear();
        }
    }
}
