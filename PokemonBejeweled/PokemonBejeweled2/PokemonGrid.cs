using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PokemonBejeweled
{
    class PokemonGrid
    {
        public Pokemon.PokemonToken[][] pokemonGrid { get; set; }

        public PokemonGrid()
        {
        }

        public void updateGrid(Point start, Point end)
        {
        }

        public Boolean isValidMove(Point start, Point end)
        {
            return false;
        }

        public Boolean Equals(PokemonGrid pokemonGrid) {
            return false;
        }

        public void resetBoard()
        {
        }

    }
}
