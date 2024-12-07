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
                    }

                    player.score += newScore;
                    context.SaveChanges();
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
            return scoreUpdated;
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
            return friendsLeaderboard;
        }


    }
}
