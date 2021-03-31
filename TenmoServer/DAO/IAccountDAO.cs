using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        Account GetAccount(int UserID);
        Account RowToObject(SqlDataReader rdr);
    }
}