using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDAO userDAO;
        
        public UserController(IUserDAO userDAO)
        {
            this.userDAO = userDAO;
           
        }
        [HttpGet()]
        public List<User> GetUsers()
        {
            List<User> users = userDAO.GetUsers();
            return users;

        }





    }
}
