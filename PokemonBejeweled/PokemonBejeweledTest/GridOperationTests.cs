using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PokemonBejeweled;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweledTest
{
    [TestFixture]
    class GridOperationTests
    {
        private PokemonToken[,] _pokemonGrid = new PokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];

        [SetUp]
        public void resetPokemonGrid()
        {
            for (int i = 0; i < 8; i += 2)
            {
                for (int j = 0; j < 8; j += 2)
                {
                    _pokemonGrid[i, j] = new BulbasaurToken();
                    _pokemonGrid[i + 1, j] = new CharmanderToken();
                    _pokemonGrid[i + 1, j + 1] = new BulbasaurToken();
                    _pokemonGrid[i, j + 1] = new CharmanderToken();
                }
            }
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreHorizontallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(GridOperations.arePiecesAdjacent(1, 1, 1, 2));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreVerticallyAdjacent_ReturnTrue()
        {
            Assert.IsTrue(GridOperations.arePiecesAdjacent(1, 2, 1, 1));
        }

        [Test]
        public void PiecesAreAdjacent_PiecesAreNotAdjacent_ReturnFalse()
        {
            Assert.IsFalse(GridOperations.arePiecesAdjacent(1, 1, 2, 2));
        }

        [Test]
        public void InvertGrid_NoError_GridInverted()
        {
            IBasicPokemonToken[,] pokemonToInvert = new IBasicPokemonToken[3, 3];
            pokemonToInvert[0, 0] = new BulbasaurToken();
            pokemonToInvert[0, 1] = new BulbasaurToken();
            pokemonToInvert[0, 2] = new BulbasaurToken();
            pokemonToInvert[1, 0] = new CharmanderToken();
            pokemonToInvert[1, 1] = new CharmanderToken();
            pokemonToInvert[1, 2] = new CharmanderToken();
            pokemonToInvert[2, 0] = new BulbasaurToken();
            pokemonToInvert[2, 1] = new BulbasaurToken();
            pokemonToInvert[2, 2] = new BulbasaurToken();
            IBasicPokemonToken[,] invertedPokemon = new IBasicPokemonToken[3, 3];
            invertedPokemon[0, 0] = new BulbasaurToken();
            invertedPokemon[0, 1] = new CharmanderToken();
            invertedPokemon[0, 2] = new BulbasaurToken();
            invertedPokemon[1, 0] = new BulbasaurToken();
            invertedPokemon[1, 1] = new CharmanderToken();
            invertedPokemon[1, 2] = new BulbasaurToken();
            invertedPokemon[2, 0] = new BulbasaurToken();
            invertedPokemon[2, 1] = new CharmanderToken();
            invertedPokemon[2, 2] = new BulbasaurToken();
            GridOperations.invertGrid(pokemonToInvert);
            Assert.AreEqual(pokemonToInvert, invertedPokemon);
        }

        [Test]
        [ExpectedException(typeof(ArithmeticException))]
        public void InvertGrid_GridNotSquare_ThrowArithmeticException()
        {
            IBasicPokemonToken[,] pokemonToInvert = new IBasicPokemonToken[3, 4];
            GridOperations.invertGrid(pokemonToInvert);
        }

        [Test]
        public void CopyGrid_ValidGrid_GridCopiedCorrectly()
        {
            PokemonToken[,] newPokemon = new PokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
            GridOperations.copyGrid(_pokemonGrid, newPokemon);
            Assert.AreEqual(_pokemonGrid, newPokemon);
        }

        [Test]
        [ExpectedException(typeof(ArithmeticException))]
        public void CopyGrid_GridRowsMismatched_ThrowArithmeticException()
        {
            IBasicPokemonToken[,] firstGrid = new IBasicPokemonToken[3, 3];
            IBasicPokemonToken[,] secondGrid = new IBasicPokemonToken[4, 3];
            GridOperations.copyGrid(firstGrid, secondGrid);
        }

        [Test]
        [ExpectedException(typeof(ArithmeticException))]
        public void CopyGrid_GridColumnsMismatched_ThrowArithmeticException()
        {
            IBasicPokemonToken[,] firstGrid = new IBasicPokemonToken[3, 3];
            IBasicPokemonToken[,] secondGrid = new IBasicPokemonToken[3, 4];
            GridOperations.copyGrid(firstGrid, secondGrid);
        }
    }
}
