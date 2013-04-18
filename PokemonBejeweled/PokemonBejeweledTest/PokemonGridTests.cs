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
        private PokemonToken[,] _pokemon = new PokemonToken[PokemonGrid.gridSize, PokemonGrid.gridSize];
        private PokemonGrid _pokemonGrid;
        private MockRepository _mocks;
        private PokemonGrid _mockGrid;

        [SetUp]
        public void setupMocks()
        {
            _mocks = new MockRepository();
            _mockGrid = _mocks.PartialMock<PokemonGrid>();
        }

        [SetUp]
        public void resetPokemonGrid()
        {
            _pokemonGrid = new PokemonGrid();
            for (int i = 0; i < 8; i+=2) {
                for (int j = 0; j < 8; j += 2)
                {
                    _pokemon[i, j] = new BulbasaurToken();
                    _pokemon[i + 1, j] = new CharmanderToken();
                    _pokemon[i + 1, j + 1] = new BulbasaurToken();
                    _pokemon[i, j + 1] = new CharmanderToken();
                }
            }
            _pokemonGrid.Pokemon = _pokemon;
            _pokemonGrid.NewPokemon = _pokemon;
        }

        [Test]
        public void PokemonGrid_NoError_GridInitializedToNotNulls()
        {
            _pokemonGrid = new PokemonGrid();
            for (int row = 0; row < PokemonGrid.gridSize; row++)
            {
                for (int col = 0; col < PokemonGrid.gridSize; col++)
                {
                    Assert.NotNull(_pokemonGrid.Pokemon[row, col]);
                }
            }
        }

        [Test]
        public void PokemonGrid_NoError_PokemonAndNewPokemonAreEqual()
        {
            _pokemonGrid = new PokemonGrid();
            Assert.AreEqual(_pokemonGrid.Pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void CopyGrid_ValidGrid_GridCopiedCorrectly()
        {
            PokemonToken[,] newPokemon = new PokemonToken[PokemonGrid.gridSize, PokemonGrid.gridSize];
            PokemonGrid.copyGrid(_pokemon, newPokemon);
            Assert.AreEqual(_pokemon, newPokemon);
        }

        [Test]
        public void UpdateBoard_NoRowsOrColumnsOfThree_GridUnchanged()
        {
            _pokemonGrid.updateBoard(0, 0, 0, 0);
            Assert.AreEqual(_pokemon, _pokemonGrid.Pokemon);
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
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensGreaterThan3_RowMarkNull()
        {
            _pokemon[0, 0] = null;
            _pokemon[0, 1] = null;
            _pokemon[0, 2] = null;
            _pokemon[0, 3] = null;
            _pokemonGrid.markNullRow(0, 0, 4);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkNullRow_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonGrid.markNullRow(-1, 0, 4);
        }

        [Test]
        public void MarkNullColumn_NumberOfSameTokensLessThan3_PokemonGridUnchanged()
        {
            _pokemonGrid.markNullColumn(0, 0, 0);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkNullColumn_NumberOfSameTokensGreaterThan3_RowMarkNull()
        {
            _pokemon[0, 0] = null;
            _pokemon[1, 0] = null;
            _pokemon[2, 0] = null;
            _pokemon[3, 0] = null;
            _pokemonGrid.markNullColumn(0, 0, 4);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkNullColumn_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            _pokemonGrid.markNullColumn(-1, 0, 4);
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
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
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
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
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
            for (int currentRow = 0; currentRow < PokemonGrid.gridSize; currentRow++)
            {
                _pokemon[currentRow, col] = null;
            }
            for (int currentCol = 0; currentCol < PokemonGrid.gridSize; currentCol++)
            {
                _pokemon[row, currentCol] = null;
            }
            _pokemonGrid.markFullRowAndColumnAsNull(row, col);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);            
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
            for (int row = 0; row < PokemonGrid.gridSize; row++)
            {
                for (int col = 0; col < PokemonGrid.gridSize; col++)
                {
                    if (_pokemon[row, col].GetType() == type)
                    {
                        _pokemon[row, col] = null;
                    }
                }
            }
            _pokemonGrid.markAllTokensOfSameTypeAsNull(type);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkAllTokensOfTheSameTypeAsNull_NoTokensOfGivenType_BoardUnchanged()
        {
            Type type = typeof(PichuToken);
            _pokemonGrid.markAllTokensOfSameTypeAsNull(type);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkSpecials_RowOfFour_TokenReplacedWithFirstEvolution()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = new PikachuToken();
            _pokemonGrid.markSpecials(0, 0, 4);
            Assert.AreEqual(_pokemon[0, 0], _pokemonGrid.NewPokemon[0, 0]);
        }

        [Test]
        public void MarkSpecials_RowOfFive_TokenReplacedWithDitto()
        {
            _pokemon[0, 0] = new DittoToken();
            _pokemonGrid.markSpecials(0, 0, 5);
            Assert.AreEqual(_pokemon[0, 0], _pokemonGrid.NewPokemon[0, 0]);
        }

        [Test]
        public void MarkSpecials_RowOfSix_TokenReplacedWithSecondEvolution()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = new RaichuToken();
            _pokemonGrid.markSpecials(0, 0, 6);
            Assert.AreEqual(_pokemon[0, 0], _pokemonGrid.NewPokemon[0, 0]);
        }

        [Test]
        public void MarkSpecials_FirstEvolutionToken_MarkSurroundingTokensNullCalled()
        {
            _pokemon[0, 0] = new PikachuToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markSurroundingTokensNull(0, 0));
            _mockGrid.Replay();
            _mockGrid.markSpecials(0, 0, 0);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void MarkSpecials_SecondEvolutionToken_MarkFullRowAndColumnAsNullCalled()
        {
            _pokemon[0, 0] = new RaichuToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markFullRowAndColumnAsNull(0, 0));
            _mockGrid.Replay();
            _mockGrid.markSpecials(0, 0, 0);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void MarkSpecials_DittoToken_MarkAllTokensOfSameTypeAsNullCalled()
        {
            _pokemon[0, 0] = new DittoToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markAllTokensOfSameTypeAsNull(_pokemon[0, 0].GetType()));
            _mockGrid.Replay();
            _mockGrid.markSpecials(0, 0, 0);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[1, 0] = new PichuToken();
            _pokemon[2, 0] = new PichuToken();
            _pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[1, 0] = null;
            _pokemon[2, 0] = null;
            _pokemonGrid.updateAllColumns();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnBottomEdge_ColumnMarkedAsNull()
        {
            _pokemon[5, 0] = new PichuToken();
            _pokemon[6, 0] = new PichuToken();
            _pokemon[7, 0] = new PichuToken();
            _pokemonGrid.Pokemon = _pokemon;
            _pokemon[5, 0] = null;
            _pokemon[6, 0] = null;
            _pokemon[7, 0] = null;
            _pokemonGrid.updateAllColumns();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        {
            _pokemon[3, 0] = new PichuToken();
            _pokemon[4, 0] = new PichuToken();
            _pokemon[5, 0] = new PichuToken();
            _pokemonGrid.Pokemon = _pokemon;
            _pokemon[3, 0] = null;
            _pokemon[4, 0] = null;
            _pokemon[5, 0] = null;
            _pokemonGrid.updateAllColumns();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[0, 1] = new PichuToken();
            _pokemon[0, 2] = new PichuToken();
            _pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[0, 1] = null;
            _pokemon[0, 2] = null;
            _pokemonGrid.updateAllRows();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnRightEdge_RowMarkedAsNull()
        {
            _pokemon[0, 5] = new PichuToken();
            _pokemon[0, 6] = new PichuToken();
            _pokemon[0, 7] = new PichuToken();
            _pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 5] = null;
            _pokemon[0, 6] = null;
            _pokemon[0, 7] = null;
            _pokemonGrid.updateAllRows();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeInMiddle_RowMarkedAsNull()
        {
            _pokemon[0, 3] = new PichuToken();
            _pokemon[0, 4] = new PichuToken();
            _pokemon[0, 5] = new PichuToken();
            _pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 3] = null;
            _pokemon[0, 4] = null;
            _pokemon[0, 5] = null;
            _pokemonGrid.updateAllRows();
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllRows_RowOfFour_MarkSpecialsAndRow()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[0, 1] = new PichuToken();
            _pokemon[0, 2] = new PichuToken();
            _pokemon[0, 3] = new PichuToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 4));
            _mockGrid.Expect(g => g.markSpecials(0, 0, 4));
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
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 3, 5));
            _mockGrid.Expect(g => g.markSpecials(0, 3, 5));
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
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 2, 6));
            _mockGrid.Expect(g => g.markSpecials(0, 2, 6));
            _mockGrid.Replay();
            _mockGrid.updateAllRows();
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllColumns_ColumnOfFour_MarkSpecialsAndColumn()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[1, 0] = new PichuToken();
            _pokemon[2, 0] = new PichuToken();
            _pokemon[3, 0] = new PichuToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markNullColumn(0, 0, 4));
            _mockGrid.Expect(g => g.markSpecials(0, 0, 4));
            _mockGrid.Replay();
            _mockGrid.updateAllColumns();
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateBoard_ColumnOfFive_MarkSpecialsAndColumn()
        {
            _pokemon[3, 0] = new PichuToken();
            _pokemon[4, 0] = new PichuToken();
            _pokemon[5, 0] = new PichuToken();
            _pokemon[6, 0] = new PichuToken();
            _pokemon[7, 0] = new PichuToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markNullColumn(3, 0, 5));
            _mockGrid.Expect(g => g.markSpecials(3, 0, 5));
            _mockGrid.Replay();
            _mockGrid.updateAllColumns();
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateBoard_ColumnOfSix_MarkSpecialsAndColumn()
        {
            _pokemon[2, 0] = new PichuToken();
            _pokemon[3, 0] = new PichuToken();
            _pokemon[4, 0] = new PichuToken();
            _pokemon[5, 0] = new PichuToken();
            _pokemon[6, 0] = new PichuToken();
            _pokemon[7, 0] = new PichuToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markNullColumn(2, 0, 6));
            _mockGrid.Expect(g => g.markSpecials(2, 0, 6));
            _mockGrid.Replay();
            _mockGrid.updateAllColumns();
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromLeft_MarkSpecialsAndRow()
        {
            _pokemon[1, 0] = new PichuToken();
            _pokemon[0, 1] = new PichuToken();
            _pokemon[0, 2] = new PichuToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 3));
            _mockGrid.Expect(g => g.markSpecials(0, 0, 3));
            _mockGrid.Replay();
            _mockGrid.updateSingleRow(1, 0, 0, 0);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromMiddle_MarkSpecialsAndRow()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[1, 1] = new PichuToken();
            _pokemon[0, 2] = new PichuToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 3));
            _mockGrid.Expect(g => g.markSpecials(0, 0, 3));
            _mockGrid.Replay();
            _mockGrid.updateSingleRow(1, 1, 0, 1);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_RowOfThreeStartFromRight_MarkSpecialsAndRow()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[0, 1] = new PichuToken();
            _pokemon[1, 2] = new PichuToken();
            _mockGrid.Pokemon = _pokemon;
            _mockGrid.Expect(g => g.markNullRow(0, 0, 3));
            _mockGrid.Expect(g => g.markSpecials(0, 0, 3));
            _mockGrid.Replay();
            _mockGrid.updateSingleRow(1, 2, 0, 2);
            _mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateSingleRow_NoRowOfThree_BoardUnchanged()
        {
            _pokemonGrid.updateSingleRow(1, 0, 0, 0);
            Assert.AreEqual(_pokemon, _pokemonGrid.NewPokemon);
        }

        [Test]
        public void PullDownTokens_NoError_NoNullsLeftInGrid()
        {
            _pokemon[0, 0] = null;
            _pokemon[2, 3] = new PichuToken();
            _pokemon[3, 3] = null;
            _pokemon[7, 7] = null;
            _pokemon[6, 7] = new PichuToken();
            _pokemonGrid.NewPokemon = _pokemon;
            _pokemonGrid.pullDownTokens();
            PokemonGrid.printGrid(_pokemon);
            PokemonGrid.printGrid(_pokemonGrid.Pokemon);
            Assert.IsNotNull(_pokemonGrid.Pokemon[0, 0]);
            Assert.AreEqual(_pokemon[2, 3], _pokemonGrid.Pokemon[3,3]);
            Assert.AreEqual(_pokemon[6, 7], _pokemonGrid.Pokemon[7,7]);
        }
    }
}
