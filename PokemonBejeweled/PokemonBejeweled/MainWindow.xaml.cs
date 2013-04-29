using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameState gameState = new GameState();
        private System.Windows.Controls.Primitives.UniformGrid gridBoard;
        private System.Windows.Controls.Label timerLabel;
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        private System.Windows.Controls.RadioButton oneMin;
        private System.Windows.Controls.RadioButton fiveMin;
        private System.Windows.Controls.RadioButton tenMin;
        private System.Windows.Controls.RadioButton noTimeLimit;
        private System.Windows.Controls.Label scoreboard;

        public MainWindow()
        {
            InitializeComponent();
            gridBoard = this.GridBoard;
            timerLabel = this.TimerLabel;
            oneMin = this.oneMinute;
            fiveMin = this.fiveMinute;
            tenMin = this.tenMinute;
            noTimeLimit = this.unlimitedTime;
            scoreboard = this.ScoreboardLabel;
            setUpGridBoard();
            newGame();
            NewGameButton.Click += delegate { newGame(); };
            QuitGameButton.Click += delegate { this.Close(); };
            this.MouseRightButtonDown += delegate
            {
                DependencyObject scope = FocusManager.GetFocusScope(this);
                FocusManager.SetFocusedElement(scope, this);
            };
            timer.Tick += new EventHandler(updateTimer);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void newGame()
        {
            gameState.newGame();
            resetTimer();
            updateGridBoard();
            gameState.Grid.BoardDirtied += new BoardDirtiedEventHandler(delegate { updateGridBoard(); });
            gameState.ScoreUpdated += new ScoreUpdatedEventHandler(delegate { updateScore(); });
        }

        private void setUpGridBoard()
        {
            double buttonHeight = gridBoard.Height / PokemonBoard.gridSize;
            double buttonWidth = gridBoard.Width / PokemonBoard.gridSize;
            for (int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    GridButton newButton = new GridButton(gameState, row, col);
                    newButton.Height = buttonHeight;
                    newButton.Width = buttonWidth;
                    gridBoard.Children.Add(newButton);
                }
            }
        }

        private void updateGridBoard()
        {
            GridButton currentButton;
            System.Collections.IEnumerator buttonEnumerator = gridBoard.Children.GetEnumerator();
            for (int r = 0; r < PokemonBoard.gridSize; r++)
            {
                for (int c = 0; c < PokemonBoard.gridSize; c++)
                {
                    buttonEnumerator.MoveNext();
                    currentButton = (GridButton)buttonEnumerator.Current;
                    if (null == gameState.Grid.PokemonGrid[r, c])
                    {
                        currentButton.Background = Brushes.Black;
                    }
                    else
                    {
                        currentButton.Background = (gameState.Grid.PokemonGrid[r, c].getPokemonPicture());
                    }
                }
            }

            DependencyObject scope = FocusManager.GetFocusScope(this);
            FocusManager.SetFocusedElement(scope, this);
            gridBoard.Dispatcher.Invoke(delegate() { System.Threading.Thread.Sleep(500); }, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void updateScore()
        {
            scoreboard.Content = gameState.Score;
            scoreboard.Dispatcher.Invoke(delegate() { }, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void updateTimer(object sender, EventArgs e)
        {
            if (0 == gameState.TimeLeft)
            {
                timerLabel.Content = "GAME OVER";
            }
            else if (GameState.NO_TIME_LIMIT == gameState.TimeLeft)
            {
                timerLabel.Content = "No time limit";
            }
            else
            {
                TimeSpan t = TimeSpan.FromSeconds(gameState.TimeLeft);
                timerLabel.Content = string.Format("{0}:{1:D2}", t.Minutes, t.Seconds);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void resetTimer()
        {
            if ((bool)oneMin.IsChecked)
            {
                gameState.TimeLeft = 60;
            }
            else if ((bool)fiveMin.IsChecked)
            {
                gameState.TimeLeft = 300;
            }
            else if ((bool)tenMin.IsChecked)
            {
                gameState.TimeLeft = 600;
            }
            else if ((bool)noTimeLimit.IsChecked)
            {
                gameState.TimeLeft = GameState.NO_TIME_LIMIT;
            }
            else
            {
                gameState.TimeLeft = 12000;
            }
        }
    }
}
