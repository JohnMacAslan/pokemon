using System;
using PokemonBejeweled.Pokemon;
using PokemonBejeweled;
using NUnit.Framework;

namespace PokemonBejeweledTest
{
    [TestFixture()]
    public class PokemonGridTests
    {
        private int[,] test;
        private PokemonToken[,] pokemonGrid = {{null, null, null, null, null, null, null, null},
                                                  {null, null, null, null, null, null, null, null},
                                                  {null, null, null, null, null, null, null, null},
                                                  {null, null, null, null, null, null, null, null},
                                                  {null, null, null, null, null, null, null, null},
                                                  {null, null, null, null, null, null, null, null},
                                                  {null, null, null, null, null, null, null, null},
                                                  {null, null, null, null, null, null, null, null}};
        private PokemonGrid pokemonActualGrid = new PokemonGrid();

        [Test]
        public void UpdateBoardAlgorithm_ColumnOfThreeOnTopEdge_ColumnOfThreeMarkedAsNull()
        {

        }

        [Test]
        public void UpdateBoardAlgorithm_ColumnOfThreeOnBottomEdge_ColumnOfThreeMarkedAsNull()
        {

        }

        [Test]
        public void UpdateBoardAlgorithm_ColumnOfThreeOnLeftEdge_ColumnOfThreeMarkedAsNull()
        {

        }

        [Test]
        public void TestPokemonGridInitialization()
        {
            Assert.AreSame(pokemonGrid, pokemonActualGrid.getPokemonGrid());
        }

        s


    }
}
