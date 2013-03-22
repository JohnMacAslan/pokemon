using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonBejeweled.Pokemon;
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
            
        }


    }
}
