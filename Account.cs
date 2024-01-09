using ITI_System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace myiti
{
    internal class Account: User
    {
        private List<Account> Accounts { get; set; }

            public bool CreateAccount(string name, string specialization, string email, string password, string role)
        {
            if (Accounts == null || Accounts.Count == 0)
            {
                Accounts = LoadData<Account>("PendingAccounts.json");
            }
            // Validation on input data of register 
            if (name !=  null && name.Contains(' ') && specialization != null && email != null && email.Contains('@') && email.Contains('.') && password.Length >= 8 && role != null)
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
                SaveDataToJson("PendingAccounts.json", Accounts,"Creat","Account");
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
