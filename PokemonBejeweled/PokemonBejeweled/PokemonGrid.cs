using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    class PokemonGrid
    {
        private PokemonToken[,] pokemon;
        private PokemonToken[,] pokemonOld;
        private int gamePlayScore;
        private int pokemonGridSize = 8;


        public PokemonGrid()
        {
            pokemon = new PokemonToken[pokemonGridSize, pokemonGridSize];
            pokemonOld = new PokemonToken[pokemonGridSize, pokemonGridSize];
            gamePlayScore = 0;
        }

        private void updateBoardAlgorithm()
        {
        }
    }
}
