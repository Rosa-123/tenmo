using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferDAO : ITransferDAO
    {
        private readonly string connectionString;
        private const string SQL_GETTRANSFERS = "SELECT * FROM transfers t JOIN accounts a on a.account_id = t.account_from WHERE a.user_id = @UserId";
        private const string SQL_GETREQUESTS = "select * from transfers t join accounts a on a.account_id = t.account_from where a.user_id = @UserId and t.transfer_type_id = (select transfer_type_id from transfer_types where transfer_type_desc = 'Request')";
        private const string SQL_TRANSFERBYID = "SELECT t.*, u.username as username FROM transfers t JOIN accounts a on a.account_id = t.account_from JOIN users u on u.user_id = a.user_id WHERE transfer_id = @TransferId";
        private const string SQL_TOUSER = "SELECT t.transfer_id, u.username as username FROM transfers t JOIN accounts a on a.account_id = t.account_to JOIN users u on u.user_id = a.user_id WHERE transfer_id = @TransferId";
        private const string SQL_CREATETRANSFER = @"
begin transaction;
insert into transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount) values(@transferType, @transferStatus, (select account_id from accounts where user_id = @senderId), (select account_id from accounts where user_id = @recId), @amount); 
select @@IDENTITY;
update accounts set balance = balance -@amount where user_id = @senderId; 
update accounts set balance = balance +@amount where user_id = @recId;
commit transaction";
        private const string SQL_REQUESTTRANSFER = @"
begin transaction;
insert into transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount) values(@transferType, @transferStatus, (select account_id from accounts where user_id = @senderId), (select account_id from accounts where user_id = @recId), @amount); 
select @@IDENTITY;
commit transaction";
        private const string SQL_APPROVEREQUEST = "";
        private const string SQL_REJECTREQUEST = "";

        public TransferDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Transfer RowToObject(SqlDataReader rdr)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(rdr["transfer_id"]);
            transfer.TransferType = (TransferType)Convert.ToInt32(rdr["transfer_type_id"]);
            transfer.TransferStatus = (TransferStatus)Convert.ToInt32(rdr["transfer_status_id"]);
            transfer.AccountFrom = Convert.ToInt32(rdr["account_from"]);
            transfer.AccountTo = Convert.ToInt32(rdr["account_to"]);
            transfer.Amount = Convert.ToDecimal(rdr["amount"]);
    
            return transfer;
        }
        public List<Transfer> GetTransfers(int userId)
        {
            List<Transfer> transfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GETTRANSFERS, conn);

                    cmd.Parameters.AddWithValue("@UserId", userId);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Transfer transfer = RowToObject(rdr);
                        transfers.Add(transfer);
                    }
                    return transfers;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Transfer> GetRequests(int userId)
        {
            List<Transfer> transfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GETREQUESTS, conn);

                    cmd.Parameters.AddWithValue("@UserId", userId);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Transfer transfer = RowToObject(rdr);
                        transfers.Add(transfer);
                    }
                    return transfers;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }




        public Transfer SendTransfer(Transfer transaction)
        {
            Transfer transfer = transaction;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_CREATETRANSFER, conn);
                    cmd.Parameters.AddWithValue("@transferType", transaction.TransferType);
                    cmd.Parameters.AddWithValue("@transferStatus", transaction.TransferStatus);
                    cmd.Parameters.AddWithValue("@amount", transaction.Amount);
                    cmd.Parameters.AddWithValue("@senderId", transaction.AccountFrom);
                    cmd.Parameters.AddWithValue("@recId", transaction.AccountTo);
                    int transactionID = Convert.ToInt32(cmd.ExecuteScalar());
                    transfer.TransferId = transactionID;

                }
                return transfer;

            }
            catch (SqlException)
            {
                throw;
            }
        }

        public Transfer RequestTransfer(Transfer transaction)
        {
            Transfer transfer = transaction;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_REQUESTTRANSFER, conn);
                    cmd.Parameters.AddWithValue("@transferType", transaction.TransferType);
                    cmd.Parameters.AddWithValue("@transferStatus", transaction.TransferStatus);
                    cmd.Parameters.AddWithValue("@amount", transaction.Amount);
                    cmd.Parameters.AddWithValue("@senderId", transaction.AccountTo); // TODO
                    cmd.Parameters.AddWithValue("@recId", transaction.AccountFrom);
                    int transactionID = Convert.ToInt32(cmd.ExecuteScalar());
                    transfer.TransferId = transactionID;

                }
                return transfer;

            }
            catch (SqlException)
            {
                throw;
            }

        }


        public Transfer GetTransferById(int transferId)
        {
            Transfer transfer = new Transfer();

            try
            {
                using (SqlConnection conn = new SqlConnection($"{connectionString}MultipleActiveResultSets = true;"))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_TRANSFERBYID, conn);
                    SqlCommand cmd2 = new SqlCommand(SQL_TOUSER, conn);

                    cmd.Parameters.AddWithValue("@TransferId", transferId);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        
                        transfer = RowToObject(rdr);
                        transfer.FromName = Convert.ToString(rdr["username"]);
                    }

                    cmd2.Parameters.AddWithValue("@TransferId", transferId);

                    SqlDataReader rdr2 = cmd2.ExecuteReader();
                    while (rdr2.Read())
                    {
                        transfer.ToName = Convert.ToString(rdr2["username"]);
                    }
                    return transfer;
                }
            }
            catch (SqlException) 
            {
                throw;
            }
        }
    }
}
