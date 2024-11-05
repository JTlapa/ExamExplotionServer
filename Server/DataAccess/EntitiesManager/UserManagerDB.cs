using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class UserManagerDB
    {
        public static int AddUser(Users user)
        {
            using (var context = new ExamExplotionDBEntities())
            {
                var newUser = context.Users.Add(user);
                context.SaveChanges();
                int idAccount;
                if (newUser != null)
                {
                    idAccount = newUser.userId;
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
