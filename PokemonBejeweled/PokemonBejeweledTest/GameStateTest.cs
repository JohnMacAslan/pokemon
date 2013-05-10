using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PokemonBejeweled;
using Rhino.Mocks;


namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class GameStateTest
    {
        private GameState _gameState;
        private MockRepository _mocks;
        private PokemonBoard _mockBoard;

        [SetUp]
        public void setupMocks()
        {
            _mocks = new MockRepository();
            _mockBoard = _mocks.PartialMock<PokemonBoard>();
        }

        [SetUp]
        public void resetGameState()
        {
            _gameState = new GameState();
        }

        [Test()]
        public void TestGameInitialization()
        {
            Assert.AreEqual(0, _gameState.Score);
            Assert.AreEqual(120000, _gameState.TimeLeft);
            Assert.IsNotNull(_gameState);
        }

        [Test()]
        public void TestGetScore()
        {
            Assert.AreEqual(0, _gameState.Score);
        }

        [Test()]
        public void TestTimeLeftDefault()
        {
            Assert.AreEqual(120000, _gameState.TimeLeft);
        }

        [Test()]
        public void TestSetTimeLeft()
        {
            _gameState.TimeLeft = 2000;
            Assert.AreEqual(2000, _gameState.TimeLeft);
        }
        [Test()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetTimeWhenNegativeValue()
        {
            _gameState.TimeLeft = -100;
        }

        [Test()]
        public void TestSetTimeThatExceptionNotThrownWhenNoTimeLimit()
        {
            _gameState.TimeLeft = -1; // When -1, no time limit for game
            Assert.AreEqual(-1, _gameState.TimeLeft);
        }

        [Test()]
        public void TestThatNewGameResetsGameValues()
        {
            _gameState.TimeLeft = 1000;
            Assert.AreEqual(1000, _gameState.TimeLeft);

            _gameState.newGame();
            Assert.AreEqual(0, _gameState.Score);
            Assert.AreEqual(120000, _gameState.TimeLeft);
        }

        [Test]
        public void MakePlay_CanMakePlay_CallsBoardMakePlay()
        {
            _mockBoard.Expect(g => g.startPlay(0, 0, 0, 0));
            _mockBoard.Replay();
            _gameState.makePlay(0, 0);
            _gameState.Board = _mockBoard;
            _gameState.makePlay(0, 0);
            _mockBoard.VerifyAllExpectations();
        }
    }
}
