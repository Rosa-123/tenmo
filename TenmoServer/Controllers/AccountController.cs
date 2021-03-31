using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserDAO userDAO;
        public IAccountDAO accountSqlDAO;
        public AccountController(IAccountDAO accountSqlDAO, IUserDAO userDAO)
        {
            this.userDAO = userDAO;
            this.accountSqlDAO = accountSqlDAO;
            
        }


        [HttpGet("{id}")]
        public Account GetAccountBalance(int userId)
        {
            int id = int.Parse(User.FindFirst("sub").Value);
            Account account = accountSqlDAO.GetAccount(id);
            return account;
        }
    }
}

