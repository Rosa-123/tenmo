using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient.DAL
{
    public class AccountApiDAO : ApiDAO, IAccountApiDAO
    {
        public AccountApiDAO(string apiUrl) : base(apiUrl) { }

        public decimal GetAccountBalance(int userId)
        {
            RestRequest request = new RestRequest($"account/{userId}");
            IRestResponse<Account> response = client.Get<Account>(request);

            CheckResponse(response);
            Account account = response.Data;
            return account.Balance;

        }
        
    }
}
