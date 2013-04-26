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

        public PokemonGridHistory()
        {
        }

        public void Add(IBasicPokemonToken[,] pokemon)
        {
            _pokemonHistory.Add(pokemon);
        }

        public IBasicPokemonToken[,] Last()
        {
            return _pokemonHistory[_pokemonHistory.Count - 1];
        }

        public void Clear()
        {
            _pokemonHistory.Clear();
        }
    }
}
