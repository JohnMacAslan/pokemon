using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;


namespace PokemonBejeweled
{
    class GameBoard
    {
        private PokemonGrid pokemonGrid = new PokemonGrid();
        private Timer countdown = new Timer(1000);
        int timeLeft = 120000;
        private int score;

        public GameBoard()
        {
        }

        public void newGame()
        {
        }

        public void setTime(int seconds)
        {
        }

        public void makePlay(Point start, Point end)
        {
        }
        
    }
}
