using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PokemonBejeweled;
using PokemonBejeweled.GameState;

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
            Assert.IsNotNull(game);
        }
    }
}
