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

        public PokemonGridHistory()
        {
        }

        public void Add(IBasicPokemonToken[,] pokemon)
        {
            _pokemonHistory.Add(pokemon);
        }

        public void RemoveAt(int index)
        {
            _pokemonHistory.RemoveAt(index);
        }

        public IBasicPokemonToken[,] Last()
        {
            if(1 >= _pokemonHistory.Count)
            {
                return _pokemonHistory[_pokemonHistory.Count - 1];
            }
            else
            {
                return _pokemonHistory[0];
            }
        }

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

        public void Clear()
        {
            _pokemonHistory.Clear();
        }
    }
}
