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
        public void TestMethod1()
        {
            PokemonToken[] pokemen = {new PokemonBejeweled.Pokemon.BulbasaurToken(), new PokemonBejeweled.Pokemon.CharmanderToken()};
            pokemonGrid = {{new PokemonBejeweled.Pokemon.BulbasaurToken(), new PokemonBejeweled.Pokemon.CharmanderToken()}, {new PokemonBejeweled.Pokemon.BulbasaurToken(), new PokemonBejeweled.Pokemon.CharmanderToken()}};
        }

        [Test]
        public void TestPokemonGridInitialization()
        {
            
        }


    }
}
