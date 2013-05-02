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
    class PokemonGridHistoryTests
    {
        private PokemonGridHistory _pokemonGridHistory;

        [SetUp]
        public void ResetHistory()
        {
            _pokemonGridHistory = new PokemonGridHistory();
        }

        [Test]
        public void Add_AddGridToGridHistory()
        {
            IBasicPokemonToken[,] gridToAdd = new IBasicPokemonToken[3, 3];
            _pokemonGridHistory.Add(gridToAdd);
            Assert.AreSame(gridToAdd, _pokemonGridHistory.PokemonHistory[0]);
        }

        [Test]
        public void RemoveAt_RemovesGridAtSpecifiedIndex()
        {
            IBasicPokemonToken[,] gridOne = new IBasicPokemonToken[3, 3];
            IBasicPokemonToken[,] gridTwo = new IBasicPokemonToken[3, 3];
            _pokemonGridHistory.Add(gridOne);
            _pokemonGridHistory.Add(gridTwo);
            _pokemonGridHistory.RemoveAt(0);
            Assert.AreSame(gridTwo, _pokemonGridHistory.PokemonHistory[0]);
        }

        [Test]
        public void Last_ReturnLastGrid()
        {
            IBasicPokemonToken[,] gridOne = new IBasicPokemonToken[3, 3];
            IBasicPokemonToken[,] gridTwo = new IBasicPokemonToken[3, 3];
            _pokemonGridHistory.Add(gridOne);
            _pokemonGridHistory.Add(gridTwo);
            Assert.AreSame(gridTwo, _pokemonGridHistory.Last());
        }

        [Test]
        public void NextToLast_MoreThanOneGrid_ReturnNextToLastGrid()
        {
            IBasicPokemonToken[,] gridOne = new IBasicPokemonToken[3, 3];
            IBasicPokemonToken[,] gridTwo = new IBasicPokemonToken[3, 3];
            _pokemonGridHistory.Add(gridOne);
            _pokemonGridHistory.Add(gridTwo);
            Assert.AreSame(gridOne, _pokemonGridHistory.NextToLast());
        }

        [Test]
        public void NextToLast_OneGrid_ReturnFirstGrid()
        {
            IBasicPokemonToken[,] gridOne = new IBasicPokemonToken[3, 3];
            _pokemonGridHistory.Add(gridOne);
            Assert.AreSame(gridOne, _pokemonGridHistory.NextToLast());
        }

        [Test]
        public void Clear_AllGridsRemoved()
        {
            IBasicPokemonToken[,] gridOne = new IBasicPokemonToken[3, 3];
            IBasicPokemonToken[,] gridTwo = new IBasicPokemonToken[3, 3];
            _pokemonGridHistory.Add(gridOne);
            _pokemonGridHistory.Add(gridTwo);
            _pokemonGridHistory.Clear();
            Assert.AreEqual(0, _pokemonGridHistory.PokemonHistory.Count);
        }
    }
}
