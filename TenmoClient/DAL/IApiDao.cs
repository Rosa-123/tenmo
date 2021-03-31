using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

namespace TenmoClient.DAL
{
    public interface IApiDao
    {
        bool LoggedIn { get; }
        User User { get; }
    }
}
