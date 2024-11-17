using DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class GuestManagerDB
    {
        public static int AddGuest(Guest guest)
        {
            int guestNumber = -1;
            using (var context = new ExamExplotionDBEntities())
            {
                Guest newGuest = context.Guest.Add(guest);
                context.SaveChanges();
                if(newGuest != null)
                {
                    guestNumber = newGuest.guestNumber;
                }
            }
            return guestNumber;
        }
    }
}
