using MenuFramework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.DAL;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient.Views
{
    public class MainMenu : ConsoleMenu
    {
        private readonly IUserApiDAO userDAO;
        private readonly IAccountApiDAO dao;
        private readonly ITransferApiDAO transferDAO;

        public MainMenu(IAccountApiDAO dao, ITransferApiDAO transferDAO, IUserApiDAO userDAO)
        {
            this.userDAO = userDAO;
            this.dao = dao;
            this.transferDAO = transferDAO;
            AddOption("View your current balance", ViewBalance)
                .AddOption("View your past transfers", ViewTransfers)
                .AddOption("View your pending requests", ViewRequests)
                .AddOption("Send TE bucks", SendTEBucks)
                .AddOption("Request TE bucks", RequestTEBucks)
                .AddOption("Log in as different user", Logout)
                .AddOption("Exit", Exit);
            this.Configure(config => {
                config.SelectedItemForegroundColor = ConsoleColor.Cyan;
                config.ItemForegroundColor = ConsoleColor.DarkCyan;

            });
        }

        protected override void OnBeforeShow()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("");
            Console.WriteLine($"TE Account Menu for User: {UserService.GetUserName()}");
        }

        private MenuOptionResult ViewBalance()
        {
            Console.WriteLine("*************ACCOUNT BALANCE*************");
            Console.WriteLine($"Your current account balance is: ${dao.GetAccountBalance(UserService.GetUserId())}");
            return MenuOptionResult.WaitAfterMenuSelection;

        }

        private MenuOptionResult ViewTransfers()
        {
            IList<Transfer> transfers = transferDAO.ViewTransfers();
            Console.WriteLine("***************************************");
            Console.WriteLine("            TRANSFERS");
            Console.WriteLine("***************************************");

            foreach (Transfer transfer in transfers)
            {
                Console.WriteLine($"{transfer.ToString()}");
            }

            Console.Write("Enter a transfer ID to view details (0 to cancel): ");

            string Id = Console.ReadLine();
            if (Id == "0")
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }
            int transferId = int.Parse(Id);

            Transfer transferDetails = transferDAO.TransferDetails(transferId);
            Console.WriteLine("***************************************");
            Console.WriteLine("          TRANSFER DETAILS");
            Console.WriteLine("***************************************");
            Console.WriteLine($"Id: {transferDetails.TransferId}");
            Console.WriteLine($"From: {transferDetails.FromName}");
            Console.WriteLine($"To: {transferDetails.ToName}");
            Console.WriteLine($"Type: {transferDetails.TransferType}");
            Console.WriteLine($"Status: {transferDetails.TransferStatus}");
            Console.WriteLine($"Amount: ${transferDetails.Amount}");

            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewRequests()
        {
            IList<Transfer> transfers = transferDAO.ViewRequests();
            Console.WriteLine("***************************************");
            Console.WriteLine("***********TRANSFER REQUESTS***********");
            Console.WriteLine("***************************************");

            foreach (Transfer transfer in transfers)
            {
                Console.WriteLine($"{transfer.ToString()}");
            }
            Console.Write("Enter a transfer ID to approve/reject (0 to cancel): ");

            string Id = Console.ReadLine();
            if (Id == "0")
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }
            int transferId = int.Parse(Id);

            Console.WriteLine("1) Approve");
            Console.WriteLine("2) Reject");
            Console.WriteLine("Please choose an option (0 to cancel): ");
            string input = Console.ReadLine();
            if (input == "0")
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }
            else if (input == "1")
            {

            }
            else if (input == "2")
            {

            }

            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult SendTEBucks()
        {
            List<User> users = userDAO.GetAllUsers();
            Console.WriteLine("**********************");
            Console.WriteLine("       USERS");
            Console.WriteLine("**********************");

            foreach (User user in users)
            {
                Console.WriteLine($"ID: {user.UserId} Name: {user.Username}");
            }
            Console.Write("Enter ID of the user you are sending a transfer to (0 to cancel): ");
            string input = Console.ReadLine();
            if (input == "0")
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }
            int userIdTo = int.Parse(input);
            Console.Write("Enter amount: $");
            string amountString = Console.ReadLine();
            decimal amount = decimal.Parse(amountString);

            if (dao.GetAccountBalance(UserService.GetUserId()) < amount)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("*************ERROR: INSUFFICIENT FUNDS*************");
                System.Threading.Thread.Sleep(1000);
                Console.ResetColor();
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }

            SendTransfer transfer = new SendTransfer()
            {
                ToUser = userIdTo,
                Amount = amount
            };
            Transfer returnedTransfer = transferDAO.SendTransfer(transfer);

            Console.WriteLine($"Transfer is completed confirmation number is: {returnedTransfer.TransferId}.");

            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult RequestTEBucks()
        {
            List<User> users = userDAO.GetAllUsers();
            Console.WriteLine("**********************");
            Console.WriteLine("       USERS");
            Console.WriteLine("**********************");

            foreach (User user in users)
            {
                Console.WriteLine($"ID: {user.UserId} Name: {user.Username}");
            }
            Console.Write("Enter ID of the user you are sending a transfer request to : ");
            string input = Console.ReadLine();
            int userRequested = int.Parse(input);
            Console.Write("Enter amount: $");
            string amountString = Console.ReadLine();
            decimal amount = decimal.Parse(amountString);

            Transfer request = new Transfer()
            {
                AccountTo = userRequested,
                AccountFrom = UserService.GetUserId(),
                Amount = amount
            };
            Transfer returnedTransfer = transferDAO.RequestTransfer(request);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"****************YOUR TRANSFER REQUEST IS PENDING****************");
            Console.ResetColor();
            
            return MenuOptionResult.WaitAfterMenuSelection;

        }

        private MenuOptionResult Logout()
        {
            UserService.SetLogin(new API_User()); //wipe out previous login info
            return MenuOptionResult.CloseMenuAfterSelection;
        }

    }
}
