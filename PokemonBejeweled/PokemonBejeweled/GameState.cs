using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace PokemonBejeweled
{
    public class GameState
    {
        private Timer _countdown = new Timer(1000);
        public Timer Countdown
        {
            get { return _countdown; }
            set { }
        }
        private double _timeLeft;
        public double TimeLeft
        {
            get { return _timeLeft; }
            set
            {
                if (value < 0 && value != NO_TIME_LIMIT)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _timeLeft = value;
            }
        }
        private int NO_TIME_LIMIT = -1;
        private int _score = 0;
        public int Score
        {
            get { return _score; }
            set { }
        }
        private PokemonBoard _grid;
        public PokemonBoard Grid
        {
            get
            {
                return _grid;
            }
        }

        public GameState()
        {
            newGame();
        }

        public void newGame()
        {
            _grid = new PokemonBoard();
            _score = 0;
            _timeLeft = 120000; // Default
            _countdown.Start();
        }

        public void decrementTime(object sender, ElapsedEventArgs e)
        {
            if (_timeLeft != 0 && NO_TIME_LIMIT != _timeLeft)
            {
                _timeLeft--;
            }
        }
    }
}
