using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountDAO : IAccountDAO
    {
        private readonly string connectionString;

        public AccountDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Account RowToObject(SqlDataReader rdr)
        {
            Account account = new Account();
            account.AccountId = Convert.ToInt32(rdr["account_id"]);
            account.UserId = Convert.ToInt32(rdr["user_id"]);
            account.Balance = Convert.ToDecimal(rdr["balance"]);
            return account;
        }

        public Account GetAccount(int UserID)
        {
            Account account = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM ACCOUNTS WHERE user_id = @UserId", conn);

                    cmd.Parameters.AddWithValue("@UserId", UserID);

                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                         account = RowToObject(rdr);
                        
                    }
                    return account;

                }
            }
            catch (SqlException)
            {
                throw;
            }

        }

    }
}
