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

        public Boolean areThreeTokensInARow(int row, int col)
        {
            if (col > gridSize - 3 || row > gridSize - 1)
            {
                return false;
            } else {
                Type pokemonType = _pokemon[row,col].GetType();
                if (_pokemon[row, col + 1].GetType() == pokemonType && _pokemon[row, col + 2].GetType() == pokemonType)
                {
                    return true;
                }
            }
            return false;
        }

        public bool areThreeTokensInACol(int row, int col)
        {
            if (row > gridSize - 3 || col > gridSize - 1)
            {
                return false;
            }
            else
            {
                Type pokemonType = _pokemon[row, col].GetType();
                if (_pokemon[row + 1, col].GetType() == pokemonType && _pokemon[row + 2, col].GetType() == pokemonType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
