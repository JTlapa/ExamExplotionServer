using DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    Guest newGuest = context.Guest.Add(guest);
                    context.SaveChanges();
                    if(newGuest != null)
                    {
                        guestNumber = newGuest.guestNumber;
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
            return guestNumber;
        }
    }
}
