using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class FriendManagerDB
    {
        public static List<int> GetFriendsIdByPlayer(int userId)
        {
            List<int> friendsId = new List<int>();
            using (var context = new ExamExplotionDBEntities())
            {
                var friends = context.Friend.Where(f => f.playerId1 == userId).Select(f => f.playerId2).ToList();
                friendsId = friends;
            }
            return friendsId;
        }
    }
}
