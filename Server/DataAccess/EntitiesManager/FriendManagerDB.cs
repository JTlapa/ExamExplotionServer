using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public static class FriendManagerDB
    {
        public static List<int> GetFriendsIdByPlayer(int userId)
        {
            List<int> friendsId = new List<int>();
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var friends = context.Friend.Where(f => f.playerId1 == userId).Select(f => f.playerId2).ToList();
                    friendsId = friends;
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
            return friendsId;
        }
    }
}
