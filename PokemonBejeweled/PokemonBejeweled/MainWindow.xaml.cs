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
        private GameState _gameState = new GameState();
        private System.Windows.Threading.DispatcherTimer _countdown = new System.Windows.Threading.DispatcherTimer();
        private bool _paused = false;

        public MainWindow()
        {
            InitializeComponent();
            setUpGridBoard();
            newGame();
            NewGameButton.Click += delegate { newGame(); };
            PauseButton.Click += delegate { pauseGame(); };
            UndoButton.Click += delegate { _gameState.Board.undoPlay(); };
            HintButton.Click += delegate
            {
                int rowHint;
                int colHint;
                if (_gameState.Board.areMovesLeft(out rowHint, out colHint))
                {
                    ((UIElement)GridBoard.Children[rowHint * 8 + colHint]).Focus();
                }
                else
                {
                    HintButton.Content = "No moves!";
                }
            };
            QuitGameButton.Click += delegate { this.Close(); };
            _countdown.Tick += new EventHandler(updateTimer);
            _countdown.Interval = new TimeSpan(0, 0, 1);
            _countdown.Start();
        }

        private void newGame()
        {
            _gameState.newGame();
            resetTimer();
            updateGridBoard();
            _gameState.Board.BoardDirtied += new BoardDirtiedEventHandler(delegate { updateGridBoard(); });
            _gameState.ScoreUpdated += new ScoreUpdatedEventHandler(delegate { updateScore(); });
        }

        private void pauseGame()
        {
            if (!_paused)
            {
                _paused = true;
                _gameState.Countdown.Stop();
                PauseButton.Content = "Unpause";
                GridBoard.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                _paused = false;
                _gameState.Countdown.Start();
                PauseButton.Content = "Pause";
                GridBoard.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void setUpGridBoard()
        {
            double buttonHeight = GridBoard.Height / PokemonBoard.gridSize;
            double buttonWidth = GridBoard.Width / PokemonBoard.gridSize;
            for (int row = 0; row < PokemonBoard.gridSize; row++)
            {
                for (int col = 0; col < PokemonBoard.gridSize; col++)
                {
                    GridButton newButton = new GridButton(_gameState, row, col);
                    newButton.Height = buttonHeight;
                    newButton.Width = buttonWidth;
                    GridBoard.Children.Add(newButton);
                }
            }
        }

        private void updateGridBoard()
        {
            GridButton currentButton;
            System.Collections.IEnumerator buttonEnumerator = GridBoard.Children.GetEnumerator();
            for (int r = 0; r < PokemonBoard.gridSize; r++)
            {
                for (int c = 0; c < PokemonBoard.gridSize; c++)
                {
                    buttonEnumerator.MoveNext();
                    currentButton = (GridButton)buttonEnumerator.Current;
                    if (null == _gameState.Board.PokemonGrid[r, c])
                    {
                        currentButton.Background = Brushes.Black;
                    }
                    else
                    {
                        currentButton.Background = (_gameState.Board.PokemonGrid[r, c].getPokemonPicture());
                    }
                }
            }

            HintButton.Content = "Hint";
            DependencyObject scope = FocusManager.GetFocusScope(this);
            FocusManager.SetFocusedElement(scope, this);
            GridBoard.Dispatcher.Invoke(delegate() { System.Threading.Thread.Sleep(500); }, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void updateScore()
        {
            ScoreboardLabel.Content = _gameState.Score;
            ScoreboardLabel.Dispatcher.Invoke(delegate() { }, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void updateTimer(object sender, EventArgs e)
        {
            if (0 == _gameState.TimeLeft)
            {
                TimerLabel.Foreground = Brushes.White;
                TimerLabel.Content = "DONE!";
            }
            else if (GameState.NO_TIME_LIMIT == _gameState.TimeLeft)
            {
                TimerLabel.Content = "--:--";
            }
            else
            {
                if (10 > _gameState.TimeLeft)
                {
                    TimerLabel.Foreground = Brushes.Red;
                }
                else if (30 > _gameState.TimeLeft)
                {
                    TimerLabel.Foreground = Brushes.Yellow;
                }
                else
                {
                    TimerLabel.Foreground = Brushes.Green;
                }
                TimeSpan t = TimeSpan.FromSeconds(_gameState.TimeLeft);
                TimerLabel.Content = string.Format("{0}:{1:D2}", t.Minutes, t.Seconds);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void resetTimer()
        {
            if ((bool)OneMinuteRadio.IsChecked)
            {
                _gameState.TimeLeft = 60;
            }
            else if ((bool)FiveMinuteRadio.IsChecked)
            {
                _gameState.TimeLeft = 300;
            }
            else if ((bool)TenMinuteRadio.IsChecked)
            {
                _gameState.TimeLeft = 600;
            }
            else if ((bool)UnlimitedRadio.IsChecked)
            {
                _gameState.TimeLeft = GameState.NO_TIME_LIMIT;
            }
            else
            {
                _gameState.TimeLeft = 12000;
            }
        }
    }
}
