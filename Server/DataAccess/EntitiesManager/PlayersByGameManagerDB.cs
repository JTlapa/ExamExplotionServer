using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class PlayersByGameManagerDB
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GameManagerDB));

        public static bool AddPlayerToGame(int idGame, int idPlayer)
        {
            bool playerAdded = false;

            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var playersByGame = new PlayersByGame
                    {
                        gameId = idGame,
                        playerId = idPlayer,
                        tipo = "A"
                    };

                    var addedEntry = context.PlayersByGame.Add(playersByGame);

                    context.SaveChanges();

                    if (addedEntry != null)
                    {
                        playerAdded = true;
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

            return playerAdded;
        }

        public static List<int> GetGamePlayersId(int idGame)
        {
            List<int> playerIds = new List<int>();

            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    playerIds = context.PlayersByGame
                                       .Where(p => p.gameId == idGame)
                                       .Select(p => p.playerId)
                                       .ToList();
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

            return playerIds;
        }
        public static List<int> GetAccountsIdByPlayerId(List<int> playersId)
        {
            List<int> accountsId = new List<int>();

            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    foreach(int playerId in playersId)
                    {
                        var accountId = context.Player
                                        .Where(p => p.userId == playerId)
                                        .Select(p => p.accountId)
                                        .FirstOrDefault();
                        if(accountId > 0)
                        {
                            accountsId.Add((int)accountId);
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
            return accountsId;
        }

    }
}
