using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class PlayerManagerDB
    {
        public static bool RegisterPlayer(Player player)
        {
            using (var context = new ExamExplotionDBEntities())
            {
                var playerRegistered = context.Player.Add(player);
                context.SaveChanges();
                if (playerRegistered != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool UpdateScore(int userId, int newScore)
        {
            using (var context = new ExamExplotionDBEntities())
            {
                var player = context.Player.FirstOrDefault(p => p.userId == userId);

                if (player == null)
                {
                    return false;
                }

                player.score += newScore;
                context.SaveChanges();
            }
            return true;
        }

        /*
        public static int AddFriend(int playerId, int friendId)
        {
            using (var context = new ExamExplotionDBEntities())
            {
                var player1 = context.Player.FirstOrDefault(p => p.userId == playerId);
                var player2 = context.Player.FirstOrDefault(p => playerId == friendId);

                if (player1 != null || player2 != null)
                {
                    bool alreadyFriends = context.Friend.Any(f =>
                        (f.playerId1 == playerId && f.playerId2 == friendId) ||
                        (f.playerId1 == friendId && f.playerId2 == playerId));
                    if (alreadyFriends)
                    {
                        return 2; // They're already friends 
                    }

                    context.Friend.Add(new Friend)
                }
            }
        } */

        public static int GetWins(int playerId)
        {
            using(var context = new ExamExplotionDBEntities())
            {
                var player = context.Player.FirstOrDefault(p => p.userId == playerId);
                return player != null ? player.wins.GetValueOrDefault(0) : -1; /* Returns -1 if player doesnt exists */
            }
        }

        public static int GetPoints(int playerId)
        {
            using (var context = new ExamExplotionDBEntities())
            {
                var player = context.Player.FirstOrDefault(p => p.userId == playerId);
                return player != null ? player.score.GetValueOrDefault(0) : -1; /* Returns -1 if player doesnt exists */
            }
        }

        public static Player GetPlayerByGamertag(string gamertag)
        {
            int accountId = AccountManagerDB.GetAccountIdByGamertag(gamertag);
            Player player = null;
            using (var context = new ExamExplotionDBEntities())
            {
                player = context.Player.FirstOrDefault(p => p.accountId == accountId);
            }
            return player;

        }

        public static Dictionary<string, int> GetGlobalLeaderboard()
        {
            Dictionary<string, int> globalLeaderboard= new Dictionary<string, int>();
            using (var context = new ExamExplotionDBEntities())
            {
                var topPlayers = context.Player.Join(context.Account, player => player.accountId, account => account.accountId, 
                                         (player, account) => new {Gamertag = account.gamertag, Wins = player.wins})
                                         .OrderByDescending(p => p.Wins).Take(5).ToList();

                foreach (var player in topPlayers)
                {
                    globalLeaderboard.Add(player.Gamertag, (int)player.Wins);
                }
            }
            return globalLeaderboard;
        }

        public static Dictionary<string, int> GetLeaderboardByFriends(List<int> friendsId)
        {
            Dictionary<string, int> friendsLeaderboard = new Dictionary<string, int>();
            using (var context = new ExamExplotionDBEntities())
            {
                var leaderboard = context.Player
                    .Join(context.Account,
                          player => player.accountId,
                          account => account.accountId,
                          (player, account) => new
                          {
                              Gamertag = account.gamertag,
                              Wins = player.wins,
                              UserId = player.userId
                          })
                    .Where(p => friendsId.Contains(p.UserId))
                    .OrderByDescending(p => p.Wins)
                    .ToList();
                foreach (var player in leaderboard)
                {
                    if (!friendsLeaderboard.ContainsKey(player.Gamertag))
                    {
                        friendsLeaderboard.Add(player.Gamertag, (int)player.Wins);
                    }
                }
            }
            return friendsLeaderboard;
        }

    }
}
