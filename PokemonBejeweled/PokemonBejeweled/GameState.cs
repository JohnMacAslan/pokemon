using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    public class GameState
    {
        internal static readonly int NO_TIME_LIMIT = -1;
        internal int PreviousScore = 0;
        internal int CurrentScore = 0;
        private int _previousRow;
        private int _previousColumn;
        internal double TimeLeft = NO_TIME_LIMIT;
        private Boolean _justMadeMove;
        private Timer _countdown = new Timer(1000);
        internal Timer Countdown
        {
            get { return _countdown; }
        }
        public EventHandler<TimeUpdatedEventArgs> TimeUpdated;
        public EventHandler<ScoreUpdatedEventArgs> ScoreUpdated;
        public EventHandler<MakingPlayEventArgs> BoardChanged;
        private PokemonBoard _board;
        internal PokemonBoard Board
        {
            get { return _board; }
            set { _board = value; }
        }
        private IBasicPokemonToken[,] _currentGrid = new IBasicPokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
        internal IBasicPokemonToken[,] CurrentGrid
        {
            get { return _currentGrid; }
            set { GridOperations.copyGrid(value, _currentGrid); }
        }
        private IBasicPokemonToken[,] _previousGrid = new IBasicPokemonToken[PokemonBoard.gridSize, PokemonBoard.gridSize];
        internal IBasicPokemonToken[,] PreviousGrid
        {
            get { return _previousGrid; }
            set { GridOperations.copyGrid(value, _previousGrid); }
        }

        /// <summary>
        /// Constructs the object that holds the current state of the game. 
        /// </summary>
        public GameState()
        {
            newGame();
            _countdown.Elapsed += new ElapsedEventHandler(decrementTime);
            _countdown.Start();
        }

        /// <summary>
        /// Initializes all the fields to their initial values for a new game. 
        /// </summary>
        public void newGame()
        {
            _previousRow = 0;
            _previousColumn = 0;
            _board = new PokemonBoard();
            PreviousGrid = _board.generateGrid();
            CurrentGrid = PreviousGrid;
            PreviousScore = 0;
            CurrentScore = 0;
            _board.PointsAdded += OnPointsAdded;
            _board.BoardChanged += OnBoardChanged;
            _board.StartingPlay += OnStartingPlay;
            _board.EndingPlay += OnEndingPlay;
            OnScoreUpdated();
            OnBoardChanged(this, new MakingPlayEventArgs(CurrentGrid));
        }

        /// <summary>
        /// If a move has not just been made, attempts to swap the last token clicked
        /// with the previously clicked token. 
        /// </summary>
        /// <param name="row">The row of the last token clicked. </param>
        /// <param name="col">The column of the last token clicked. </param>
        public virtual void tryPlay(int row, int col)
        {
            if (TimeLeft != 0 && !_justMadeMove)
            {
                _board.tryPlay(CurrentGrid, row, col, _previousRow, _previousColumn);
            }
            else
            {
                _justMadeMove = false;
            }
            _previousRow = row;
            _previousColumn = col;
        }
        
        /// <summary>
        /// Reverts the state of the current grid to that of the previous grid and the current score to the previous score.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void undoPlay(object sender, EventArgs e)
        {
            CurrentGrid = PreviousGrid;
            CurrentScore = PreviousScore;
            OnBoardChanged(this, new MakingPlayEventArgs(CurrentGrid));
            OnScoreUpdated();
        }

        /// <summary>
        /// Decrements the time every second, provided that the time has not reached zero and there is a time limit. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void decrementTime(object sender, ElapsedEventArgs e)
        {
            if (TimeLeft != 0 && NO_TIME_LIMIT != TimeLeft)
            {
                TimeLeft--;
                if (null != TimeUpdated)
                {
                    TimeUpdated(this, new TimeUpdatedEventArgs(TimeLeft));
                }
            }
        }

        /// <summary>
        /// Updates the state of the current grid and previous grid when a valid play starts. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnStartingPlay(object sender, EventArgs e)
        {
            PreviousGrid = CurrentGrid;
            PreviousScore = CurrentScore;
        }

        /// <summary>
        /// Updates the state of the current grid when a play ends. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEndingPlay(object sender, MakingPlayEventArgs e)
        {
            CurrentGrid = e.PokemonGrid;
            OnBoardChanged(this, e);
        }

        /// <summary>
        /// Updates the state of the current grid and previous grid when a play ends. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBoardChanged(object sender, MakingPlayEventArgs e)
        {
            _justMadeMove = true;
            if (null != BoardChanged)
            {
                BoardChanged(this, new MakingPlayEventArgs(e.PokemonGrid));
            }
        }

        /// <summary>
        /// Fired when points are added to the score. 
        /// </summary>
        private void OnPointsAdded(object sender, PointsAddedEventArgs e)
        {
            CurrentScore += e.Points;
            OnScoreUpdated();
        }

        /// <summary>
        /// Fired when the score is updated. 
        /// </summary>
        private void OnScoreUpdated()
        {
            if (null != ScoreUpdated)
            {
                ScoreUpdated(this, new ScoreUpdatedEventArgs(CurrentScore));
            }
        }
    }
}
