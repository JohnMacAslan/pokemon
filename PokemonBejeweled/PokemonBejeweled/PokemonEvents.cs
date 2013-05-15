using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    public class PointsAddedEventArgs : EventArgs
    {
        public int Points { get; set; }

        public PointsAddedEventArgs(int points)
        {
            Points = points;
        }
    }

    public class MakingPlayEventArgs : EventArgs
    {
        private IBasicPokemonToken[,] _pokemonGrid = new IBasicPokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
        internal IBasicPokemonToken[,] PokemonGrid
        {
            get { return _pokemonGrid; }
            set { GridOperations.copyGrid(value, _pokemonGrid); }
        }

        public MakingPlayEventArgs(IBasicPokemonToken[,] pokemonGrid)
        {
            PokemonGrid = pokemonGrid;
        }
    }

    public class ScoreUpdatedEventArgs : EventArgs
    {
        public int Score { get; set; }

        public ScoreUpdatedEventArgs(int score)
        {
            Score = score;
        }
    }

    public class TimeUpdatedEventArgs : EventArgs
    {
        public double Time { get; set; }

        public TimeUpdatedEventArgs(double time)
        {
            Time = time;
        }
    }
}
