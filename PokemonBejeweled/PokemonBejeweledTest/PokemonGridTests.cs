using System;
using PokemonBejeweled.Pokemon;
using PokemonBejeweled;
using NUnit.Framework;

namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class PokemonGridTests
    {
        private PokemonToken[,] pokemonGrid = new PokemonToken[PokemonGrid.gridSize, PokemonGrid.gridSize];
        private PokemonGrid pokemonActualGrid;

        [SetUp]
        public void resetPokemonGrid()
        {
            pokemonActualGrid = new PokemonGrid();
            for (int i = 0; i < 8; i+=2) {
                for(int j = 0; j < 8; j+=2) {
                    pokemonGrid[i,j] = new BulbasaurToken();
                    pokemonGrid[i+1,j] = new CharmanderToken();
                    pokemonGrid[i+1,j+1] = new BulbasaurToken();
                    pokemonGrid[i,j+1] = new CharmanderToken();
                }
            }
            pokemonActualGrid.Pokemon = pokemonGrid;
        }

        [Test]
        public void UpdateGridAlgorithm_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        {
            pokemonGrid[0, 3] = new DittoPokemonToken();
            pokemonGrid[1, 3] = new DittoPokemonToken();
            pokemonGrid[2, 3] = new DittoPokemonToken();
            pokemonActualGrid.Pokemon = pokemonGrid;
            pokemonGrid[0, 3] = null;
            pokemonGrid[1, 3] = null;
            pokemonGrid[2, 3] = null;
            pokemonActualGrid.updateBoardAlgorithm();
            Assert.AreEqual(pokemonActualGrid.Pokemon, pokemonGrid);
        }

        [Test]
        public void UpdateGridAlgorithm_ColumnOfThreeOnBottomEdge_ColumnMarkedAsNull()
        {
            pokemonGrid[5, 3] = new DittoPokemonToken();
            pokemonGrid[6, 3] = new DittoPokemonToken();
            pokemonGrid[7, 3] = new DittoPokemonToken();
            pokemonActualGrid.Pokemon = pokemonGrid;
            pokemonGrid[5, 3] = null;
            pokemonGrid[6, 3] = null;
            pokemonGrid[7, 3] = null;
            pokemonActualGrid.updateBoardAlgorithm();
            Assert.AreEqual(pokemonActualGrid.Pokemon, pokemonGrid);
        }

        [Test]
        public void UpdateGridAlgorithm_ColumnOfThreeInMiddle_ColumnMarkedAsNull()
        {
            pokemonGrid[3, 3] = new DittoPokemonToken();
            pokemonGrid[4, 3] = new DittoPokemonToken();
            pokemonGrid[5, 3] = new DittoPokemonToken();
            pokemonActualGrid.Pokemon = pokemonGrid;
            pokemonGrid[3, 3] = null;
            pokemonGrid[4, 3] = null;
            pokemonGrid[5, 3] = null;
            pokemonActualGrid.updateBoardAlgorithm();
            Assert.AreEqual(pokemonActualGrid.Pokemon, pokemonGrid);
        }

        [Test]
        public void UpdateGridAlgorithm_RowOfThreeOnLeftEdge_RowMarkedAsNull()
        {
            pokemonGrid[3, 0] = new DittoPokemonToken();
            pokemonGrid[3, 1] = new DittoPokemonToken();
            pokemonGrid[3, 2] = new DittoPokemonToken();
            pokemonActualGrid.Pokemon = pokemonGrid;
            pokemonGrid[3, 0] = null;
            pokemonGrid[3, 1] = null;
            pokemonGrid[3, 2] = null;
            pokemonActualGrid.updateBoardAlgorithm();
            Assert.AreEqual(pokemonActualGrid.Pokemon, pokemonGrid);
        }

        [Test]
        public void UpdateGridAlgorithm_RowOfThreeOnRightEdge_RowMarkedAsNull()
        {
            pokemonGrid[3, 5] = new DittoPokemonToken();
            pokemonGrid[3, 6] = new DittoPokemonToken();
            pokemonGrid[3, 7] = new DittoPokemonToken();
            pokemonActualGrid.Pokemon = pokemonGrid;
            pokemonGrid[3, 5] = null;
            pokemonGrid[3, 6] = null;
            pokemonGrid[3, 7] = null;
            pokemonActualGrid.updateBoardAlgorithm();
            Assert.AreEqual(pokemonActualGrid.Pokemon, pokemonGrid);
        }

        [Test]
        public void UpdateGridAlgorithm_RowOfThreeInMiddle_RowMarkedAsNull()
        {
            pokemonGrid[3, 3] = new DittoPokemonToken();
            pokemonGrid[3, 4] = new DittoPokemonToken();
            pokemonGrid[3, 5] = new DittoPokemonToken();
            pokemonActualGrid.Pokemon = pokemonGrid;
            pokemonGrid[3, 3] = null;
            pokemonGrid[3, 4] = null;
            pokemonGrid[3, 5] = null;
            pokemonActualGrid.updateBoardAlgorithm();
            Assert.AreEqual(pokemonActualGrid.Pokemon, pokemonGrid);
        }

        [Test]
        public void UpdateGridAlgorithm_NoRowsOrColumnsOfThree_GridUnchanged()
        {
            pokemonActualGrid.updateBoardAlgorithm();
            Assert.AreEqual(pokemonActualGrid.Pokemon, pokemonGrid);
        }

        [Test]
        public void AreThreeTokensInARow_InvalidColIndex_ReturnFalse()
        {
            Assert.IsFalse(pokemonActualGrid.areThreeTokensInARow(3, PokemonGrid.gridSize)); 
        }

        [Test]
        public void AreThreeTokensInARow_InvalidRowIndex_ReturnFalse()
        {
            Assert.IsFalse(pokemonActualGrid.areThreeTokensInARow(PokemonGrid.gridSize, 3)); 
        }

        [Test]
        public void AreThreeTokensInARow_ThreeInARow_ReturnTrue()
        {
            pokemonGrid[3, 3] = new DittoPokemonToken();
            pokemonGrid[3, 4] = new DittoPokemonToken();
            pokemonGrid[3, 5] = new DittoPokemonToken();
            pokemonActualGrid.Pokemon = pokemonGrid;
            Assert.IsTrue(pokemonActualGrid.areThreeTokensInARow(3, 3));
        }

        [Test]
        public void AreThreeTokensInARow_ThreeNotInARow_ReturnFalse()
        {
            Assert.IsFalse(pokemonActualGrid.areThreeTokensInARow(3, 3));
        }

        [Test]
        public void AreThreeTokensInARow_TokenIsNull_ReturnFalse()
        {
            pokemonGrid[3, 3] = null;
            pokemonActualGrid.Pokemon = pokemonGrid;
            Assert.IsFalse(pokemonActualGrid.areThreeTokensInARow(3, 3));
        }

        [Test]
        public void AreThreeTokensInACol_InvalidColIndex_ReturnFalse()
        {
            Assert.IsFalse(pokemonActualGrid.areThreeTokensInACol(3, PokemonGrid.gridSize));
        }

        [Test]
        public void AreThreeTokensInACol_InvalidRowIndex_ReturnFalse()
        {
            Assert.IsFalse(pokemonActualGrid.areThreeTokensInACol(PokemonGrid.gridSize, 3));
        }

        [Test]
        public void AreThreeTokensInACol_ThreeInACol_ReturnTrue()
        {
            pokemonGrid[3, 3] = new DittoPokemonToken();
            pokemonGrid[4, 3] = new DittoPokemonToken();
            pokemonGrid[5, 3] = new DittoPokemonToken();
            pokemonActualGrid.Pokemon = pokemonGrid;
            Assert.IsTrue(pokemonActualGrid.areThreeTokensInACol(3, 3));
        }

        [Test]
        public void AreThreeTokensInACol_ThreeNotInACol_ReturnFalse()
        {
            Assert.IsFalse(pokemonActualGrid.areThreeTokensInACol(3, 3));
        }

        [Test]
        public void AreThreeTokensInACol_TokenIsNull_ReturnFalse()
        {
            pokemonGrid[3, 3] = null;
            pokemonActualGrid.Pokemon = pokemonGrid;
            Assert.IsFalse(pokemonActualGrid.areThreeTokensInACol(3, 3));
        }

        [Test]

        public void TestIsValidMoveReturnsFalse()
        {
            Assert.False(pokemonActualGrid.isValidMove(-1, -1, -1, -1));
        }

        public void PokemonGrid_PokemonInitializedToNulls()
        {
            pokemonGrid = new PokemonToken[PokemonGrid.gridSize, PokemonGrid.gridSize];
            Assert.AreEqual((new PokemonGrid()).Pokemon, pokemonGrid);
        }

        [Test]
        public void PokemonGrid_GamePlayScoreSetTo0()
        {
            Assert.AreEqual(0, pokemonActualGrid.GamePlayScore);
        }



       
    }
}
