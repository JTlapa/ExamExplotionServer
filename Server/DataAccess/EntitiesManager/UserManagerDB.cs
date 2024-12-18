using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.EntitiesManager
{
    public static class UserManagerDB
    { 
        private static readonly ILog log = LogManager.GetLogger(typeof(UserManagerDB));
        /// <summary>
        /// Agrega un usuario a la base de datos y retorna su identificador.
        /// </summary>
        /// <param name="user">El objeto <see cref="Users"/> a agregar.</param>
        /// <returns>El ID del usuario si fue agregado exitosamente; -1 si ocurrió un error.</returns>
        public static int AddUser(Users user)
        {
            int idAccount = -1;

            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var newUser = context.Users.Add(user);
                    context.SaveChanges();

                    idAccount = newUser != null ? newUser.userId : -1;
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
    }
}
