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
        private PokemonGrid pokemonGrid;

        [SetUp]
        public void resetPokemonGrid()
        {
            pokemonGrid = new PokemonGrid();
            for (int i = 0; i < 8; i+=2) {
                for (int j = 0; j < 8; j += 2)
                {
                    _pokemon[i, j] = new BulbasaurToken();
                    _pokemon[i + 1, j] = new CharmanderToken();
                    _pokemon[i + 1, j + 1] = new BulbasaurToken();
                    _pokemon[i, j + 1] = new CharmanderToken();
                }
            }
            pokemonGrid.Pokemon = _pokemon;
            pokemonGrid.NewPokemon = _pokemon;
        }

        [Test]
        public void PokemonGrid_NoError_GridInitializedToNotNulls()
        {
            pokemonGrid = new PokemonGrid();
            for (int row = 0; row < PokemonGrid.gridSize; row++)
            {
                for (int col = 0; col < PokemonGrid.gridSize; col++)
                {
                    Assert.NotNull(pokemonGrid.Pokemon[row, col]);
                }
            }
        }

        [Test]
        public void PokemonGrid_NoError_PokemonAndNewPokemonAreEqual()
        {
            pokemonGrid = new PokemonGrid();
            Assert.AreEqual(pokemonGrid.Pokemon, pokemonGrid.NewPokemon);
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
            pokemonGrid.updateBoard(0, 0, 0, 0);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreHorizontallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(pokemonGrid.piecesAreAdjacent(1, 1, 1, 2));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreVerticallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(pokemonGrid.piecesAreAdjacent(1, 2, 1, 1));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreNotAdjacent_ReturnFalse()
        {
            Assert.IsFalse(pokemonGrid.piecesAreAdjacent(1, 1, 2, 2));
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensLessThan3_PokemonGridUnchanged()
        {
            pokemonGrid.markNullRow(0, 0, 0);
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensGreaterThan3_RowMarkNull()
        {
            _pokemon[0, 0] = null;
            _pokemon[0, 1] = null;
            _pokemon[0, 2] = null;
            _pokemon[0, 3] = null;
            pokemonGrid.markNullRow(0, 0, 4);
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkNullRow_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            pokemonGrid.markNullRow(-1, 0, 4);
        }

        [Test]
        public void MarkNullColumn_NumberOfSameTokensLessThan3_PokemonGridUnchanged()
        {
            pokemonGrid.markNullColumn(0, 0, 0);
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkNullColumn_NumberOfSameTokensGreaterThan3_RowMarkNull()
        {
            _pokemon[0, 0] = null;
            _pokemon[1, 0] = null;
            _pokemon[2, 0] = null;
            _pokemon[3, 0] = null;
            pokemonGrid.markNullColumn(0, 0, 4);
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkNullColumn_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            pokemonGrid.markNullColumn(-1, 0, 4);
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
            pokemonGrid.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
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
            pokemonGrid.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkSurroundingTokensNull_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            pokemonGrid.markSurroundingTokensNull(-12, -12);
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
            pokemonGrid.markFullRowAndColumnAsNull(row, col);
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);            
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkFullColumnAndRowAsNull_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            pokemonGrid.markFullRowAndColumnAsNull(-12, -12);
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
            pokemonGrid.markAllTokensOfSameTypeAsNull(type);
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkAllTokensOfTheSameTypeAsNull_NoTokensOfGivenType_BoardUnchanged()
        {
            Type type = typeof(PichuToken);
            pokemonGrid.markAllTokensOfSameTypeAsNull(type);
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkSpecials_SpecialOfFour_TokenReplacedWithFirstEvolution()
        {
            _pokemon[0, 0] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = new PikachuToken();
            pokemonGrid.markSpecials(0, 0, 4);
            Assert.AreEqual(_pokemon[0, 0], pokemonGrid.NewPokemon[0, 0]);
        }

        [Test]
        public void MarkSpecials_SpecialOfFour_MarkSurroundingTokensNullCalled()
        {
            MockRepository mocks = new MockRepository();
            PokemonGrid mockGrid = mocks.PartialMock<PokemonGrid>();
            mockGrid.Expect(g => g.markSurroundingTokensNull(0, 0));
            mockGrid.Replay();
            mockGrid.markSpecials(0, 0, 4);
            mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void MarkSpecials_SpecialOfFive_TokenReplacedWithDitto()
        {
            _pokemon[0, 0] = new DittoToken();
            pokemonGrid.markSpecials(0, 0, 5);
            Assert.AreEqual(_pokemon[0, 0], pokemonGrid.NewPokemon[0, 0]);
        }

        [Test]
        public void MarkSpecials_SpecialOfFive_MarkFullRowAndColumnAsNullCalled()
        {
            MockRepository mocks = new MockRepository();
            PokemonGrid mockGrid = mocks.PartialMock<PokemonGrid>();
            mockGrid.Expect(g => g.markFullRowAndColumnAsNull(0, 0));
            mockGrid.Replay();
            mockGrid.markSpecials(0, 0, 5);
            mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void MarkSpecials_SpecialOfSix_TokenReplacedWithSecondEvolution()
        {
            _pokemon[0, 0] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = new RaichuToken();
            pokemonGrid.markSpecials(0, 0, 6);
            Assert.AreEqual(_pokemon[0, 0], pokemonGrid.NewPokemon[0, 0]);
        }

        [Test]
        public void MarkSpecials_SpecialOfSix_MarkAllTokensOfSameTypeAsNullCalled()
        {
            MockRepository mocks = new MockRepository();
            PokemonGrid mockGrid = mocks.PartialMock<PokemonGrid>();
            mockGrid.Expect(g => g.markAllTokensOfSameTypeAsNull(_pokemon[0, 0].GetType()));
            mockGrid.Replay();
            mockGrid.markSpecials(0, 0, 6);
            mockGrid.VerifyAllExpectations();
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[1, 0] = new PichuToken();
            _pokemon[2, 0] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[1, 0] = null;
            _pokemon[2, 0] = null;
            pokemonGrid.updateAllColumns();
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeOnBottomEdge_ColumnMarkedAsNull()
        {
            _pokemon[5, 0] = new PichuToken();
            _pokemon[6, 0] = new PichuToken();
            _pokemon[7, 0] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[5, 0] = null;
            _pokemon[6, 0] = null;
            _pokemon[7, 0] = null;
            pokemonGrid.updateAllColumns();
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllColumns_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        {
            _pokemon[3, 0] = new PichuToken();
            _pokemon[4, 0] = new PichuToken();
            _pokemon[5, 0] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[3, 0] = null;
            _pokemon[4, 0] = null;
            _pokemon[5, 0] = null;
            pokemonGrid.updateAllColumns();
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        {
            _pokemon[0, 0] = new PichuToken();
            _pokemon[0, 1] = new PichuToken();
            _pokemon[0, 2] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[0, 1] = null;
            _pokemon[0, 2] = null;
            pokemonGrid.updateAllRows();
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeOnRightEdge_RowMarkedAsNull()
        {
            _pokemon[0, 5] = new PichuToken();
            _pokemon[0, 6] = new PichuToken();
            _pokemon[0, 7] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 5] = null;
            _pokemon[0, 6] = null;
            _pokemon[0, 7] = null;
            pokemonGrid.updateAllRows();
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void UpdateAllRows_RowOfThreeInMiddle_RowMarkedAsNull()
        {
            _pokemon[0, 3] = new PichuToken();
            _pokemon[0, 4] = new PichuToken();
            _pokemon[0, 5] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 3] = null;
            _pokemon[0, 4] = null;
            _pokemon[0, 5] = null;
            pokemonGrid.updateAllRows();
            Assert.AreEqual(_pokemon, pokemonGrid.NewPokemon);
        }

        //[Test]
        //public void UpdateAllRows_RowOfFour_LeftMostTokenReplacedByFirstEvolutionAndNullsSetCorrectly()
        //{
        //    _pokemon[0, 0] = new PichuToken();
        //    _pokemon[0, 1] = new PichuToken();
        //    _pokemon[0, 2] = new PichuToken();
        //    _pokemon[0, 3] = new PichuToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[0, 0] = new PikachuToken();
        //    _pokemon[0, 1] = null;
        //    _pokemon[0, 2] = null;
        //    _pokemon[0, 3] = null;
        //    _pokemon[1, 0] = null;
        //    _pokemon[1, 1] = null;
        //    pokemonGrid.updateAllRows();
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_RowOfFive_MovedTokenReplacedByDitto()
        //{
        //    _pokemon[0, 3] = new PichuToken();
        //    _pokemon[0, 4] = new PichuToken();
        //    _pokemon[0, 5] = new PichuToken();
        //    _pokemon[0, 6] = new PichuToken();
        //    _pokemon[0, 7] = new PichuToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[1, 3] = new PichuToken();
        //    _pokemon[0, 3] = new DittoToken();
        //    _pokemon[0, 4] = null;
        //    _pokemon[0, 5] = null;
        //    _pokemon[0, 6] = null;
        //    _pokemon[0, 7] = null;
        //    pokemonGrid.updateBoard(0, 3, 1, 3);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_RowOfSix_MovedTokenReplacedBySecondEvolution()
        //{
        //    _pokemon[0, 2] = new PichuToken();
        //    _pokemon[0, 3] = new PichuToken();
        //    _pokemon[0, 4] = new PichuToken();
        //    _pokemon[0, 5] = new PichuToken();
        //    _pokemon[0, 6] = new PichuToken();
        //    _pokemon[0, 7] = new PichuToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[1, 3] = new PichuToken();
        //    _pokemon[0, 2] = null;
        //    _pokemon[0, 3] = new RaichuToken();
        //    _pokemon[0, 4] = null;
        //    _pokemon[0, 5] = null;
        //    _pokemon[0, 6] = null;
        //    _pokemon[0, 7] = null;
        //    pokemonGrid.updateBoard(0, 3, 1, 3);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_ColumnOfFour_MovedTokenReplacedByFirstEvolution()
        //{
        //    _pokemon[3, 0] = new PichuToken();
        //    _pokemon[4, 0] = new PichuToken();
        //    _pokemon[5, 0] = new PichuToken();
        //    _pokemon[6, 0] = new PichuToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[3, 1] = new PichuToken();
        //    _pokemon[3, 0] = new PikachuToken();
        //    _pokemon[4, 0] = null;
        //    _pokemon[5, 0] = null;
        //    _pokemon[6, 0] = null;
        //    pokemonGrid.updateBoard(3, 0, 3, 1);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_ColumnOfFive_MovedTokenReplacedByDitto()
        //{
        //    _pokemon[3, 0] = new PichuToken();
        //    _pokemon[4, 0] = new PichuToken();
        //    _pokemon[5, 0] = new PichuToken();
        //    _pokemon[6, 0] = new PichuToken();
        //    _pokemon[7, 0] = new PichuToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[3, 1] = new PichuToken();
        //    _pokemon[3, 0] = new DittoToken();
        //    _pokemon[4, 0] = null;
        //    _pokemon[5, 0] = null;
        //    _pokemon[6, 0] = null;
        //    _pokemon[7, 0] = null;
        //    pokemonGrid.updateBoard(3, 0, 3, 1);
        //    Assert.AreEqual(_pokemon[0, 0], pokemonGrid.Pokemon[0, 0]);
        //}

        //[Test]
        //public void UpdateBoard_ColumnOfSix_MovedTokenReplacedBySecondEvolution()
        //{
        //    _pokemon[2, 0] = new PichuToken();
        //    _pokemon[3, 0] = new PichuToken();
        //    _pokemon[4, 0] = new PichuToken();
        //    _pokemon[5, 0] = new PichuToken();
        //    _pokemon[6, 0] = new PichuToken();
        //    _pokemon[7, 0] = new PichuToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[3, 1] = new PichuToken();
        //    _pokemon[2, 0] = null;
        //    _pokemon[3, 0] = new RaichuToken();
        //    _pokemon[4, 0] = null;
        //    _pokemon[5, 0] = null;
        //    _pokemon[6, 0] = null;
        //    _pokemon[7, 0] = null;
        //    pokemonGrid.updateBoard(3, 0, 3, 1);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}
    }
}
