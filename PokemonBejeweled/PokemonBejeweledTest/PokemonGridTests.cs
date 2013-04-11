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
        private PokemonGrid pokemonGrid;

        [SetUp]
        public void resetPokemonGrid()
        {
            pokemonGrid = new PokemonGrid();
            for (int i = 0; i < 8; i+=2) {
                for(int j = 0; j < 8; j+=2) {
                    _pokemon[i,j] = new BulbasaurToken();
                    _pokemon[i+1,j] = new CharmanderToken();
                    _pokemon[i+1,j+1] = new BulbasaurToken();
                    _pokemon[i,j+1] = new CharmanderToken();
                }
            }
            pokemonGrid.Pokemon = _pokemon;
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
        public void UpdateBoard_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        {
            _pokemon[0, 0] = new DittoToken();
            _pokemon[1, 0] = new DittoToken();
            _pokemon[2, 0] = new DittoToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[1, 0] = null;
            _pokemon[2, 0] = null;
            pokemonGrid.updateBoard(0, 0, 0, 0);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_ColumnOfThreeOnBottomEdge_ColumnMarkedAsNull()
        {
            _pokemon[5, 0] = new DittoToken();
            _pokemon[6, 0] = new DittoToken();
            _pokemon[7, 0] = new DittoToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[5, 0] = null;
            _pokemon[6, 0] = null;
            _pokemon[7, 0] = null;
            pokemonGrid.updateBoard(0, 0, 0, 0);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        {
            _pokemon[3, 0] = new DittoToken();
            _pokemon[4, 0] = new DittoToken();
            _pokemon[5, 0] = new DittoToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[3, 0] = null;
            _pokemon[4, 0] = null;
            _pokemon[5, 0] = null;
            pokemonGrid.updateBoard(0, 0, 0, 0);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        {
            _pokemon[0, 0] = new DittoToken();
            _pokemon[0, 1] = new DittoToken();
            _pokemon[0, 2] = new DittoToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[0, 1] = null;
            _pokemon[0, 2] = null;
            pokemonGrid.updateBoard(0, 0, 0, 0);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_RowOfThreeOnRightEdge_RowMarkedAsNull()
        {
            _pokemon[0, 5] = new DittoToken();
            _pokemon[0, 6] = new DittoToken();
            _pokemon[0, 7] = new DittoToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 5] = null;
            _pokemon[0, 6] = null;
            _pokemon[0, 7] = null;
            pokemonGrid.updateBoard(0, 0, 0, 0);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_RowOfThreeInMiddle_RowMarkedAsNull()
        {
            _pokemon[0, 3] = new DittoToken();
            _pokemon[0, 4] = new DittoToken();
            _pokemon[0, 5] = new DittoToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 3] = null;
            _pokemon[0, 4] = null;
            _pokemon[0, 5] = null;
            pokemonGrid.updateBoard(0, 0, 0, 0);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_RowOfFour_MovedTokenReplacedByFirstEvolution()
        {
            _pokemon[0, 3] = new PichuToken();
            _pokemon[0, 4] = new PichuToken();
            _pokemon[0, 5] = new PichuToken();
            _pokemon[0, 6] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 3] = new PikachuToken();
            _pokemon[0, 4] = null;
            _pokemon[0, 5] = null;
            _pokemon[0, 6] = null;
            pokemonGrid.updateBoard(0, 3, 1, 3);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_ColumnOfFour_MovedTokenReplacedByFirstEvolution()
        {
            _pokemon[3, 0] = new PichuToken();
            _pokemon[4, 0] = new PichuToken();
            _pokemon[5, 0] = new PichuToken();
            _pokemon[6, 0] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[3, 0] = new PikachuToken();
            _pokemon[4, 0] = null;
            _pokemon[5, 0] = null;
            _pokemon[6, 0] = null;
            pokemonGrid.updateBoard(3, 0, 3, 1);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_RowOfFive_MovedTokenReplacedByDitto()
        {
            _pokemon[0, 3] = new PichuToken();
            _pokemon[0, 4] = new PichuToken();
            _pokemon[0, 5] = new PichuToken();
            _pokemon[0, 6] = new PichuToken();
            _pokemon[0, 7] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 3] = new DittoToken();
            _pokemon[0, 4] = null;
            _pokemon[0, 5] = null;
            _pokemon[0, 6] = null;
            _pokemon[0, 7] = null;
            pokemonGrid.updateBoard(0, 3, 1, 3);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_ColumnOfFive_MovedTokenReplacedByDitto()
        {
            _pokemon[3, 0] = new PichuToken();
            _pokemon[4, 0] = new PichuToken();
            _pokemon[5, 0] = new PichuToken();
            _pokemon[6, 0] = new PichuToken();
            _pokemon[7, 0] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[3, 0] = new DittoToken();
            _pokemon[4, 0] = null;
            _pokemon[5, 0] = null;
            _pokemon[6, 0] = null;
            _pokemon[7, 0] = null;
            pokemonGrid.updateBoard(3, 0, 3, 1);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_RowOfSix_MovedTokenReplacedBySecondEvolution()
        {
            _pokemon[0, 2] = new PichuToken();
            _pokemon[0, 3] = new PichuToken();
            _pokemon[0, 4] = new PichuToken();
            _pokemon[0, 5] = new PichuToken();
            _pokemon[0, 6] = new PichuToken();
            _pokemon[0, 7] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 2] = null;
            _pokemon[0, 3] = new RaichuToken();
            _pokemon[0, 4] = null;
            _pokemon[0, 5] = null;
            _pokemon[0, 6] = null;
            _pokemon[0, 7] = null;
            pokemonGrid.updateBoard(0, 3, 1, 3);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateBoard_ColumnOfSix_MovedTokenReplacedBySecondEvolution()
        {
            _pokemon[2, 0] = new PichuToken();
            _pokemon[3, 0] = new PichuToken();
            _pokemon[4, 0] = new PichuToken();
            _pokemon[5, 0] = new PichuToken();
            _pokemon[6, 0] = new PichuToken();
            _pokemon[7, 0] = new PichuToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[2, 0] = null;
            _pokemon[3, 0] = new RaichuToken();
            _pokemon[4, 0] = null;
            _pokemon[5, 0] = null;
            _pokemon[6, 0] = null;
            _pokemon[7, 0] = null;
            pokemonGrid.updateBoard(3, 0, 3, 1);
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void PokemonGrid_PokemonInitializedToNulls()
        {
            _pokemon = new PokemonToken[PokemonGrid.gridSize, PokemonGrid.gridSize];
            Assert.AreEqual((new PokemonGrid()).Pokemon, _pokemon);
        }

        [Test]
        public void PokemonGrid_GamePlayScoreSetTo0()
        {
            Assert.AreEqual(0, pokemonGrid.GamePlayScore);
        }
    }
}
