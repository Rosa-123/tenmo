using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        List<Transfer> GetTransfers(int userId);
        Transfer RowToObject(SqlDataReader rdr);
        Transfer SendTransfer(Transfer transfer);
        Transfer GetTransferById(int transferId);
        Transfer RequestTransfer(Transfer transaction);
        List<Transfer> GetRequests(int userId);
    }
}