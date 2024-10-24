﻿using DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class AccountManagerDB
    {
        public static int ValidateAccount(Account account)
        {
            string hashedPassword = PasswordEncryptor.HashPassword(account.password);
            account.password = hashedPassword;
            using (var context = new ExamExplotionDB())
            {
                var accountVerifed = context.Account.FirstOrDefault(a => a.gamertag == account.gamertag && a.password == account.password);
                int idAccount;
                if (accountVerifed != null && accountVerifed.status == "Active")
                {
                    idAccount = accountVerifed.accountId;
                }
                else
                {
                    idAccount = -1;
                }
                return idAccount;
            }
        }

        public static int AddAcount(Account account)
        {
            using (var context = new ExamExplotionDB())
            {
                account.status = "Active";
                string hashedPassword = PasswordEncryptor.HashPassword(account.password);
                account.password = hashedPassword;

                var newAccount = context.Account.Add(account);
                context.SaveChanges();
                int idAccount;
                if (newAccount != null)
                {
                    idAccount = newAccount.accountId;
                }
                else
                {
                    idAccount = -1;
                }
                return idAccount;
            }
        }
        public static bool UpdatePassword(Account account)
        {
            bool passwordUpdated = false;
            using (var context = new ExamExplotionDB())
            {
                string hashedPassword = PasswordEncryptor.HashPassword(account.password);

                var accountToUpdate = context.Account.FirstOrDefault(a => a.gamertag == account.gamertag);
                if (accountToUpdate != null)
                {
                    accountToUpdate.password = hashedPassword;
                    context.SaveChanges();
                    passwordUpdated = true;
                }
            }
            return passwordUpdated;
        }

        public static bool VerifyExistingGamertag(string gamertag)
        {
            bool gamertagExists = false;
            using (var context = new ExamExplotionDB())
            {
                var account = context.Account.FirstOrDefault(a => a.gamertag == gamertag);
                if(account != null)
                {
                    gamertagExists = true;
                }
            }
            return gamertagExists;
        }
        public static bool VerifyExistingEmail(string email)
        {
            bool emailExists = false;
            using (var context = new ExamExplotionDB())
            {
                var account = context.Account.FirstOrDefault(a => a.email == email);
                if (account != null)
                {
                    emailExists = true;
                }
            }
            return emailExists;
        }
    }
}

