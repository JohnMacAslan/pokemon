using System;
using PokemonBejeweled.Pokemon;
using PokemonBejeweled;
using NUnit.Framework;

namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class PokemonGridTests
    {
        private PokemonToken[,] _pokemon = new PokemonToken[PokemonGrid.gridSize, PokemonGrid.gridSize];
        private PokemonToken[,] _newPokemon = new PokemonToken[PokemonGrid.gridSize, PokemonGrid.gridSize];
        private PokemonGrid pokemonGrid;

        [SetUp]
        public void resetPokemonGrid()
        {
            pokemonGrid = new PokemonGrid();
            for (int i = 0; i < 8; i+=2) {
                for (int j = 0; j < 8; j += 2)
                {
                    _pokemon[i, j] = new BulbasaurToken();
                    _newPokemon[i, j] = new BulbasaurToken();
                    _pokemon[i + 1, j] = new CharmanderToken();
                    _newPokemon[i + 1, j] = new CharmanderToken();
                    _pokemon[i + 1, j + 1] = new BulbasaurToken();
                    _newPokemon[i + 1, j + 1] = new BulbasaurToken();
                    _pokemon[i, j + 1] = new CharmanderToken();
                    _newPokemon[i, j + 1] = new CharmanderToken();
                }
            }
            pokemonGrid.Pokemon = _pokemon;
            pokemonGrid.NewPokemon = _newPokemon;
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
            Assert.AreEqual(_newPokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkNullRow_NumberOfSameTokensGreaterThan3_RowMarkNull()
        {
            _newPokemon[0, 0] = null;
            _newPokemon[0, 1] = null;
            _newPokemon[0, 2] = null;
            _newPokemon[0, 3] = null;
            pokemonGrid.markNullRow(0, 0, 4);
            Assert.AreEqual(_newPokemon, pokemonGrid.NewPokemon);
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
            Assert.AreEqual(_newPokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkNullColumn_NumberOfSameTokensGreaterThan3_RowMarkNull()
        {
            _newPokemon[0, 0] = null;
            _newPokemon[1, 0] = null;
            _newPokemon[2, 0] = null;
            _newPokemon[3, 0] = null;
            pokemonGrid.markNullColumn(0, 0, 4);
            Assert.AreEqual(_newPokemon, pokemonGrid.NewPokemon);
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
            _newPokemon[row - 1, col] = null;
            _newPokemon[row - 1, col - 1] = null;
            _newPokemon[row - 1, col + 1] = null;
            _newPokemon[row, col - 1] = null;
            _newPokemon[row, col + 1] = null;
            _newPokemon[row + 1, col] = null;
            _newPokemon[row + 1, col - 1] = null;
            _newPokemon[row + 1, col + 1] = null;
            pokemonGrid.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_newPokemon, pokemonGrid.NewPokemon);
        }

        [Test]
        public void MarkSurroundingTokensNull_IndexOnEdge_SurroundingTokensMarkedNull()
        {
            int row = 4;
            int col = 0;
            _newPokemon[row - 1, col] = null;
            _newPokemon[row - 1, col + 1] = null;
            _newPokemon[row, col + 1] = null;
            _newPokemon[row + 1, col] = null;
            _newPokemon[row + 1, col + 1] = null;
            pokemonGrid.markSurroundingTokensNull(row, col);
            Assert.AreEqual(_newPokemon, pokemonGrid.NewPokemon);
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
                _newPokemon[currentRow, col] = null;
            }
            for (int currentCol = 0; currentCol < PokemonGrid.gridSize; currentCol++)
            {
                _newPokemon[row, currentCol] = null;
            }
            pokemonGrid.markFullRowAndColumnAsNull(row, col);
            Assert.AreEqual(_newPokemon, pokemonGrid.NewPokemon);            
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void MarkFullColumnAndRowAsNull_IndexOutOfRange_ThrowIndexOutOfRangeException()
        {
            pokemonGrid.markFullRowAndColumnAsNull(-12, -12);
        }

        //[Test]
        //public void UpdateBoard_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        //{
        //    _pokemon[0, 0] = new PichuToken();
        //    _pokemon[1, 0] = new PichuToken();
        //    _pokemon[2, 0] = new PichuToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[0, 0] = null;
        //    _pokemon[1, 0] = null;
        //    _pokemon[2, 0] = null;
        //    pokemonGrid.updateBoard(0, 0, 0, 0);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_ColumnOfThreeOnBottomEdge_ColumnMarkedAsNull()
        //{
        //    _pokemon[5, 0] = new DittoToken();
        //    _pokemon[6, 0] = new DittoToken();
        //    _pokemon[7, 0] = new DittoToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[5, 0] = null;
        //    _pokemon[6, 0] = null;
        //    _pokemon[7, 0] = null;
        //    pokemonGrid.updateBoard(0, 0, 0, 0);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        //{
        //    _pokemon[3, 0] = new DittoToken();
        //    _pokemon[4, 0] = new DittoToken();
        //    _pokemon[5, 0] = new DittoToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[3, 0] = null;
        //    _pokemon[4, 0] = null;
        //    _pokemon[5, 0] = null;
        //    pokemonGrid.updateBoard(0, 0, 0, 0);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        //{
        //    _pokemon[0, 0] = new DittoToken();
        //    _pokemon[0, 1] = new DittoToken();
        //    _pokemon[0, 2] = new DittoToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[0, 0] = null;
        //    _pokemon[0, 1] = null;
        //    _pokemon[0, 2] = null;
        //    pokemonGrid.updateBoard(0, 0, 0, 0);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_RowOfThreeOnRightEdge_RowMarkedAsNull()
        //{
        //    _pokemon[0, 5] = new DittoToken();
        //    _pokemon[0, 6] = new DittoToken();
        //    _pokemon[0, 7] = new DittoToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[0, 5] = null;
        //    _pokemon[0, 6] = null;
        //    _pokemon[0, 7] = null;
        //    pokemonGrid.updateBoard(0, 0, 0, 0);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_RowOfThreeInMiddle_RowMarkedAsNull()
        //{
        //    _pokemon[0, 3] = new DittoToken();
        //    _pokemon[0, 4] = new DittoToken();
        //    _pokemon[0, 5] = new DittoToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[0, 3] = null;
        //    _pokemon[0, 4] = null;
        //    _pokemon[0, 5] = null;
        //    pokemonGrid.updateBoard(0, 0, 0, 0);
        //    Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        //}

        //[Test]
        //public void UpdateBoard_RowOfFour_MovedTokenReplacedByFirstEvolution()
        //{
        //    _pokemon[0, 3] = new PichuToken();
        //    _pokemon[0, 4] = new PichuToken();
        //    _pokemon[0, 5] = new PichuToken();
        //    _pokemon[0, 6] = new PichuToken();
        //    pokemonGrid.Pokemon = _pokemon;
        //    _pokemon[1, 3] = new PichuToken();
        //    _pokemon[0, 3] = new PikachuToken();
        //    _pokemon[0, 4] = null;
        //    _pokemon[0, 5] = null;
        //    _pokemon[0, 6] = null;
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
        //    Assert.AreEqual(_pokemon[0,0], pokemonGrid.Pokemon[0,0]);
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
