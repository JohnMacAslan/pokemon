using System;
using PokemonBejeweled.Pokemon;
using PokemonBejeweled;
using NUnit.Framework;
using Rhino.Mocks;

namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class PokemonBoardTests
    {
        private PokemonToken[,] _pokemonGrid = new PokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
        private PokemonBoard _pokemonBoard;
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
            _pokemonBoard = new PokemonBoard();
            for (int i = 0; i < 8; i+=2) {
                for (int j = 0; j < 8; j += 2)
                {
                    _pokemonGrid[i, j] = new BulbasaurToken();
                    _pokemonGrid[i + 1, j] = new CharmanderToken();
                    _pokemonGrid[i + 1, j + 1] = new BulbasaurToken();
                    _pokemonGrid[i, j + 1] = new CharmanderToken();
                }
            }
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonBoard.NewPokemonGrid = _pokemonGrid;
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
            IBasicPokemonToken[,] _invertedPokemon = new IBasicPokemonToken[3, 3];
            _invertedPokemon[0, 0] = new BulbasaurToken();
            _invertedPokemon[0, 1] = new CharmanderToken();
            _invertedPokemon[0, 2] = new BulbasaurToken();
            _invertedPokemon[1, 0] = new BulbasaurToken();
            _invertedPokemon[1, 1] = new CharmanderToken();
            _invertedPokemon[1, 2] = new BulbasaurToken();
            _invertedPokemon[2, 0] = new BulbasaurToken();
            _invertedPokemon[2, 1] = new CharmanderToken();
            _invertedPokemon[2, 2] = new BulbasaurToken();
            GridOperations.invertGrid(_pokemonToInvert);
            Assert.AreEqual(_pokemonToInvert, _invertedPokemon);
        }

        [Test]
        public void PokemonGrid_NoError_GridInitializedToIBasicPokemon()
        {
            _pokemonBoard = new PokemonBoard();
            for (int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    Assert.IsInstanceOf(typeof(IBasicPokemonToken), _pokemonGrid[row, col]);
                }
            }
        }

        [Test]
        public void PokemonGrid_NoError_PokemonAndNewPokemonAreEqual()
        {
            _pokemonBoard = new PokemonBoard();
            Assert.AreEqual(_pokemonBoard.PokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void CopyGrid_ValidGrid_GridCopiedCorrectly()
        {
            PokemonToken[,] newPokemon = new PokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
            GridOperations.copyGrid(_pokemonGrid, newPokemon);
            Assert.AreEqual(_pokemonGrid, newPokemon);
        }

        [Test]
        public void UpdateBoard_NoRowsOrColumnsOfThree_GridUnchanged()
        {
            _pokemonBoard.makePlay(0, 0, 0, 0);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.PokemonGrid);
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreHorizontallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(_pokemonBoard.piecesAreAdjacent(1, 1, 1, 2));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreVerticallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(_pokemonBoard.piecesAreAdjacent(1, 2, 1, 1));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreNotAdjacent_ReturnFalse()
        {
            Assert.IsFalse(_pokemonBoard.piecesAreAdjacent(1, 1, 2, 2));
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensLessThan3_PokemonGridUnchanged()
        {
            _pokemonBoard.markNullRow(0, 0, 0);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensGreaterThan3_RowMarkNull()
        {
            _pokemonGrid[0, 0] = null;
            _pokemonGrid[0, 1] = null;
            _pokemonGrid[0, 2] = null;
            _pokemonGrid[0, 3] = null;
            _pokemonBoard.markNullRow(0, 0, 4);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkNullRow_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonBoard.markNullRow(-1, 0, 4);
        }

        [Test]
        public void MarkSurroundingTokensNull_IndicesWithinRange_SurroundingTokensMarkedNull()
        {
            int row = 4;
            int col = 4;
            _pokemonGrid[row - 1, col] = null;
            _pokemonGrid[row - 1, col - 1] = null;
            _pokemonGrid[row - 1, col + 1] = null;
            _pokemonGrid[row, col - 1] = null;
            _pokemonGrid[row, col + 1] = null;
            _pokemonGrid[row + 1, col] = null;
            _pokemonGrid[row + 1, col - 1] = null;
            _pokemonGrid[row + 1, col + 1] = null;
            _pokemonBoard.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void MarkSurroundingTokensNull_IndexOnEdge_SurroundingTokensMarkedNull()
        {
            int row = 4;
            int col = 0;
            _pokemonGrid[row - 1, col] = null;
            _pokemonGrid[row - 1, col + 1] = null;
            _pokemonGrid[row, col + 1] = null;
            _pokemonGrid[row + 1, col] = null;
            _pokemonGrid[row + 1, col + 1] = null;
            _pokemonBoard.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkSurroundingTokensNull_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonBoard.markSurroundingTokensNull(-12, -12);
        }

        [Test]
        public void MarkFullColumnAndRowAsNull_IndicesWithinRange_RowAndColumnMarkedAsNull()
        {
            int row = 4;
            int col = 4;
            for (int currentRow = 0; currentRow < PokemonBoard.gridSize; currentRow++)
            {
                _pokemonGrid[currentRow, col] = null;
            }
            for (int currentCol = 0; currentCol < PokemonBoard.gridSize; currentCol++)
            {
                _pokemonGrid[row, currentCol] = null;
            }
            _pokemonBoard.markFullRowAndColumnAsNull(row, col);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);            
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkFullColumnAndRowAsNull_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonBoard.markFullRowAndColumnAsNull(-12, -12);
        }

        [Test]
        public void MarkAllTokensOfTheSameTypeAsNull_ValidTokenType_TokensMarkedAsNull()
        {
            Type type = typeof(BulbasaurToken);
            for (int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    if (_pokemonGrid[row, col].GetType() == type)
                    {
                        _pokemonGrid[row, col] = null;
                    }
                }
            }
            _pokemonBoard.markAllTokensOfSameTypeAsNull(new BulbasaurToken());
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void MarkAllTokensOfTheSameTypeAsNull_NoTokensOfGivenType_BoardUnchanged()
        {
            Type type = typeof(PichuToken);
            _pokemonBoard.markAllTokensOfSameTypeAsNull(new PichuToken());
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void EvolveToken_RowOfFour_TokenReplacedWithFirstEvolution()
        {
            _pokemonGrid[0, 0] = new PichuToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 0] = new PikachuToken();
            _pokemonBoard.evolveToken(0, 0, 4);
            Assert.AreEqual(_pokemonGrid[0, 0], _pokemonBoard.NewPokemonGrid[0, 0]);
        }

        [Test]
        public void EvolveToken_RowOfFive_TokenReplacedWithDitto()
        {
            _pokemonGrid[0, 0] = new DittoToken();
            _pokemonBoard.evolveToken(0, 0, 5);
            Assert.AreEqual(_pokemonGrid[0, 0], _pokemonBoard.NewPokemonGrid[0, 0]);
        }

        [Test]
        public void EvolveToken_RowOfSix_TokenReplacedWithSecondEvolution()
        {
            _pokemonGrid[0, 0] = new PichuToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 0] = new RaichuToken();
            _pokemonBoard.evolveToken(0, 0, 6);
            Assert.AreEqual(_pokemonGrid[0, 0], _pokemonBoard.NewPokemonGrid[0, 0]);
        }

        [Test]
        public void SwapDitto_SuccessfulSwap_MarkAllTokensOfSameTypeAsNullCalled()
        {
            _pokemonGrid[0, 0] = new DittoToken();
            _mockGrid.PokemonGrid = _pokemonGrid;
            _mockGrid.Expect(g => g.markAllTokensOfSameTypeAsNull(_pokemonGrid[0, 1]));
            _mockGrid.Replay();
            _mockGrid.swapDitto(0, 0, 0, 1);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        {
            _pokemonGrid[0, 0] = new PichuToken();
            _pokemonGrid[1, 0] = new PichuToken();
            _pokemonGrid[2, 0] = new PichuToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 0] = null;
            _pokemonGrid[1, 0] = null;
            _pokemonGrid[2, 0] = null;
            _pokemonBoard.updateAllColumns();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnBottomEdge_ColumnMarkedAsNull()
        {
            _pokemonGrid[5, 0] = new PichuToken();
            _pokemonGrid[6, 0] = new PichuToken();
            _pokemonGrid[7, 0] = new PichuToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[5, 0] = null;
            _pokemonGrid[6, 0] = null;
            _pokemonGrid[7, 0] = null;
            _pokemonBoard.updateAllColumns();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        {
            _pokemonGrid[3, 0] = new PichuToken();
            _pokemonGrid[4, 0] = new PichuToken();
            _pokemonGrid[5, 0] = new PichuToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[3, 0] = null;
            _pokemonGrid[4, 0] = null;
            _pokemonGrid[5, 0] = null;
            _pokemonBoard.updateAllColumns();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        {
            _pokemonGrid[0, 0] = new PichuToken();
            _pokemonGrid[0, 1] = new PichuToken();
            _pokemonGrid[0, 2] = new PichuToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 0] = null;
            _pokemonGrid[0, 1] = null;
            _pokemonGrid[0, 2] = null;
            _pokemonBoard.updateAllRows();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnRightEdge_RowMarkedAsNull()
        {
            _pokemonGrid[0, 5] = new PichuToken();
            _pokemonGrid[0, 6] = new PichuToken();
            _pokemonGrid[0, 7] = new PichuToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 5] = null;
            _pokemonGrid[0, 6] = null;
            _pokemonGrid[0, 7] = null;
            _pokemonBoard.updateAllRows();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeInMiddle_RowMarkedAsNull()
        {
            _pokemonGrid[0, 3] = new PichuToken();
            _pokemonGrid[0, 4] = new PichuToken();
            _pokemonGrid[0, 5] = new PichuToken();
            _pokemonBoard.PokemonGrid = _pokemonGrid;
            _pokemonGrid[0, 3] = null;
            _pokemonGrid[0, 4] = null;
            _pokemonGrid[0, 5] = null;
            _pokemonBoard.updateAllRows();
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void UpdateAllRows_RowOfFour_MarkSpecialsAndRow()
        {
            _pokemonGrid[0, 0] = new PichuToken();
            _pokemonGrid[0, 1] = new PichuToken();
            _pokemonGrid[0, 2] = new PichuToken();
            _pokemonGrid[0, 3] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemonGrid;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 4));
            _mockGrid.Expect(g => g.evolveToken(0, 0, 4));
            _mockGrid.Replay();
            _mockGrid.updateAllRows();
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllRows_RowOfFive_MarkSpecialsAndRow()
        {
            _pokemonGrid[0, 3] = new PichuToken();
            _pokemonGrid[0, 4] = new PichuToken();
            _pokemonGrid[0, 5] = new PichuToken();
            _pokemonGrid[0, 6] = new PichuToken();
            _pokemonGrid[0, 7] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemonGrid;
            _mockGrid.Expect(g => g.markNullRow(0, 3, 5));
            _mockGrid.Expect(g => g.evolveToken(0, 3, 5));
            _mockGrid.Replay();
            _mockGrid.updateAllRows();
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllRows_RowOfSix_MarkSpecialsAndRow()
        {
            _pokemonGrid[0, 2] = new PichuToken();
            _pokemonGrid[0, 3] = new PichuToken();
            _pokemonGrid[0, 4] = new PichuToken();
            _pokemonGrid[0, 5] = new PichuToken();
            _pokemonGrid[0, 6] = new PichuToken();
            _pokemonGrid[0, 7] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemonGrid;
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
            _pokemonGrid[0, 0] = new PichuToken();
            _pokemonGrid[0, 1] = new PichuToken();
            _pokemonGrid[0, 2] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemonGrid;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 3));
            _mockGrid.Expect(g => g.evolveToken(0, 0, 3));
            _mockGrid.Replay();
            _mockGrid.updateSingleRow(0, 0);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromMiddle_NullifyRowAndEvolveToken()
        {
            _pokemonGrid[0, 0] = new PichuToken();
            _pokemonGrid[0, 1] = new PichuToken();
            _pokemonGrid[0, 2] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemonGrid;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 3));
            _mockGrid.Expect(g => g.evolveToken(0, 1, 3));
            _mockGrid.Replay();
            _mockGrid.updateSingleRow(0, 1);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromRight_NullifyRowAndEvolveToken()
        {
            _pokemonGrid[0, 0] = new PichuToken();
            _pokemonGrid[0, 1] = new PichuToken();
            _pokemonGrid[0, 2] = new PichuToken();
            _mockGrid.PokemonGrid = _pokemonGrid;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 3));
            _mockGrid.Expect(g => g.evolveToken(0, 2, 3));
            _mockGrid.Replay();
            _mockGrid.updateSingleRow(0, 2);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleColumn_ValidIndices_InvertGridsUpdateRowInvertBack()
        {
            _mockGrid.PokemonGrid = _pokemonGrid;
            _mockGrid.Expect(g => g.updateSingleRow(0, 0));
            _mockGrid.Replay();
            _mockGrid.updateSingleColumn(0, 0);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_NoRowOfThree_BoardUnchanged()
        {
            _pokemonBoard.updateSingleRow(0, 0);
            Assert.AreEqual(_pokemonGrid, _pokemonBoard.NewPokemonGrid);
        }

        [Test]
        public void PullDownTokens_NoError_NoNullsLeftInGrid()
        {
            _pokemonGrid[0, 0] = null;
            _pokemonGrid[2, 3] = new PichuToken();
            _pokemonGrid[3, 3] = null;
            _pokemonGrid[7, 7] = null;
            _pokemonGrid[6, 7] = new PichuToken();
            _pokemonBoard.NewPokemonGrid = _pokemonGrid;
            _pokemonBoard.pullDownTokens();
            Assert.IsNotNull(_pokemonBoard.PokemonGrid[0, 0]);
            Assert.AreEqual(_pokemonGrid[2, 3], _pokemonBoard.PokemonGrid[3,3]);
            Assert.AreEqual(_pokemonGrid[6, 7], _pokemonBoard.PokemonGrid[7,7]);
        }
    }
}
