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
            PokemonToken[,] _newPokemon = new PokemonToken[PokemonGrid.gridSize, PokemonGrid.gridSize];
            PokemonGrid.copyGrid(_pokemon, _newPokemon);
        }

        [Test]
        public void MarkRowsOfSameTokenAsNull_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        {
            _pokemon[0, 0] = new DittoPokemonToken();
            _pokemon[0, 1] = new DittoPokemonToken();
            _pokemon[0, 2] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            PokemonToken[,] _newPokemon = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[0, 1] = null;
            _pokemon[0, 2] = null;
            pokemonGrid.markRowsOfSameTokenAsNull(_newPokemon);
            Assert.AreEqual(_pokemon, _newPokemon);
        }

        [Test]
        public void MarkRowsOfSameTokenAsNull_RowOfThreeOnRightEdge_RowMarkedAsNull()
        {
            _pokemon[0, 5] = new DittoPokemonToken();
            _pokemon[0, 6] = new DittoPokemonToken();
            _pokemon[0, 7] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            PokemonToken[,] _newPokemon = _pokemon;
            _pokemon[0, 5] = null;
            _pokemon[0, 6] = null;
            _pokemon[0, 7] = null;
            pokemonGrid.markRowsOfSameTokenAsNull(_newPokemon);
            Assert.AreEqual(_pokemon, _newPokemon);
        }

        [Test]
        public void MarkRowsOfSameTokenAsNull_RowOfThreeInMiddle_RowMarkedAsNull()
        {
            _pokemon[0, 3] = new DittoPokemonToken();
            _pokemon[0, 4] = new DittoPokemonToken();
            _pokemon[0, 5] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            PokemonToken[,] _newPokemon = _pokemon;
            _pokemon[0, 3] = null;
            _pokemon[0, 4] = null;
            _pokemon[0, 5] = null;
            pokemonGrid.markRowsOfSameTokenAsNull(_newPokemon);
            Assert.AreEqual(_pokemon, _newPokemon);
        }

        [Test]
        public void MarkColumnsOfSameTokenAsNull_ColumnOfThreeOnTop_ColumnMarkedAsNull()
        {
            _pokemon[0, 3] = new DittoPokemonToken();
            _pokemon[1, 3] = new DittoPokemonToken();
            _pokemon[2, 3] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            PokemonToken[,] _newPokemon = _pokemon;
            _pokemon[0, 3] = null;
            _pokemon[1, 3] = null;
            _pokemon[2, 3] = null;
            pokemonGrid.markColumnsOfSameTokenAsNull(_newPokemon);
            Assert.AreEqual(_pokemon, _newPokemon);
        }

        [Test]
        public void MarkColumnsOfSameTokenAsNull_ColumnOfThreeOnBottom_ColumnMarkedAsNull()
        {
            _pokemon[5, 3] = new DittoPokemonToken();
            _pokemon[6, 3] = new DittoPokemonToken();
            _pokemon[7, 3] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            PokemonToken[,] _newPokemon = _pokemon;
            _pokemon[5, 3] = null;
            _pokemon[6, 3] = null;
            _pokemon[7, 3] = null;
            pokemonGrid.markColumnsOfSameTokenAsNull(_newPokemon);
            Assert.AreEqual(_pokemon, _newPokemon);
        }

        [Test]
        public void MarkColumnsOfSameTokenAsNull_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        {
            _pokemon[3, 3] = new DittoPokemonToken();
            _pokemon[4, 3] = new DittoPokemonToken();
            _pokemon[5, 3] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            PokemonToken[,] _newPokemon = _pokemon;
            _pokemon[3, 3] = null;
            _pokemon[4, 3] = null;
            _pokemon[5, 3] = null;
            pokemonGrid.markColumnsOfSameTokenAsNull(_newPokemon);
            Assert.AreEqual(_pokemon, _newPokemon);
        }

        [Test]
        public void UpdateGridAlgorithm_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        {
            _pokemon[0, 3] = new DittoPokemonToken();
            _pokemon[1, 3] = new DittoPokemonToken();
            _pokemon[2, 3] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 3] = null;
            _pokemon[1, 3] = null;
            _pokemon[2, 3] = null;
            pokemonGrid.updateBoardAlgorithm();
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateGridAlgorithm_ColumnOfThreeOnBottomEdge_ColumnMarkedAsNull()
        {
            _pokemon[5, 3] = new DittoPokemonToken();
            _pokemon[6, 3] = new DittoPokemonToken();
            _pokemon[7, 3] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[5, 3] = null;
            _pokemon[6, 3] = null;
            _pokemon[7, 3] = null;
            pokemonGrid.updateBoardAlgorithm();
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateGridAlgorithm_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        {
            _pokemon[3, 3] = new DittoPokemonToken();
            _pokemon[4, 3] = new DittoPokemonToken();
            _pokemon[5, 3] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[3, 3] = null;
            _pokemon[4, 3] = null;
            _pokemon[5, 3] = null;
            pokemonGrid.updateBoardAlgorithm();
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateGridAlgorithm_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        {
            _pokemon[0, 0] = new DittoPokemonToken();
            _pokemon[0, 1] = new DittoPokemonToken();
            _pokemon[0, 2] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 0] = null;
            _pokemon[0, 1] = null;
            _pokemon[0, 2] = null;
            pokemonGrid.updateBoardAlgorithm();
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateGridAlgorithm_RowOfThreeOnRightEdge_RowMarkedAsNull()
        {
            _pokemon[0, 5] = new DittoPokemonToken();
            _pokemon[0, 6] = new DittoPokemonToken();
            _pokemon[0, 7] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 5] = null;
            _pokemon[0, 6] = null;
            _pokemon[0, 7] = null;
            pokemonGrid.updateBoardAlgorithm();
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateGridAlgorithm_RowOfThreeInMiddle_RowMarkedAsNull()
        {
            _pokemon[0, 3] = new DittoPokemonToken();
            _pokemon[0, 4] = new DittoPokemonToken();
            _pokemon[0, 5] = new DittoPokemonToken();
            pokemonGrid.Pokemon = _pokemon;
            _pokemon[0, 3] = null;
            _pokemon[0, 4] = null;
            _pokemon[0, 5] = null;
            pokemonGrid.updateBoardAlgorithm();
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void UpdateGridAlgorithm_NoRowsOrColumnsOfThree_GridUnchanged()
        {
            pokemonGrid.updateBoardAlgorithm();
            Assert.AreEqual(_pokemon, pokemonGrid.Pokemon);
        }

        [Test]
        public void TestIsValidMoveReturnsFalse()
        {
            Assert.False(pokemonGrid.isValidMove(-1, -1, -1, -1));
        }

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
