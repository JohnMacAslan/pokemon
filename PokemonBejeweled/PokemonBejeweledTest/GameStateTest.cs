using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PokemonBejeweled;


namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class GameStateTest
    {
        private GameState game;

        [Test()]
        public void TestGameInitialization()
        {
            game = new GameState();

            Assert.AreEqual(0, game.Score);
            Assert.AreEqual(120000, game.TimeLeft);
            Assert.IsNotNull(game);
        }

        [Test()]
        public void TestGetScore()
        {
            game = new GameState();
            Assert.AreEqual(0, game.Score);
        }

        [Test()]
        public void TestTimeLeftDefault()
        {
            game = new GameState();
            Assert.AreEqual(120000, game.TimeLeft);
        }

        [Test()]
        public void TestSetTimeLeft()
        {
            game = new GameState();
            game.TimeLeft = 2000;
            Assert.AreEqual(2000, game.TimeLeft);
        }
        [Test()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetTimeWhenNegativeValue()
        {
            game = new GameState();
            game.TimeLeft = -100;
        }

        [Test()]
        public void TestSetTimeThatExceptionNotThrownWhenNoTimeLimit()
        {
            game = new GameState();
            game.TimeLeft = -1; // When -1, no time limit for game
            Assert.AreEqual(-1, game.TimeLeft);
        }

        [Test()]
        public void TestThatNewGameResetsGameValues()
        {
            game = new GameState();

            game.TimeLeft = 1000;
            Assert.AreEqual(1000, game.TimeLeft);

            game.newGame();
            Assert.AreEqual(0, game.Score);
            Assert.AreEqual(120000, game.TimeLeft);
        }
    }
}
