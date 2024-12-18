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
        private static readonly ILog log = LogManager.GetLogger(typeof(FriendManagerDB));
        public static List<int> GetFriendsIdByPlayer(int userId)
        {
            List<int> friendsId = new List<int>();
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var friends = context.Friend
                        .Where(f => f.playerId1 == userId || f.playerId2 == userId)
                        .Select(f => f.playerId1 == userId ? f.playerId2 : f.playerId1)
                        .ToList();

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
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return friendsId;
        }
        public static Dictionary<int, string> GetFriendsGamertags(int playerId)
        {
            Dictionary<int, string> friendsGamertags = new Dictionary<int, string>();
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    friendsGamertags = context.Friend
                        .Where(f => f.playerId1 == playerId || f.playerId2 == playerId)
                        .Join(context.Player,
                              friend => friend.playerId1 == playerId ? friend.playerId2 : friend.playerId1,
                              player => player.userId,
                              (friend, player) => new { player.userId, player.accountId })
                        .Join(context.Account,
                              playerInfo => playerInfo.accountId,
                              account => account.accountId,
                              (playerInfo, account) => new { playerInfo.userId, account.gamertag })
                        .ToDictionary(entry => entry.userId, entry => entry.gamertag);
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
            return friendsGamertags;
        }
        public static int AddFriend(Friend friend)
        {
            int friendIdAdded = -1;
            BlockList blockList = new BlockList();
            blockList.idPlayer = friend.playerId1;
            blockList.blockedPlayer = friend.playerId2;
            if (!ArePlayersFriends(friend) || !BlockListManagerDB.IsPlayerBlocked(blockList))
            {
                try
                {
                    using (var context = new ExamExplotionDBEntities())
                    {
                        var newFriend = context.Friend.Add(friend);
                        context.SaveChanges();
                        if (newFriend != null)
                        {
                            friendIdAdded = newFriend.FriendId;
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
                catch (Exception exception)
                {
                    log.Error(exception);
                }
            }
            else
            {
                friendIdAdded = -2;
            }
            return friendIdAdded;
        }
        private static bool ArePlayersFriends(Friend friend)
        {
            bool areFriends = false;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    areFriends = context.Friend.Any(f =>
                        (f.playerId1 == friend.playerId1 && f.playerId2 == friend.playerId2) ||
                        (f.playerId1 == friend.playerId2 && f.playerId2 == friend.playerId1));
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
            return areFriends;
        }
        public static bool RemoveFriend(Friend friend)
        {
            bool removed = false;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var friendsToRemove = context.Friend.Where(f =>
                        (f.playerId1 == friend.playerId1 && f.playerId2 == friend.playerId2) ||
                        (f.playerId1 == friend.playerId2 && f.playerId2 == friend.playerId1)).ToList();

                    if (friendsToRemove.Any())
                    {
                        context.Friend.RemoveRange(friendsToRemove);
                        context.SaveChanges();
                        removed = true;
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
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return removed;
        }

    }
}
