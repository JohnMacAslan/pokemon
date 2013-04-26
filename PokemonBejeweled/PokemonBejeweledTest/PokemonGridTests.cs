using System;
using PokemonBejeweled.Pokemon;
using PokemonBejeweled;
using NUnit.Framework;
using Rhino.Mocks;

namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class PokemonGridTests
    {
        private PokemonToken[,] _pokemon = new PokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
        private PokemonBoard _pokemonGrid;
        private MockRepository _mocks;
        private PokemonBoard _mockGrid;

        [SetUp]
        public void setupMocks()
        {
            _mocks = new MockRepository();
            _mockGrid = _mocks.PartialMock<PokemonBoard>();
        }

        [SetUp]
        public void resetPokemonGrid()
        {
            _pokemonGrid = new PokemonBoard();
            for (int i = 0; i < 8; i+=2) {
                for (int j = 0; j < 8; j += 2)
                {
                    _pokemon[i, j] = new BulbasaurToken();
                    _pokemon[i + 1, j] = new CharmanderToken();
                    _pokemon[i + 1, j + 1] = new BulbasaurToken();
                    _pokemon[i, j + 1] = new CharmanderToken();
                }
            }
            _pokemonGrid.PokemonGrid = _pokemon;
            _pokemonGrid.NewPokemonGrid = _pokemon;
        }

        [Test]
        public void InvertGrid_NoError_GridInverted()
        {
            IBasicPokemonToken[,] _pokemonToInvert = new IBasicPokemonToken[3, 3];
            _pokemonToInvert[0, 0] = new BulbasaurToken();
            _pokemonToInvert[0, 1] = new BulbasaurToken();
            _pokemonToInvert[0, 2] = new BulbasaurToken();
            _pokemonToInvert[1, 0] = new CharmanderToken();
            _pokemonToInvert[1, 1] = new CharmanderToken();
            _pokemonToInvert[1, 2] = new CharmanderToken();
            _pokemonToInvert[2, 0] = new BulbasaurToken();
            _pokemonToInvert[2, 1] = new BulbasaurToken();
            _pokemonToInvert[2, 2] = new BulbasaurToken();
            IBasicPokemonToken[,] _invertedPokemon = PokemonBoard.invertPokemon(_pokemonToInvert);
            _pokemonToInvert[0, 0] = new BulbasaurToken();
            _pokemonToInvert[0, 1] = new CharmanderToken();
            _pokemonToInvert[0, 2] = new BulbasaurToken();
            _pokemonToInvert[1, 0] = new BulbasaurToken();
            _pokemonToInvert[1, 1] = new CharmanderToken();
            _pokemonToInvert[1, 2] = new BulbasaurToken();
            _pokemonToInvert[2, 0] = new BulbasaurToken();
            _pokemonToInvert[2, 1] = new CharmanderToken();
            _pokemonToInvert[2, 2] = new BulbasaurToken();
            Assert.AreEqual(_pokemonToInvert, _invertedPokemon);
        }

        [Test]
        public void PokemonGrid_NoError_GridInitializedToIBasicPokemon()
        {
            _pokemonGrid = new PokemonBoard();
            for (int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    Assert.IsInstanceOf(typeof(IBasicPokemonToken), _pokemon[row, col]);
                }
            }
        }

        [Test]
        public void PokemonGrid_NoError_PokemonAndNewPokemonAreEqual()
        {
            _pokemonGrid = new PokemonBoard();
            Assert.AreEqual(_pokemonGrid.PokemonGrid, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void CopyGrid_ValidGrid_GridCopiedCorrectly()
        {
            PokemonToken[,] newPokemon = new PokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
            PokemonBoard.copyGrid(_pokemon, newPokemon);
            Assert.AreEqual(_pokemon, newPokemon);
        }

        [Test]
        public void UpdateBoard_NoRowsOrColumnsOfThree_GridUnchanged()
        {
            _pokemonGrid.makePlay(0, 0, 0, 0);
            Assert.AreEqual(_pokemon, _pokemonGrid.PokemonGrid);
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreHorizontallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(_pokemonGrid.piecesAreAdjacent(1, 1, 1, 2));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreVerticallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(_pokemonGrid.piecesAreAdjacent(1, 2, 1, 1));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreNotAdjacent_ReturnFalse()
        {
            Assert.IsFalse(_pokemonGrid.piecesAreAdjacent(1, 1, 2, 2));
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensLessThan3_PokemonGridUnchanged()
        {
            _pokemonGrid.markNullRow(0, 0, 0);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensGreaterThan3_RowMarkNull()
        {
            _pokemon[0, 0] = null;
            _pokemon[0, 1] = null;
            _pokemon[0, 2] = null;
            _pokemon[0, 3] = null;
            _pokemonGrid.markNullRow(0, 0, 4);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkNullRow_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonGrid.markNullRow(-1, 0, 4);
        }

        [Test]
        public void MarkSurroundingTokensNull_IndicesWithinRange_SurroundingTokensMarkedNull()
        {
            int row = 4;
            int col = 4;
            _pokemon[row - 1, col] = null;
            _pokemon[row - 1, col - 1] = null;
            _pokemon[row - 1, col + 1] = null;
            _pokemon[row, col - 1] = null;
            _pokemon[row, col + 1] = null;
            _pokemon[row + 1, col] = null;
            _pokemon[row + 1, col - 1] = null;
            _pokemon[row + 1, col + 1] = null;
            _pokemonGrid.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void MarkSurroundingTokensNull_IndexOnEdge_SurroundingTokensMarkedNull()
        {
            int row = 4;
            int col = 0;
            _pokemon[row - 1, col] = null;
            _pokemon[row - 1, col + 1] = null;
            _pokemon[row, col + 1] = null;
            _pokemon[row + 1, col] = null;
            _pokemon[row + 1, col + 1] = null;
            _pokemonGrid.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkSurroundingTokensNull_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonGrid.markSurroundingTokensNull(-12, -12);
        }

        [Test]
        public void MarkFullColumnAndRowAsNull_IndicesWithinRange_RowAndColumnMarkedAsNull()
        {
            int row = 4;
            int col = 4;
            for (int currentRow = 0; currentRow < PokemonBoard.gridSize; currentRow++)
            {
                _pokemon[currentRow, col] = null;
            }
            for (int currentCol = 0; currentCol < PokemonBoard.gridSize; currentCol++)
            {
                _pokemon[row, currentCol] = null;
            }
            _pokemonGrid.markFullRowAndColumnAsNull(row, col);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);            
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkFullColumnAndRowAsNull_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonGrid.markFullRowAndColumnAsNull(-12, -12);
        }

        [Test]
        public void MarkAllTokensOfTheSameTypeAsNull_ValidTokenType_TokensMarkedAsNull()
        {
            Type type = typeof(BulbasaurToken);
            for (int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    if (_pokemon[row, col].GetType() == type)
                    {
                        _pokemon[row, col] = null;
                    }
                }
            }
            _pokemonGrid.markAllTokensOfSameTypeAsNull(type);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void MarkAllTokensOfTheSameTypeAsNull_NoTokensOfGivenType_BoardUnchanged()
        {
            Type type = typeof(PichuToken);
            _pokemonGrid.markAllTokensOfSameTypeAsNull(type);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void MarkSpecials_RowOfFour_TokenReplacedWithFirstEvolution()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemonGrid.PokemonGrid = _pokemon;
            _pokemon[0, 0] = new PikachuToken();
            _pokemonGrid.evolveToken(0, 0, 4);
            Assert.AreEqual(_pokemon[0, 0], _pokemonGrid.NewPokemonGrid[0, 0]);
        }

        [Test]
        public void MarkSpecials_RowOfFive_TokenReplacedWithDitto()
        {
            _pokemon[0, 0] = new DittoToken();
            _pokemonGrid.evolveToken(0, 0, 5);
            Assert.AreEqual(_pokemon[0, 0], _pokemonGrid.NewPokemonGrid[0, 0]);
        }

        [Test]
        public void MarkSpecials_RowOfSix_TokenReplacedWithSecondEvolution()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemonGrid.PokemonGrid = _pokemon;
            _pokemon[0, 0] = new RaichuToken();
            _pokemonGrid.evolveToken(0, 0, 6);
            Assert.AreEqual(_pokemon[0, 0], _pokemonGrid.NewPokemonGrid[0, 0]);
        }

        [Test]
        public void MarkRowSpecials_FirstEvolutionToken_MarkSurroundingTokensNullCalled()
        {
            _pokemon[0, 0] = new PikachuToken();
            _mockGrid.PokemonGrid = _pokemon;
            _mockGrid.Expect(g => g.markSurroundingTokensNull(0, 0));
            _mockGrid.Replay();
            _mockGrid.markRowSpecials(0, 0, 3);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void MarkRowSpecials_SecondEvolutionToken_MarkFullRowAndColumnAsNullCalled()
        {
            _pokemon[0, 0] = new RaichuToken();
            _mockGrid.PokemonGrid = _pokemon;
            _mockGrid.Expect(g => g.markFullRowAndColumnAsNull(0, 0));
            _mockGrid.Replay();
            _mockGrid.markRowSpecials(0, 0, 3);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void SwapDitto_SuccessfulSwap_MarkAllTokensOfSameTypeAsNullCalled()
        {
            _pokemon[0, 0] = new DittoToken();
            _mockGrid.PokemonGrid = _pokemon;
            _mockGrid.Expect(g => g.markAllTokensOfSameTypeAsNull(_pokemon[0, 0].GetType()));
            _mockGrid.Replay();
            _mockGrid.swapDitto(0, 0, 0, 1);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[1, 0] = new PichuToken();
            _pokemon[2, 0] = new PichuToken();
            _pokemonGrid.PokemonGrid = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[1, 0] = null;
            _pokemon[2, 0] = null;
            _pokemonGrid.updateAllColumns();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnBottomEdge_ColumnMarkedAsNull()
        {
            _pokemon[5, 0] = new PichuToken();
            _pokemon[6, 0] = new PichuToken();
            _pokemon[7, 0] = new PichuToken();
            _pokemonGrid.PokemonGrid = _pokemon;
            _pokemon[5, 0] = null;
            _pokemon[6, 0] = null;
            _pokemon[7, 0] = null;
            _pokemonGrid.updateAllColumns();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        {
            _pokemon[3, 0] = new PichuToken();
            _pokemon[4, 0] = new PichuToken();
            _pokemon[5, 0] = new PichuToken();
            _pokemonGrid.PokemonGrid = _pokemon;
            _pokemon[3, 0] = null;
            _pokemon[4, 0] = null;
            _pokemon[5, 0] = null;
            _pokemonGrid.updateAllColumns();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[0, 1] = new PichuToken();
            _pokemon[0, 2] = new PichuToken();
            _pokemonGrid.PokemonGrid = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[0, 1] = null;
            _pokemon[0, 2] = null;
            _pokemonGrid.updateAllRows();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnRightEdge_RowMarkedAsNull()
        {
            _pokemon[0, 5] = new PichuToken();
            _pokemon[0, 6] = new PichuToken();
            _pokemon[0, 7] = new PichuToken();
            _pokemonGrid.PokemonGrid = _pokemon;
            _pokemon[0, 5] = null;
            _pokemon[0, 6] = null;
            _pokemon[0, 7] = null;
            _pokemonGrid.updateAllRows();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeInMiddle_RowMarkedAsNull()
        {
            _pokemon[0, 3] = new PichuToken();
            _pokemon[0, 4] = new PichuToken();
            _pokemon[0, 5] = new PichuToken();
            _pokemonGrid.PokemonGrid = _pokemon;
            _pokemon[0, 3] = null;
            _pokemon[0, 4] = null;
            _pokemon[0, 5] = null;
            _pokemonGrid.updateAllRows();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfFour_MarkSpecialsAndRow()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[0, 1] = new PichuToken();
            _pokemon[0, 2] = new PichuToken();
            _pokemon[0, 3] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 4));
            _mockGrid.Expect(g => g.evolveToken(0, 0, 4));
            _mockGrid.Replay();
            _mockGrid.updateAllRows();
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllRows_RowOfFive_MarkSpecialsAndRow()
        {
            _pokemon[0, 3] = new PichuToken();
            _pokemon[0, 4] = new PichuToken();
            _pokemon[0, 5] = new PichuToken();
            _pokemon[0, 6] = new PichuToken();
            _pokemon[0, 7] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 3, 5));
            _mockGrid.Expect(g => g.evolveToken(0, 3, 5));
            _mockGrid.Replay();
            _mockGrid.updateAllRows();
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllRows_RowOfSix_MarkSpecialsAndRow()
        {
            _pokemon[0, 2] = new PichuToken();
            _pokemon[0, 3] = new PichuToken();
            _pokemon[0, 4] = new PichuToken();
            _pokemon[0, 5] = new PichuToken();
            _pokemon[0, 6] = new PichuToken();
            _pokemon[0, 7] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 2, 6));
            _mockGrid.Expect(g => g.evolveToken(0, 2, 6));
            _mockGrid.Replay();
            _mockGrid.updateAllRows();
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllColumns_NoError_CallMarkRow()
        {
            _mockGrid.Expect(g => g.updateAllRows());
            _mockGrid.Replay();
            _mockGrid.updateAllColumns();
            _mockGrid.VerifyAllExpectations();
        }
        
        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromLeft_NullifyRowAndEvolveToken()
        {
            _pokemon[1, 0] = new PichuToken();
            _pokemon[0, 1] = new PichuToken();
            _pokemon[0, 2] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 3));
            _mockGrid.Expect(g => g.evolveToken(0, 0, 3));
            _mockGrid.Replay();
            _mockGrid.updateSingleRow(1, 0, 0, 0);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromMiddle_NullifyRowAndEvolveToken()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[1, 1] = new PichuToken();
            _pokemon[0, 2] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 3));
            _mockGrid.Expect(g => g.evolveToken(0, 0, 3));
            _mockGrid.Replay();
            _mockGrid.updateSingleRow(1, 1, 0, 1);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromRight_NullifyRowAndEvolveToken()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[0, 1] = new PichuToken();
            _pokemon[1, 2] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 3));
            _mockGrid.Expect(g => g.evolveToken(0, 0, 3));
            _mockGrid.Replay();
            _mockGrid.updateSingleRow(1, 2, 0, 2);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_NoRowOfThree_BoardUnchanged()
        {
            _pokemonGrid.updateSingleRow(1, 0, 0, 0);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemonGrid);
        }

        [Test]
        public void PullDownTokens_NoError_NoNullsLeftInGrid()
        {
            _pokemon[0, 0] = null;
            _pokemon[2, 3] = new PichuToken();
            _pokemon[3, 3] = null;
            _pokemon[7, 7] = null;
            _pokemon[6, 7] = new PichuToken();
            _pokemonGrid.NewPokemonGrid = _pokemon;
            _pokemonGrid.pullDownTokens();
            PokemonBoard.printGrid(_pokemon);
            PokemonBoard.printGrid(_pokemonGrid.PokemonGrid);
            Assert.IsNotNull(_pokemonGrid.PokemonGrid[0, 0]);
            Assert.AreEqual(_pokemon[2, 3], _pokemonGrid.PokemonGrid[3,3]);
            Assert.AreEqual(_pokemon[6, 7], _pokemonGrid.PokemonGrid[7,7]);
        }
    }
}
