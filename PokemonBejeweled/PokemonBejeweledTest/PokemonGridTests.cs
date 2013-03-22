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
        public void UpdateBoardAlgorithm_ColumnOfThreeOnTopEdge_ColumnMarkedAsNull()
        {
        }

        [Test]
        public void TestPokemonGridInitialization()
        {
            
        }


    }
}
