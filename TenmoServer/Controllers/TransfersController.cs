using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransfersController : ControllerBase
    {

        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;
        public ITransferDAO transferSqlDAO;
        public TransfersController(ITransferDAO transferSqlDAO, IUserDAO userDAO, IAccountDAO accountDAO)
        {
            this.accountDAO = accountDAO;
            this.userDAO = userDAO;
            this.transferSqlDAO = transferSqlDAO;

        }
        [HttpGet()]
        public List<Transfer> GetTransfers()
        {
            int id = int.Parse(User.FindFirst("sub").Value);
            List<Transfer> transfers = transferSqlDAO.GetTransfers(id);
            return transfers;
        }
        [HttpGet("requests")]
        public List<Transfer> GetRequests()
        {
            int id = int.Parse(User.FindFirst("sub").Value);
            List<Transfer> transfers = transferSqlDAO.GetRequests(id);
            return transfers;
        }


        [HttpPost()]
        public Transfer SendTransfer(SendTransfer st)
        {
            //TODO validate data?

            int id = int.Parse(User.FindFirst("sub").Value);
            // call account dao get balance for current user 
            Account account = accountDAO.GetAccount(id);
            decimal balance = account.Balance;


            Transfer transfer = new Transfer();
            transfer.Amount = st.Amount;
            transfer.TransferStatus = TransferStatus.Approved;
            transfer.TransferType = TransferType.Send;
            transfer.AccountTo = st.ToUser;
            transfer.AccountFrom = id;


            return transferSqlDAO.SendTransfer(transfer);

        }

        [HttpPost("requests")]
        public Transfer RequestTransfer(Transfer request)
        {
            //TODO validate data?

            int id = int.Parse(User.FindFirst("sub").Value);
            // call account dao get balance for current user 
            //Account account = accountDAO.GetAccount(id);
            //decimal balance = account.Balance;

            Transfer transfer = new Transfer();
            transfer.Amount = request.Amount;
            transfer.TransferStatus = TransferStatus.Pending;
            transfer.TransferType = TransferType.Request;
            transfer.AccountTo = request.AccountTo;
            transfer.AccountFrom = id;


            return transferSqlDAO.RequestTransfer(transfer);

        }
        [HttpGet("{transferId}")]
        public Transfer TransferDetails(int transferId)
        {
            Transfer transfer = transferSqlDAO.GetTransferById(transferId);

            return transfer;
        }


    }
}
    


