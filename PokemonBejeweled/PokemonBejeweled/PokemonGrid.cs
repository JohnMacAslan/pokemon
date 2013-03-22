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
        public PokemonToken[,] pokemon {get; set;}
        private PokemonToken[,] pokemonOld;
        private int gamePlayScore;
        private int pokemonGridSize = 8;


        public PokemonGrid()
        {
            pokemon = new PokemonToken[pokemonGridSize, pokemonGridSize];
            pokemonOld = new PokemonToken[pokemonGridSize, pokemonGridSize];
            gamePlayScore = 0;
        }

        public void updateBoard()
        {
            pokemon = pokemonOld;
        }

        public Boolean isValidMove(int row1, int col1, int row2, int col2)
        {

            return false;
        }

        

        private void updateBoardAlgorithm()
        {
        }

       
    }
}
