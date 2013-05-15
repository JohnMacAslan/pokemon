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
        private bool _paused = false;
        public bool Paused
        {
            get { return _paused; }
        }
        private Window _instructionsWindow = new Window();
        private TextBlock _instructionsText = new TextBlock();
        private ImageBrush _pokeball = PokemonPictureDictionary.getImageBrush(new PokeballToken());

        /// <summary>
        /// Constructs the primary display for the PokemonBejeweled game. 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            setUpInstructionWindow();
            setUpGridBoard();
            _gameState.ScoreUpdated += OnScoreUpdated;
            _gameState.BoardChanged += OnBoardChanged;
            _gameState.TimeUpdated += OnTimeUpdated;
            newGame(this, null);
            setLocalizedText();
            setUpLanguageButtons();
            NewGameButton.Click += newGame;
            PauseButton.Click += pauseGame;
            UndoButton.Click += _gameState.undoPlay;
            HintButton.Click += hint;
            QuitGameButton.Click += delegate { this.Close(); };
            Closing += delegate
            {
                _instructionsWindow.Closing -= new CancelEventHandler(HideInsteadOfClose);
                _instructionsWindow.Close();
            };
            InstructionsButton.Click += openInstructions;
        }

        /// <summary>
        /// Sets up the window for the game instructions. 
        /// </summary>
        private void setUpInstructionWindow()
        {
            _instructionsText.TextWrapping = System.Windows.TextWrapping.Wrap;
            _instructionsText.Text = _resourceManager.GetString("Instruction_Content");
            _instructionsWindow.Closing += new CancelEventHandler(HideInsteadOfClose);
            _instructionsWindow.Width = 450;
            _instructionsWindow.Height = 563;
        }

        /// <summary>
        /// Sets up the grid of buttons for the PokemonBoard. 
        /// </summary>
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

        /// <summary>
        /// Sets the GUI text based on the chosen language. 
        /// </summary>
        private void setLocalizedText()
        {
            try
            {
                this.Title = _resourceManager.GetString("Window_Title");
                MainMenu.Header = _resourceManager.GetString("Main_Menu");
                GameTitle.Text = _resourceManager.GetString("Game_Title");
                NewGameButton.Content = _resourceManager.GetString("New_Game");
                PauseButton.Content = _paused ? _resourceManager.GetString("Resume") : _resourceManager.GetString("Pause");
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

        /// <summary>
        /// Sets up the logic for switching between languages. 
        /// </summary>
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

        /// <summary>
        /// Resets the timer according to the checked button. 
        /// </summary>
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
            else
            {
                _gameState.TimeLeft = GameState.NO_TIME_LIMIT;
            }
        }

        /// <summary>
        /// Starts a new game. 
        /// </summary>
        private void newGame(object sender, RoutedEventArgs e)
        {
            _gameState.newGame();
            resetTimer();
        }

        /// <summary>
        /// Pauses the timer and hides the board from view. 
        /// </summary>
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

        /// <summary>
        /// Searches the board for a valid play. If a play is found, the corresponding token flashes. 
        /// </summary>
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

        /// <summary>
        /// Makes the instructions window simply hide instead of closing when closed. 
        /// </summary>
        private void HideInsteadOfClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _instructionsWindow.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Opens the instructions window. 
        /// </summary>
        private void openInstructions(object sender, RoutedEventArgs e)
        {
            _instructionsWindow.Visibility = Visibility.Visible;
            _instructionsWindow.Content = _instructionsText;
        }

        /// <summary>
        /// Updates the images associated with the grid buttons for the PokemonBoard. 
        /// <param name="e">The event args that holds to current grid with which to updatee the screen. </param>
        /// </summary>
        private void OnBoardChanged(object obj, MakingPlayEventArgs e)
        {
            IBasicPokemonToken[,] currentGrid = e.PokemonGrid;
            GridButton currentButton;
            System.Collections.IEnumerator buttonEnumerator = GridBoard.Children.GetEnumerator();
            for (int r = 0; r < PokemonBoard.gridSize; r++)
            {
                for (int c = 0; c < PokemonBoard.gridSize; c++)
                {
                    buttonEnumerator.MoveNext();
                    currentButton = (GridButton)buttonEnumerator.Current;
                    if (null == currentGrid[r, c])
                    {
                        currentButton.Background = _pokeball;
                    }
                    else
                    {
                        currentButton.Background = PokemonPictureDictionary.getImageBrush(currentGrid[r, c]);
                    }
                }
            }

            HintButton.Content = _resourceManager.GetString("Hint");
            DependencyObject scope = FocusManager.GetFocusScope(this);
            FocusManager.SetFocusedElement(scope, this);
            GridBoard.Dispatcher.Invoke(delegate() { Thread.Sleep(500); }, DispatcherPriority.Render);
        }

        /// <summary>
        /// Updates the time according to the TimeUpdatedEventArgs parameter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeUpdated(object sender, TimeUpdatedEventArgs e)
        {
            double timeLeft = e.Time;
            this.Dispatcher.Invoke((Action)(() =>
            {
                TimerLabel.Foreground = Brushes.Green;
                if (0 == timeLeft)
                {
                    TimerLabel.Foreground = Brushes.White;
                    TimerLabel.Content = _resourceManager.GetString("Done");
                }
                else if (GameState.NO_TIME_LIMIT == timeLeft)
                {
                    TimerLabel.Content = "--:--";
                }
                else
                {
                    if (10 > timeLeft)
                    {
                        TimerLabel.Foreground = Brushes.Red;
                    }
                    else if (30 > timeLeft)
                    {
                        TimerLabel.Foreground = Brushes.Yellow;
                    }
                    TimeSpan t = TimeSpan.FromSeconds(timeLeft);
                    TimerLabel.Content = string.Format("{0}:{1:D2}", t.Minutes, t.Seconds);
                }
                CommandManager.InvalidateRequerySuggested();
            }));
        }

        /// <summary>
        /// Updates the displayed score according to the score stored in the GameState. 
        /// </summary>
        private void OnScoreUpdated(object obj, ScoreUpdatedEventArgs e)
        {
            ScoreboardLabel.Content = e.Score;
            ScoreboardLabel.Dispatcher.Invoke(delegate() { }, DispatcherPriority.Render);
        }
    }
}
