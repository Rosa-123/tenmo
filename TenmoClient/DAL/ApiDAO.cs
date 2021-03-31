using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;
using TenmoClient.Data;
using RestSharp;
using System.Net;

namespace TenmoClient.DAL
{
    public class ApiDAO
    {
        static private API_User apiUser = null;

        public API_User User
        {
            get
            {
                return User;
            }
            protected set
            {
                User = value;
            }
        }
        static public RestClient client = null;

        public bool LoggedIn
        {
            get
            {
                return (User != null);
            }
        }
        public ApiDAO(string apiUrl)
        {
            if (client == null)
            {
                client = new RestClient(apiUrl);
            }
        }
        protected void CheckResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }

            if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Authorization is required for this option. Please log in.");
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception("You do not have permission to perform the requested action");
                }

                throw new Exception($"Error occurred - received non-success response: {response.StatusCode} ({(int)response.StatusCode})");
            }
        }
    }
}
