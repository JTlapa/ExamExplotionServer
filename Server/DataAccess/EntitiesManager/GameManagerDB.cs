using System;
using System.Collections.Generic;
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
    }
}
