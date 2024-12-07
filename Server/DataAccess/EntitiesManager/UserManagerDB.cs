using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.EntitiesManager
{
    public static class UserManagerDB
    {
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
                // Log de error SQL
                idAccount = -1;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Log de error de operación inválida
                idAccount = -1;
            }
            catch (EntityException entityException)
            {
                // Log de error de Entity Framework
                idAccount = -1;
            }
            catch (Exception ex)
            {
                // Log de cualquier otro error no especificado
                idAccount = -1;
            }

            return idAccount;
        }
    }
}
