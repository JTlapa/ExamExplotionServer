using DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DataAccess.EntitiesManager
{
    public static class AccountManagerDB
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountManagerDB));
        public static int ValidateAccount(Account account)
        {
            string hashedPassword = PasswordEncryptor.HashPassword(account.password);
            account.password = hashedPassword;
            int idAccount = -1;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var accountVerifed = context.Account.FirstOrDefault(a => a.gamertag == account.gamertag && a.password == account.password);
                    if (accountVerifed != null && accountVerifed.status == "Active")
                    {
                        idAccount = accountVerifed.accountId;
                    }
                    else if(accountVerifed != null)
                    {
                        idAccount = -2;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
                idAccount = -3;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
                idAccount = -3;
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
                idAccount = -3;
            }
            catch (Exception exception)
            {
                log.Error(exception);
                idAccount = -3;
            }
            return idAccount;
        }

        public static int AddAcount(Account account)
        {
            int idAccount = -1;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    account.status = "Active";
                    string hashedPassword = PasswordEncryptor.HashPassword(account.password);
                    account.password = hashedPassword;

                    var newAccount = context.Account.Add(account);
                    context.SaveChanges();
                    if (newAccount != null)
                    {
                        idAccount = newAccount.accountId;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return idAccount;
        }
        public static bool UpdatePassword(Account account)
        {
            bool passwordUpdated = false;
            try
            {
                using (var context = new ExamExplotionDBEntities())
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
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return passwordUpdated;
        }

        public static int VerifyExistingGamertag(string gamertag)
        {
            int gamertagExists = 0;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var account = context.Account.FirstOrDefault(a => a.gamertag == gamertag);
                    if (account != null)
                    {
                        gamertagExists = 1;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
                gamertagExists = 2;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
                gamertagExists = 2;
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
                gamertagExists = 2;
            }
            catch(Exception exception)
            {
                log.Error(exception);
                gamertagExists = 2;
            }
            return gamertagExists;
        }
        public static int VerifyExistingEmail(string email)
        {
            int emailExists = 0;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var account = context.Account.FirstOrDefault(a => a.email == email);
                    if (account != null)
                    {
                        emailExists = 1;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
                emailExists = 2;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
                emailExists = 2;
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
                emailExists = 2;
            }
            catch (Exception exception)
            {
                log.Error(exception);
                emailExists = 2;
            }
            return emailExists;
        }

        public static int GetAccountIdByGamertag(string gamertag)
        {
            int accountId = -1;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    Account account = context.Account.FirstOrDefault(a => a.gamertag == gamertag);
                    if (account != null)
                    {
                        accountId = account.accountId;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return accountId;
        }

        public static bool DeactivateAccount(string gamertag)
        {
            bool accountDeactivated = false;
            try
            {
                using(var context = new ExamExplotionDBEntities())
                {
                    Account account = context.Account.FirstOrDefault(a => a.gamertag == gamertag);
                    if (account != null)
                    {
                        account.status = "Inactive";
                        accountDeactivated = true;
                    }
                }
            }
            catch(SqlException sqlException)
            { 
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
             }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return accountDeactivated;
        }

        public static string GetAccountGamertagById(int accountId)
        {
            string gamertag = string.Empty;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    Account account = context.Account.FirstOrDefault(a => a.accountId == accountId);
                    if (account != null)
                    {
                        gamertag = account.gamertag;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return gamertag;
        }
    }
}

