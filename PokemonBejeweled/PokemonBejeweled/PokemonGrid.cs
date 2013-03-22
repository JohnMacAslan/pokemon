using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    public class PokemonGrid
    {
        public static int gridSize = 8;
        private PokemonToken[,] _pokemonOld;
        public int GamePlayScore { get; set; }
        private PokemonToken[,] _pokemon = new PokemonToken[gridSize, gridSize];
        public PokemonToken[,] Pokemon
        {
            get
            {
                return _pokemon;
            }
            set
            {
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        _pokemon[i, j] = value[i, j];
                    }
                }
            }
        }


        public PokemonGrid()
        {
            Pokemon = new PokemonToken[gridSize, gridSize];
            _pokemonOld = new PokemonToken[gridSize, gridSize];
            GamePlayScore = 0;
        }

        public void updateBoardAlgorithm()
        {
        }

       
    }
}
