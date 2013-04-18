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
        public Timer countdown = new Timer(1000);
        private double timeLeft;
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
            timeLeft = 120000; // Default
            Console.Out.WriteLine("a new game");
            _grid.generateGrid();
        }

        public void setScore(int value)
        {
            score = value;
        }

        public void addToScore(int value)
        {
            score += value;
        }

        public int getScore()
        {
            return score;
        }

        public void setTime(double time)
        {
            if (time < 0 && time != NO_TIME_LIMIT)
            {
                throw new ArgumentOutOfRangeException();
            }
            timeLeft = time;
        }

        public double getTime()
        {
            return timeLeft;
        }

        public void start()
        {
            if (NO_TIME_LIMIT != timeLeft)
            {
                countdown.Elapsed += decrementTime;
            }
        }

        public void stop()
        {
            countdown.Elapsed -= decrementTime;
        }

        public void decrementTime(object sender, ElapsedEventArgs e)
        {
            if (timeLeft == 0)
            {
                timeLeft--;
            }
        }
    }
}
