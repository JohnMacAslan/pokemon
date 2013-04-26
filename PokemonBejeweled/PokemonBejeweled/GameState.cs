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
        private Timer _countdown = new Timer(1000); // each second
        public Timer Countdown
        {
            get
            {
                return _countdown;
            }
            set
            {
            }
        }
        private double _timeLeft;
        public double TimeLeft
        {
            get
            {
                return _timeLeft;
            }
            set
            {
            }
        }
        private int NO_TIME_LIMIT = -1;
        private int score;
        private PokemonGrid _grid;
        public PokemonGrid Grid
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

        public void makePlay(int rowStart, int colStart, int rowEnd, int colEnd)
        {
        }

        public void newGame()
        {
            _grid = new PokemonGrid();
            score = 0;
          //  _timeLeft = 300; // Default (5 min)
            start();
           // testTimer.Tick += EventHandler
            Console.Out.WriteLine("a new game");
        }

        public void setScore(int value)
        {
            score = value;
        }

     //    public void addToScore(int value)
     //   {
     //       score += value;
     //   }

        public int getScore()
        {
            return _grid.GamePlayScore;
        }

        public void setTime(double time)
        {
            if (time < 0 && time != NO_TIME_LIMIT)
            {
                throw new ArgumentOutOfRangeException();
            }
            _timeLeft = time;
        }

        public double getTime()
        {
            return _timeLeft;
        }

        public void start()
        {
            if (NO_TIME_LIMIT != _timeLeft)
            {
                _countdown.Elapsed += new ElapsedEventHandler(decrementTime);
                _countdown.Start();
            }
        }

        public void stop()
        {
            _countdown.Stop();
            _countdown.Elapsed -= decrementTime;
        }

        public void decrementTime(object sender, ElapsedEventArgs e)
        {
            if (_timeLeft != 0)
            {
                _timeLeft--;
            }
        }
    }
}
