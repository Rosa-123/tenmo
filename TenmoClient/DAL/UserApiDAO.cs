using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

namespace TenmoClient.DAL
{
    public class UserApiDAO : ApiDAO, IUserApiDAO
    {
        public UserApiDAO(string apiUrl) : base(apiUrl) { }

        public List<User> GetAllUsers()
        {
            RestRequest request = new RestRequest($"user");
            IRestResponse<List<User>> response = client.Get<List<User>>(request);
            CheckResponse(response);
            List<User> users = response.Data;
            return users;
        }


    }
}
