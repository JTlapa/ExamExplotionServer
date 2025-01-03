using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess.EntitiesManager
{
    public static class GameManagerDB
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GameManagerDB));

        public static bool AddGame(Game game)
        {
            bool gameAdded = false;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var gameRegistered = context.Game.Add(game);
                    context.SaveChanges();
                    if (gameRegistered != null)
                    {
                        gameAdded = true;
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
            return gameAdded;
        }

        public static Game getGameByCode(string code)
        {
            Game game = null;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    game = context.Game.FirstOrDefault(g => g.invitationCode == code);
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
            return game;
        }

        public static bool DeleteGame(string invitationCode)
        {
            bool deleted = false;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var game = context.Game.FirstOrDefault(g => g.invitationCode == invitationCode);
                    if (game != null)
                    {
                        context.Game.Remove(game);
                        context.SaveChanges();
                        deleted = true;
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
            return deleted;
        }
        public static void UpdateWinner(string gameCode, int userId)
        {
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var game = context.Game.FirstOrDefault(g => g.invitationCode == gameCode);

                    if (game != null)
                    {
                        game.winnerPlayerId = userId;
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
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }

        public static int GetGameId(string gameCode)
        {
            int gameId = -1;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var game = context.Game.FirstOrDefault(g => g.invitationCode == gameCode);
                    if (game != null)
                    {
                        gameId = game.gameId;
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
            return gameId;
        }
    }

}
