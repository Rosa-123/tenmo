﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class SendTransfer
    {
        public int ToUser { get; set; }
        public decimal Amount { get; set; }

    }

}