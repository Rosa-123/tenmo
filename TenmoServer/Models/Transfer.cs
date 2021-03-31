using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }

        public TransferType TransferType { get; set; }

        public TransferStatus TransferStatus { get; set; }

        public int AccountFrom { get; set; }

        public int AccountTo { get; set; }

        public decimal Amount { get; set; }

        public string FromName { get; set; }

        public string ToName { get; set; }

        public override string ToString()
        {
            return ($"{TransferId} - {TransferType} - {TransferStatus} - ${Amount}");
        }

    }


    public enum TransferType
    {
        Request = 1,
        Send
    }

    public enum TransferStatus
    {
        Pending = 1,
        Approved,
        Rejected
    }

}
