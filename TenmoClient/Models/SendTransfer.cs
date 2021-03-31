using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class SendTransfer
    {
        public int ToUser { get; set; }
        public decimal Amount { get; set; }

    }
}
