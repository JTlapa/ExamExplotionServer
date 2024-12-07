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
    public static class PlayerManagerDB
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PlayerManagerDB));

        public static bool RegisterPlayer(Player player)
        {
            bool registered = false;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var playerRegistered = context.Player.Add(player);
                    context.SaveChanges();
                    if (playerRegistered != null)
                    {
                        registered = true;
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
            return registered;
        }

        public static bool UpdateScore(int userId, int newScore)
        {
            bool scoreUpdated = false;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var player = context.Player.FirstOrDefault(p => p.userId == userId);

                    if (player != null)
                    {
                        scoreUpdated = true;
                        player.score += newScore;
                        context.SaveChanges();
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
            return scoreUpdated;
        }

        public static int GetWins(int playerId)
        {
            int wins = -1; // Valor por defecto en caso de error o si el jugador no existe
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var player = context.Player.FirstOrDefault(p => p.userId == playerId);
                    if (player != null)
                    {
                        wins = player.wins.GetValueOrDefault(0);
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
            return wins;
        }

        public static int GetPoints(int playerId)
        {
            int points = -1; // Valor por defecto en caso de error o si el jugador no existe
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var player = context.Player.FirstOrDefault(p => p.userId == playerId);
                    if (player != null)
                    {
                        points = player.score.GetValueOrDefault(0);
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
            return points;
        }

        public static Player GetPlayerByGamertag(string gamertag)
        {
            Player player = null; // Valor por defecto en caso de error o si el jugador no existe
            try
            {
                int accountId = AccountManagerDB.GetAccountIdByGamertag(gamertag);
                using (var context = new ExamExplotionDBEntities())
                {
                    player = context.Player.FirstOrDefault(p => p.accountId == accountId);
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
            return player;
        }

        public static Dictionary<string, int> GetGlobalLeaderboard()
        {
            Dictionary<string, int> globalLeaderboard = new Dictionary<string, int>(); // Inicialización por defecto
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var topPlayers = context.Player
                        .Join(context.Account,
                              player => player.accountId,
                              account => account.accountId,
                              (player, account) => new { Gamertag = account.gamertag, Wins = player.wins })
                        .OrderByDescending(p => p.Wins)
                        .Take(5)
                        .ToList();

                    foreach (var player in topPlayers)
                    {
                        globalLeaderboard.Add(player.Gamertag, (int)player.Wins);
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
            return globalLeaderboard;
        }

        public static Dictionary<string, int> GetLeaderboardByFriends(List<int> friendsId)
        {
            Dictionary<string, int> friendsLeaderboard = new Dictionary<string, int>(); // Inicialización por defecto
            try
            {
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
            return friendsLeaderboard;
        }
    }

}
