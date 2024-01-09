using ITI_System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace myiti
{
    internal class Account: User
    {
       
        private static List<Account> accounts;

        public static List<Account> Accounts
        {
            get
            {
                if (accounts == null)
                {
                    LoadAccounts(@"C:\Users\Lenov\source\repos\myiti\Database\PendingAccounts.json");
                }
                return accounts;
            }
        }

        private static void LoadAccounts(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                string jsonData = File.ReadAllText(FilePath);
                accounts = JsonConvert.DeserializeObject<List<Account>>(jsonData);
            }
            else
            {
                accounts = new List<Account>();
            }
        }

        public static void SaveAccountsToFile(string FilePath)
        {
            string jsonData = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(FilePath, jsonData);
        }

        public bool CreateAccount(string name, string specialization, string email, string password, string role)
        {
            // Validation on input data of register 
            if(name !=  null && name.Contains(' ') && specialization != null && email != null && email.Contains('@') && email.Contains('.') && password.Length >= 8 && role != null)
            {
                Random rnd = new Random();
                Account newAccount = new Account
                {
                    Id = rnd.Next(100, 1000),
                    Name = name,
                    Specialization = specialization,
                    Email = email,
                    Password = password,
                    Role = role,
                    Approvement = false
                };

                Accounts.Add(newAccount);
                SaveAccountsToFile(@"C:\Users\Lenov\source\repos\myiti\Database\PendingAccounts.json");
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
