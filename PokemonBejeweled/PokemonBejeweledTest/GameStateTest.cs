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

            Assert.AreEqual(0, game.getScore());
            Assert.AreEqual(120000, game.getTime());
            Assert.IsNotNull(game);
        }

        [Test()]
        public void TestGetScore()
        {
            game = new GameState();
            Assert.AreEqual(0, game.getScore());
        }

        [Test()]
        public void TestSetScore()
        {
            game = new GameState();
            game.setScore(24);
            Assert.AreEqual(24, game.getScore());
        }

        [Test()]
        public void TestAddToScoreWithPositiveValue()
        {
            game = new GameState();
            game.addToScore(50);
            Assert.AreEqual(50, game.getScore());

            game.addToScore(25);
            Assert.AreEqual(75, game.getScore());
        }

        [Test()]
        public void TestAddToScoreWithZeroValue()
        {
            game = new GameState();
            game.addToScore(0);
            Assert.AreEqual(0, game.getScore());
        }

        [Test()]
        public void TestSetScoreAfterAddingToScore()
        {
            game = new GameState();
            game.addToScore(100);
            game.setScore(50);
            Assert.AreEqual(50, game.getScore());
        }

        [Test()]
        public void TestAddingToScoreAfterSettingScore()
        {
            game = new GameState();
            game.setScore(50);
            game.addToScore(50);
            Assert.AreEqual(100, game.getScore());
        }

        [Test()]
        public void TestGetTime()
        {
            game = new GameState();
            Assert.AreEqual(120000, game.getTime());
        }

        [Test()]
        public void TestSetTime()
        {
            game = new GameState();
            game.setTime(2000);
            Assert.AreEqual(2000, game.getTime());
        }
        [Test()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetTimeWhenNegativeValue()
        {
            game = new GameState();
            game.setTime(-100);
        }

        [Test()]
        public void TestSetTimeThatExceptionNotThrownWhenNoTimeLimit()
        {
            game = new GameState();
            game.setTime(-1); // When -1, no time limit for game
            Assert.AreEqual(-1, game.getTime());
        }

        [Test()]
        public void TestThatNewGameResetsGameValues()
        {
            game = new GameState();

            game.setScore(500);
            game.setTime(1000);
            Assert.AreEqual(500, game.getScore());
            Assert.AreEqual(1000, game.getTime());

            game.newGame();
            Assert.AreEqual(0, game.getScore());
            Assert.AreEqual(120000, game.getTime());
        }
    }
}
