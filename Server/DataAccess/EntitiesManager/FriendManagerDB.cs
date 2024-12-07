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
    public static class FriendManagerDB
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FriendManagerDB);
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
            return friendsId;
        }

    }
}
