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
        Timer countdown = new Timer(1000);
        double timeLeft;
        int score;
        //PokemonGrid grid;
        int NO_TIME_LIMIT;

        public GameState()
        {
            newGame();
        }

        public void makePlay(Point start, Point end)
        {
            //   if (grid.isValidMove(start, end))
            //   {
            //       grid.updateBoard(start, end);
            //       score += grid.lastPlayScore();
            //   }
        }

        public void newGame() 
        {
            // make grid 
            score = 0;
            timeLeft = 120000; // Default
        }

        public void setTime(double time)
        {
            timeLeft = time;
        }

        public double getTimer()
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
