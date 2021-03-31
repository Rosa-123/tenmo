using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.DAL
{
    public interface ITransferApiDAO
    {
        Transfer RequestTransfer(Transfer request);
        Transfer SendTransfer(SendTransfer transfer);
        Transfer TransferDetails(int transferId);
        IList<Transfer> ViewRequests();
        IList<Transfer> ViewTransfers();


    }
}