using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    class PokemonGridHistory
    {
        private List<IBasicPokemonToken[,]> _pokemonHistory = new List<IBasicPokemonToken[,]>();
        public List<IBasicPokemonToken[,]> PokemonHistory
        {
            get { return _pokemonHistory; }
        }

        /// <summary>
        /// Constructs a new history of IBasicPokemonToken grids. 
        /// </summary>
        public PokemonGridHistory()
        {
        }

        /// <summary>
        /// Appends a new IBasicPokemonToken grid to the history. 
        /// </summary>
        /// <param name="pokemonGrid">A 2-dimensional array of IBasicPokemonTokens to add to the history.</param>
        public void Add(IBasicPokemonToken[,] pokemonGrid)
        {
            _pokemonHistory.Add(pokemonGrid);
        }

        /// <summary>
        /// Removes an IBasicPokemonToken grid from the history. 
        /// </summary>
        /// <param name="index">The index at which to remove a grid. </param>
        public void RemoveAt(int index)
        {
            _pokemonHistory.RemoveAt(index);
        }

        /// <summary>
        /// Returns the last IBasicPokemonToken grid in the history. 
        /// </summary>
        public IBasicPokemonToken[,] Last()
        {
            return _pokemonHistory[_pokemonHistory.Count - 1];
        }

        /// <summary>
        /// Returns the second to last IBasicPokemonToken grid in the history. 
        /// </summary>
        public IBasicPokemonToken[,] NextToLast()
        {
            if (2 <= _pokemonHistory.Count)
            {
                return _pokemonHistory[_pokemonHistory.Count - 2];
            }
            else
            {
                return _pokemonHistory[0];
            }
        }

        /// <summary>
        /// Removes all IBasicPokemonToken grids from the history. 
        /// </summary>
        public void Clear()
        {
            _pokemonHistory.Clear();
        }
    }
}
