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
        double timeLeft;
        int score;
        private PokemonGrid _grid;
        public PokemonGrid Grid
        {
            get
            {
                return _grid;
            }
        }
        int NO_TIME_LIMIT;

        public GameState()
        {
            newGame();
        }

        public void makePlay(Point start, Point end)
        {

            // convert points to rows and columns


        }

        public void newGame()
        {
            _grid = new PokemonGrid();
            NO_TIME_LIMIT = -1;
            score = 0;
            timeLeft = 120000; // Default
            Console.Out.WriteLine("a new game was made");
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
