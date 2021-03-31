﻿using MenuFramework;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.DAL;
using TenmoClient.Data;

namespace TenmoClient.Views
{

    public class LoginRegisterMenu : ConsoleMenu
    {
        private readonly AuthService authService;
        private string apiUrl = "https://localhost:44315/";

        public LoginRegisterMenu(AuthService authService)
        {
            this.authService = authService;

            AddOption("Login", Login)
                .AddOption("Register", Register)
                .AddOption("Exit", Exit);
            this.Configure(config => {
                config.SelectedItemForegroundColor = ConsoleColor.Cyan;
                config.ItemForegroundColor = ConsoleColor.DarkCyan;

            });
        }

        private MenuOptionResult Login()
        {
            API_User user = null;
            while (user == null)
            {
                LoginUser loginUser = new LoginUser();
                loginUser.Username = GetString("Username: ", true);
                if (loginUser.Username.Trim().Length == 0)
                {
                    Console.WriteLine("Login cancelled.");
                    return MenuOptionResult.WaitAfterMenuSelection;
                }

                loginUser.Password = GetPasswordFromConsole("Password: ");
                user = authService.Login(loginUser);
                if (user == null)
                {
                    Console.WriteLine("Username or password is not valid.");
                }
            }
            UserService.SetLogin(user);
            ApiDAO.client = new RestSharp.RestClient(apiUrl);
            ApiDAO.client.Authenticator = new JwtAuthenticator(user.Token);
            AccountApiDAO accountDao = new AccountApiDAO(apiUrl);
            TransferApiDAO transferDAO = new TransferApiDAO(apiUrl);
            UserApiDAO userApiDAO = new UserApiDAO(apiUrl);
            // User is logged in, show the main menu now.
            return new MainMenu(accountDao, transferDAO, userApiDAO).Show();
        }

        private MenuOptionResult Register()
        {
            bool isRegistered = false;
            while (!isRegistered)
            {
                LoginUser registerUser = new LoginUser();
                registerUser.Username = GetString("Username: ", true);
                if (registerUser.Username.Trim().Length == 0)
                {
                    Console.WriteLine("Registration cancelled.");
                    return MenuOptionResult.WaitAfterMenuSelection;
                }
                registerUser.Password = GetPasswordFromConsole("Password: ");
                isRegistered = authService.Register(registerUser);
                if (!isRegistered)
                {
                    Console.WriteLine("Registration failed.");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        protected override void OnBeforeShow()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("**************************");
            Console.WriteLine("*****Welcome to TEnmo!****");
            Console.WriteLine("**************************");
        }

        #region Console Helper Functions
        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return pass;
        }
        #endregion
    }
}
