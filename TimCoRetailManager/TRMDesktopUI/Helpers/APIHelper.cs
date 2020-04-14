﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.Helpers
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient ApiClient { get; set; }

        private void InitializeClient()
        {
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["api"])
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public APIHelper()
        {
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
    }
}
