using System;
using NUnit.Framework;
using PokemonBejeweled;
using PokemonBejeweled.Pokemon;
using Rhino.Mocks;

namespace PokemonBejeweledTest
{
    [TestFixture]
    public class GameStateTest
    {
        private GameState _gameState;
        private MockRepository _mocks;
        private PokemonBoard _pokemonBoard;

        [SetUp]
        public void setUp()
        {
            _mocks = new MockRepository();
            _pokemonBoard = _mocks.PartialMock<PokemonBoard>();
            _gameState = new GameState();
        }

        private IBasicPokemonToken[,] generateStableGrid()
        {
            int tokenToAdd = 0;
            IBasicPokemonToken[,] pokemonGrid = new IBasicPokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
            for (int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    pokemonGrid[row, col] = (PokemonToken)Activator.CreateInstance(PokemonBoard.TokenList[tokenToAdd++ % 6 + 1]);
                }
            }
            return pokemonGrid;
        }

        [Test]
        public void GameState_CurrentScoreSetTo0()
        {
            Assert.AreEqual(0, _gameState.CurrentScore);
        }

        [Test]
        public void GameState_PreviousScoreSetTo0()
        {
            Assert.AreEqual(0, _gameState.CurrentScore);
        }

        [Test]
        public void GameState_DefaultNoTimeLimit()
        {
            Assert.AreEqual(GameState.NO_TIME_LIMIT, _gameState.TimeLeft);
        }

        [Test]
        public void MakePlay_CanMakePlay_CallsBoardMakePlay()
        {
            _pokemonBoard.Expect(g => g.tryPlay(_gameState.CurrentGrid, 0, 0, 0, 0));
            _pokemonBoard.Replay();
            _gameState.tryPlay(0, 0);
            _gameState.Board = _pokemonBoard;
            _gameState.tryPlay(0, 0);
            _pokemonBoard.VerifyAllExpectations();
        }

        [Test]
        public void UndoPlay_CantUndoPlay_BoardUnchanged()
        {
            _gameState.undoPlay(this, null);
            Assert.AreEqual(_gameState.PreviousGrid, _gameState.CurrentGrid);   
        }

        [Test]
        public void UndoPlay_CanUndoPlay_BoardReverted()
        {
            _gameState.CurrentGrid = generateStableGrid();
            _gameState.PreviousGrid = generateStableGrid();
            _gameState.undoPlay(this, null);
            Assert.AreEqual(_gameState.PreviousGrid, _gameState.CurrentGrid);            
        }

        [Test]
        public void UndoPlay_CanUndoPlay_ScoreReverted()
        {
            _gameState.CurrentScore = 100;
            _gameState.PreviousScore = 50;
            _gameState.undoPlay(this, null);

        }

        [Test]
        public void PokemonBoardPointsAdded_BoardAddsPoint_ScoreIncremented()
        {
            _gameState.CurrentScore = 100;
            _gameState.CurrentGrid = generateStableGrid();
            _gameState.CurrentGrid[0, 0] = new PichuToken();
            _gameState.CurrentGrid[0, 1] = new TotodileToken();
            _gameState.CurrentGrid[0, 2] = new TotodileToken();
            _gameState.CurrentGrid[1, 0] = new TotodileToken();
            _gameState.tryPlay(0, 0);
            _gameState.tryPlay(1, 0);
            Assert.AreEqual(130, _gameState.CurrentScore);
        }

        [Test]
        public void PokemonBoardEndingPlay_BoardEndsPlay_BoardChangedEvoked()
        {
            bool boardChangedEvoked = false;
            _gameState.BoardChanged += delegate { boardChangedEvoked = true; };
            _gameState.CurrentScore = 100;
            _gameState.CurrentGrid = generateStableGrid();
            _gameState.CurrentGrid[0, 0] = new PichuToken();
            _gameState.CurrentGrid[0, 1] = new TotodileToken();
            _gameState.CurrentGrid[0, 2] = new TotodileToken();
            _gameState.CurrentGrid[1, 0] = new TotodileToken();
            _gameState.PreviousGrid = _gameState.CurrentGrid;
            _gameState.tryPlay(0, 0);
            _gameState.tryPlay(1, 0);
            Assert.IsTrue(boardChangedEvoked);
            Assert.AreNotEqual(_gameState.PreviousGrid, _gameState.CurrentGrid);
        }
    }
}
