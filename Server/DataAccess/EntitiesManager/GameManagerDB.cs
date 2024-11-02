using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class GameManagerDB
    {
        public static bool AddGame(Game game)
        {
            using (var context = new ExamExplotionDB())
            {
                var gameRegistered = context.Game.Add(game);
                context.SaveChanges();
                if (gameRegistered != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static Game getGameByCode(string code)
        {
            Game game = null;
            using (var context = new ExamExplotionDB())
            {
                game = context.Game.FirstOrDefault(g => g.invitationCode == code);
            }
            return game;
        }

        public static bool DeleteGame(string invitationCode)
        {
            using(var context = new ExamExplotionDB())
            {
                bool deleted = false;
                var game = context.Game.FirstOrDefault(g => g.invitationCode == invitationCode);
                if (game != null)
                {
                    context.Game.Remove(game);
                    context.SaveChanges();
                    deleted = true;
                }
                return deleted;
            }
        }
    }
}
