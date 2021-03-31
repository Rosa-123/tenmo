using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.DAL
{
    public interface IUserApiDAO
    {
        List<User> GetAllUsers();
    }
}