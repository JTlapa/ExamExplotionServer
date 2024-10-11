using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class AccountManagerDB
    {
        public static bool ValidateAccount(Account account)
        {
            using (var context = new ExamExplotionDBEntities())
            {
                var accountVerifed = context.Account.FirstOrDefault(u => u.gamertag == account.gamertag && u.password == account.password);

                if (accountVerifed != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}

