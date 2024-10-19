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
    }
}
