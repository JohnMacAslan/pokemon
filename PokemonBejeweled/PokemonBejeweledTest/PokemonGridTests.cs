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
        public void TestMethod1()
        {
            PokemonToken[] pokemen = {new PokemonBejeweled.Pokemon.BulbasaurToken(), new PokemonBejeweled.Pokemon.CharmanderToken()};
            pokemonGrid = {{new PokemonBejeweled.Pokemon.BulbasaurToken(), new PokemonBejeweled.Pokemon.CharmanderToken()}, {new PokemonBejeweled.Pokemon.BulbasaurToken(), new PokemonBejeweled.Pokemon.CharmanderToken()}};
        }

        [Test]
        public void TestPokemonGridInitialization()
        {
            Assert.AreSame(pokemonGrid, pokemonActualGrid.getPokemonGrid());
        }

        s


    }
}
