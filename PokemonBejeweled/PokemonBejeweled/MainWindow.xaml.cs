using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ResourceManager _resourceManager = new ResourceManager("PokemonBejeweled.en-US", Assembly.GetExecutingAssembly());
        private GameState _gameState = new GameState();
        public GameState GameState
        {
            get { return _gameState; }
            set { _gameState = value; }
        }
        private System.Windows.Threading.DispatcherTimer _countdown = new System.Windows.Threading.DispatcherTimer();
        private bool _paused = false;
        public bool Paused
        {
            get { return _paused; }
        }
        private Window _instructionsWindow = new Window();
        private TextBlock _instructionsText = new TextBlock();
        private ImageBrush _pokeball = (new PokeballToken()).getPokemonPicture();

        public MainWindow()
        {
            InitializeComponent();
            setUpInstructionWindow();
            setUpGridBoard();
            newGame(this, null);
            setLocalizedText();
            setUpLanguageButtons();
            NewGameButton.Click += newGame;
            PauseButton.Click += pauseGame;
            UndoButton.Click += delegate { _gameState.Board.undoPlay(); };
            HintButton.Click += hint;
            QuitGameButton.Click += delegate { this.Close(); };
            Closing += delegate
            {
                _instructionsWindow.Closing -= new CancelEventHandler(HideInsteadOfClose);
                _instructionsWindow.Close(); 
            };
            InstructionsButton.Click += openInstructions;
            _countdown.Tick += new EventHandler(updateTimer);
            _countdown.Interval = new TimeSpan(0, 0, 1);
            _countdown.Start();
        }

        private void setUpInstructionWindow()
        {
            _instructionsText.TextWrapping = System.Windows.TextWrapping.Wrap;
            _instructionsText.Text = _resourceManager.GetString("Instruction_Content");
            _instructionsWindow.Closing += new CancelEventHandler(HideInsteadOfClose);
            _instructionsWindow.Width = 450;
            _instructionsWindow.Height = 563;
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
                        currentButton.Background = _pokeball;
                    }
                    else
                    {
                        currentButton.Background = (_gameState.Board.PokemonGrid[r, c].getPokemonPicture());
                    }
                }
            }

            HintButton.Content = _resourceManager.GetString("Hint");
            DependencyObject scope = FocusManager.GetFocusScope(this);
            FocusManager.SetFocusedElement(scope, this);
            GridBoard.Dispatcher.Invoke(delegate() { Thread.Sleep(500); }, DispatcherPriority.Render);
        }

        private void setLocalizedText()
        {
            try
            {
                this.Title = _resourceManager.GetString("Window_Title");
                MainMenu.Header = _resourceManager.GetString("Main_Menu");
                GameTitle.Text = _resourceManager.GetString("Game_Title");
                NewGameButton.Content = _resourceManager.GetString("New_Game");
                PauseButton.Content = _resourceManager.GetString("Pause");
                HintButton.Content = _resourceManager.GetString("Hint");
                UndoButton.Content = _resourceManager.GetString("Undo");
                QuitGameButton.Content = _resourceManager.GetString("Quit");
                InstructionsButton.Content = _resourceManager.GetString("Instructions");
                OneMinuteRadio.Content = _resourceManager.GetString("One_Minute");
                FiveMinuteRadio.Content = _resourceManager.GetString("Five_Minutes");
                TenMinuteRadio.Content = _resourceManager.GetString("Ten_Minutes");
                UnlimitedRadio.Content = _resourceManager.GetString("Unlimited");
                ScoreboardExpander.Header = _resourceManager.GetString("Scoreboard");
                TimeLeftExpander.Header = _resourceManager.GetString("Time_Left");
                LanguageExpander.Header = _resourceManager.GetString("Language");
                _instructionsText.Text = _resourceManager.GetString("Instruction_Content");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void setUpLanguageButtons()
        {
            EnglishButton.Click += delegate
            {
                _resourceManager = new ResourceManager("PokemonBejeweled.en-US", Assembly.GetExecutingAssembly());
                setLocalizedText();
            };
            JapaneseButton.Click += delegate
            {
                _resourceManager = new ResourceManager("PokemonBejeweled.jp-JP", Assembly.GetExecutingAssembly());
                setLocalizedText();
            };
        }

        private void updateScore()
        {
            ScoreboardLabel.Content = _gameState.Score;
            ScoreboardLabel.Dispatcher.Invoke(delegate() { }, DispatcherPriority.Render);
        }

        public void resetTimer()
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
                _gameState.TimeLeft = 600;
            }
        }

        private void newGame(object sender, RoutedEventArgs e)
        {
            _gameState.newGame();
            resetTimer();
            updateScore();
            updateGridBoard();
            _gameState.Board.BoardDirtied += new BoardDirtiedEventHandler(delegate { updateGridBoard(); });
            _gameState.ScoreUpdated += new ScoreUpdatedEventHandler(delegate { updateScore(); });
        }

        public void pauseGame(object sender, RoutedEventArgs e)
        {
            if (!_paused)
            {
                _paused = true;
                _gameState.Countdown.Stop();
                PauseButton.Content = _resourceManager.GetString("Resume");
                GridBoard.Visibility = Visibility.Hidden;
            }
            else
            {
                _paused = false;
                _gameState.Countdown.Start();
                PauseButton.Content = _resourceManager.GetString("Pause");
                GridBoard.Visibility = Visibility.Visible;
            }
        }

        public void hint(object sender, RoutedEventArgs e)
        {
            int rowHint, colHint;
            if (_gameState.Board.areMovesLeft(out rowHint, out colHint))
            {
                ((UIElement)GridBoard.Children[rowHint * 8 + colHint]).Focus();
            }
            else
            {
                HintButton.Content = _resourceManager.GetString("No_Moves");
            }
        }

        private void HideInsteadOfClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _instructionsWindow.Visibility = Visibility.Hidden;
        }

        private void openInstructions(object sender, RoutedEventArgs e)
        {
            _instructionsWindow.Visibility = Visibility.Visible;
            _instructionsWindow.Content = _instructionsText;
        }

        private void updateTimer(object sender, EventArgs e)
        {
            TimerLabel.Foreground = Brushes.Green;
            if (0 == _gameState.TimeLeft)
            {
                TimerLabel.Foreground = Brushes.White;
                TimerLabel.Content = _resourceManager.GetString("Done");
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
                TimeSpan t = TimeSpan.FromSeconds(_gameState.TimeLeft);
                TimerLabel.Content = string.Format("{0}:{1:D2}", t.Minutes, t.Seconds);
            }
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
