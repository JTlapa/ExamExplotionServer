using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public static class GameManagerDB
    {
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
            return game;
        }

        public static bool DeleteGame(string invitationCode)
        {
            bool deleted = false;
            try
            {
                using(var context = new ExamExplotionDBEntities())
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
            return deleted;
        }
    }
}
