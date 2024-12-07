using DataAccess.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public static class GuestManagerDB
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GuestManagerDB));

        public static int AddGuest(Guest guest)
        {
            int guestNumber = -1;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    Guest newGuest = context.Guest.Add(guest);
                    context.SaveChanges();
                    if (newGuest != null)
                    {
                        guestNumber = newGuest.guestNumber;
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
            return guestNumber;
        }
    }

}
