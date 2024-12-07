using DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public static class AccountManagerDB
    {
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
                }
            }
            catch (SqlException sqlException)
            {
                // Log de error SQL
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Log de error de operación inválida
            }
            catch (EntityException entityException)
            {
                // Log de error de Entity Framework
            }
            catch (Exception ex)
            {
                // Log de cualquier otro error no especificado
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
                // Log de error SQL
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Log de error de operación inválida
            }
            catch (EntityException entityException)
            {
                // Log de error de Entity Framework
            }
            catch (Exception ex)
            {
                // Log de cualquier otro error no especificado
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
                // Log de error SQL
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Log de error de operación inválida
            }
            catch (EntityException entityException)
            {
                // Log de error de Entity Framework
            }
            catch (Exception ex)
            {
                // Log de cualquier otro error no especificado
            }
            return passwordUpdated;
        }

        public static bool VerifyExistingGamertag(string gamertag)
        {
            bool gamertagExists = false;
            using (var context = new ExamExplotionDBEntities())
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
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var account = context.Account.FirstOrDefault(a => a.email == email);
                    if (account != null)
                    {
                        emailExists = true;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                // Log de error SQL
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Log de error de operación inválida
            }
            catch (EntityException entityException)
            {
                // Log de error de Entity Framework
            }
            catch (Exception ex)
            {
                // Log de cualquier otro error no especificado
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
                // Log de error SQL
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Log de error de operación inválida
            }
            catch (EntityException entityException)
            {
                // Log de error de Entity Framework
            }
            catch (Exception ex)
            {
                // Log de cualquier otro error no especificado
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
            catch (SqlException sqlException)
            {
                // Log de error SQL
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Log de error de operación inválida
            }
            catch (EntityException entityException)
            {
                // Log de error de Entity Framework
            }
            catch (Exception ex)
            {
                // Log de cualquier otro error no especificado
            }
            return accountDeactivated;
        }
    }
}

