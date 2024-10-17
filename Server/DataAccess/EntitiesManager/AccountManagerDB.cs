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
            using (var context = new ExamExplotionDB())
            {
                var accountVerifed = context.Account.FirstOrDefault(u => u.gamertag == account.gamertag && u.password == account.password);
                int idAccount;
                if (accountVerifed != null)
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
    }
}

