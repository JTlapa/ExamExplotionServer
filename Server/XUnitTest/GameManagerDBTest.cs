using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DataAccess;
using DataAccess.EntitiesManager;

namespace XUnitTest
{
    public class GameManagerDBTest
    {
        [Fact]
        public void AddGameSuccessTest()
        {
            var game = new Game()
            {
                numberPlayers = 4,
                hostPlayerId = 1,
                timePerTurn = 60,
                lives = 3,
            };

            bool result = GameManagerDB.AddGame(game);
            Assert.True(result, "El juego fue insertado con exito!");
        }

        [Fact]
        public void GetGameByCodeSuccessTest()
        {
            var testGame = new Game
            {
                invitationCode = "Z19D",
                numberPlayers = 4,
                hostPlayerId = 1,
                timePerTurn = 60,
                lives = 3
            };

            using (var context = new ExamExplotionDB())
            {
                context.Game.Add(testGame);
                context.SaveChanges();
            }

            var result = GameManagerDB.getGameByCode("Z19D");

            Assert.NotNull(result);
            Assert.Equal("Z19D", result.invitationCode);
        }
    }
}
