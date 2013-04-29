using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace PokemonBejeweled
{
    public delegate void ScoreUpdatedEventHandler(object source);

    public class GameState
    {
        private Boolean _inMove;
        private int _previousRow;
        private int _previousColumn;
        private Timer _countdown = new Timer(1000);
        public Timer Countdown
        {
            get { return _countdown; }
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
        public static readonly int NO_TIME_LIMIT = -1;
        private int _score = 0;
        public int Score
        {
            get { return _score; }
        }
        public ScoreUpdatedEventHandler ScoreUpdated;
        private PokemonBoard _grid;
        public PokemonBoard Grid
        {
            get { return _grid; }
        }

        public GameState()
        {
            newGame();
            _countdown.Elapsed += new ElapsedEventHandler(decrementTime);
            _countdown.Start();
        }

        public void newGame()
        {
            _grid = new PokemonBoard();
            _grid.PointsAdded += delegate
            {
                OnScoreUpdated();
            };
            _score = 0;
            _inMove = false;
            _previousColumn = 0;
            _previousRow = 0;
            _timeLeft = 120000; // Default
        }

        public void makePlay(int row, int col)
        {
            if (!_inMove)
            {
                _inMove = true;
                _previousRow = row;
                _previousColumn = col;
            }
            else
            {
                _inMove = false;
                if (TimeLeft != 0)
                {
                    _grid.makePlay(row, col, _previousRow, _previousColumn);
                }
            }
        }

        private void OnScoreUpdated()
        {
            _score += _grid.PointsToAdd;
            if (null != ScoreUpdated)
            {
                ScoreUpdated(this);
            }
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
