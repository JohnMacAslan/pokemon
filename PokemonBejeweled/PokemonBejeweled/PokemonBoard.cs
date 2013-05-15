using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    public class PokemonBoard
    {
        public static readonly int gridSize = 8;
        public static List<Type> TokenList = basicTokens();
        public event EventHandler StartingPlay;
        public event EventHandler<MakingPlayEventArgs> EndingPlay;
        public event EventHandler<MakingPlayEventArgs> BoardChanged;
        public event EventHandler<PointsAddedEventArgs> PointsAdded;
        private Random _rand = new Random();
        private int _pointsFromCurrentPlay = 0;
        private IBasicPokemonToken[,] _pokemonGrid = new IBasicPokemonToken[gridSize, gridSize];
        internal IBasicPokemonToken[,] PokemonGrid
        {
            get { return _pokemonGrid; }
            set { GridOperations.copyGrid(value, _pokemonGrid); }
        }
        private IBasicPokemonToken[,] _newPokemonGrid = new IBasicPokemonToken[gridSize, gridSize];
        internal IBasicPokemonToken[,] NewPokemonGrid
        {
            get { return _newPokemonGrid; }
            set { GridOperations.copyGrid(value, _newPokemonGrid); }
        }

        /// <summary>
        /// Constructs a grid of pokemon tokens. 
        /// </summary>
        public PokemonBoard()
        {
        }

        /// <summary>
        /// Generates the inital grid of IBasicPokemonTokens for the board. 
        /// </summary>
        public virtual IBasicPokemonToken[,] generateGrid()
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    _pokemonGrid[row, col] = generateNewPokemon();
                }
            }
            updateBoard();
            return PokemonGrid;
        }

        /// <summary>
        /// Generates a random basic pokemon. 
        /// </summary>
        /// <returns>A random basic pokemon of the type IBasicPokemonToken</returns>
        private IBasicPokemonToken generateNewPokemon()
        {
            int pokeNumber = _rand.Next(TokenList.Count);
            return (IBasicPokemonToken)Activator.CreateInstance(TokenList[pokeNumber]);
        }

        /// <summary>
        /// Begins a play. 
        /// </summary>
        /// <param name="row1">The row of the first location on the grid. </param>
        /// <param name="col1">The column of the first location on the grid. </param>
        /// <param name="row2">The row of the second location on the grid. </param>
        /// <param name="col2">The column of the second location on the grid. </param>
        /// <returns>True if a play was made, false otherwise. </returns>
        public virtual void tryPlay(IBasicPokemonToken[,] pokemonGrid, int row1, int col1, int row2, int col2)
        {
            if (GridOperations.arePiecesAdjacent(row1, col1, row2, col2))
            {
                PokemonGrid = pokemonGrid;
                NewPokemonGrid = pokemonGrid;
                makePlay(row1, col1, row2, col2);
                updateBoard();
                OnEndingPlay();
            }
        }

        /// <summary>
        /// Executes a play. 
        /// </summary>
        /// <param name="row1">The row of the first location on the grid. </param>
        /// <param name="col1">The column of the first location on the grid. </param>
        /// <param name="row2">The row of the second location on the grid. </param>
        /// <param name="col2">The column of the second location on the grid. </param>
        public virtual void makePlay(int row1, int col1, int row2, int col2)
        {
            _pointsFromCurrentPlay = 0;
            bool madeMove = false;
            IBasicPokemonToken firstToken = _pokemonGrid[row1, col1];
            IBasicPokemonToken secondToken = _pokemonGrid[row2, col2];
            _pokemonGrid[row1, col1] = secondToken;
            _pokemonGrid[row2, col2] = firstToken;
            _newPokemonGrid[row1, col1] = secondToken;
            _newPokemonGrid[row2, col2] = firstToken;
            madeMove |= updateSingleRow(row1, col1);
            madeMove |= updateSingleRow(row2, col2);
            madeMove |= updateSingleColumn(row1, col1);
            madeMove |= updateSingleColumn(row2, col2);
            madeMove |= markDittoNulls(row1, col1, row2, col2);
            if (!madeMove)
            {
                _newPokemonGrid[row1, col1] = firstToken;
                _newPokemonGrid[row2, col2] = secondToken;
            }
            _pokemonGrid[row1, col1] = firstToken;
            _pokemonGrid[row2, col2] = secondToken;
        }

        /// <summary>
        /// If the new grid state is different from the current grid state, new tokens are pulled down and the grid is updated. 
        /// </summary>
        public virtual void updateBoard()
        {
            while (!haveGridsStabilized())
            {
                pullDownTokens();
                updateAllRows();
                updateAllColumns();
            }
        }

        /// <summary>
        /// Checks to see if the new grid state is different from the current grid state. 
        /// </summary>
        /// <returns>True if the new grid state is different from the current grid state, false otherwise. </returns>
        private bool haveGridsStabilized()
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (null == _pokemonGrid[row, col] || null == _newPokemonGrid[row, col] || !_pokemonGrid[row, col].Equals(_newPokemonGrid[row, col]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Fills in empty spaces in the grid by first pulling down existing tokens and then adding new ones where necessary. 
        /// </summary>
        public virtual void pullDownTokens()
        {
            PokemonGrid = NewPokemonGrid;
            OnBoardChanged();
            int numberOfTokensToPullDown;
            for (int col = 0; col < gridSize; col++)
            {
                for (int row = gridSize - 1; row >= 0; row--)
                {
                    if (null == _pokemonGrid[row, col])
                    {
                        numberOfTokensToPullDown = 0;
                        while (row >= numberOfTokensToPullDown && null == _pokemonGrid[row - numberOfTokensToPullDown, col])
                        {
                            numberOfTokensToPullDown++;
                        }
                        if (row >= numberOfTokensToPullDown)
                        {
                            _pokemonGrid[row, col] = _pokemonGrid[row - numberOfTokensToPullDown, col];
                            _pokemonGrid[row - numberOfTokensToPullDown, col] = null;
                        }
                        else
                        {
                            while (numberOfTokensToPullDown > 0)
                            {
                                _pokemonGrid[--numberOfTokensToPullDown, col] = generateNewPokemon();
                            }
                        }
                    }
                }
            }
            NewPokemonGrid = PokemonGrid;
            OnBoardChanged();
        }

        /// <summary>
        /// Iterates through all the rows of the grid checking for rows of three or more and updates the new grid accordingly. 
        /// </summary>
        public virtual void updateAllRows()
        {
            int numberOfSameTokens;
            IBasicPokemonToken currentToken;
            for (int row = 0; row < gridSize; row++)
            {
                currentToken = _pokemonGrid[row, 0];
                numberOfSameTokens = 1;
                for (int col = 1; col < gridSize; col++)
                {
                    if (currentToken.isSameSpecies(_pokemonGrid[row, col]))
                    {
                        numberOfSameTokens++;
                    }
                    else if (3 <= numberOfSameTokens)
                    {
                        markNullRow(row, col - numberOfSameTokens, numberOfSameTokens);
                        evolveToken(row, col - numberOfSameTokens, numberOfSameTokens);
                        numberOfSameTokens = 1;
                    }
                    else
                    {
                        currentToken = _pokemonGrid[row, col];
                        numberOfSameTokens = 1;
                    }
                }
                if (3 <= numberOfSameTokens)
                {
                    markNullRow(row, gridSize - numberOfSameTokens, numberOfSameTokens);
                    evolveToken(row, gridSize - numberOfSameTokens, numberOfSameTokens);
                }
            }
        }

        /// <summary>
        /// Iterates through all the columns of the grid checking for columns of three or more and updates the new grid accordingly. 
        /// </summary>
        public virtual void updateAllColumns()
        {
            GridOperations.invertGrid(_pokemonGrid);
            GridOperations.invertGrid(_newPokemonGrid);
            updateAllRows();
            GridOperations.invertGrid(_pokemonGrid);
            GridOperations.invertGrid(_newPokemonGrid);
        }
        
        /// <summary>
        /// Checks to see if the given location is in a row of three or more. 
        /// </summary>
        /// <param name="rowStart">The row of the location to check. </param>
        /// <param name="colStart">The column of the location to check. </param>
        /// <returns>True if a row of three or more was found, false otherwise. </returns>
        public virtual bool updateSingleRow(int rowStart, int colStart)
        {
            IBasicPokemonToken startToken = _pokemonGrid[rowStart, colStart];
            int numberOfSameTokens = 1;

            int currentCol = colStart - 1;
            while (currentCol >= 0 && startToken.isSameSpecies(_pokemonGrid[rowStart, currentCol]))
            {
                numberOfSameTokens++;
                currentCol--;
            }
            currentCol = colStart + 1;
            while (currentCol < gridSize && startToken.isSameSpecies(_pokemonGrid[rowStart, currentCol]))
            {
                numberOfSameTokens++;
                currentCol++;
            }
            if (3 <= numberOfSameTokens)
            {
                OnStartingPlay();
                markNullRow(rowStart, currentCol - numberOfSameTokens, numberOfSameTokens);
                evolveToken(rowStart, colStart, numberOfSameTokens);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the given location is in a column of three or more. 
        /// </summary>
        /// <param name="rowStart">The row of the location to check. </param>
        /// <param name="colStart">The column of the location to check. </param>
        /// <returns>True if a column of three or more was found, false otherwise. </returns>
        public virtual bool updateSingleColumn(int rowStart, int colStart)
        {
            GridOperations.invertGrid(_pokemonGrid);
            GridOperations.invertGrid(_newPokemonGrid);
            bool madeMove = updateSingleRow(colStart, rowStart);
            GridOperations.invertGrid(_pokemonGrid);
            GridOperations.invertGrid(_newPokemonGrid);
            return madeMove;
        }

        /// <summary>
        /// Checks to see if one of the two swapped locations is a DittoToken, and if so marks all
        /// tokens of the same type as was in the other location as null. 
        /// </summary>
        /// <param name="row1">The row of the first swapped location. </param>
        /// <param name="col1">The column of the first swapped location. </param>
        /// <param name="row2">The row of the second swapped location. </param>
        /// <param name="col2">The column of the second swapped location. </param>
        /// <returns>True if a DittoToken was swapped, false otherwise. </returns>
        public virtual bool markDittoNulls(int row1, int col1, int row2, int col2)
        {
            if (_pokemonGrid[row1, col1].GetType() == typeof(DittoToken) && _pokemonGrid[row2, col2].GetType() == typeof(DittoToken))
            {
                OnPointsAdded(1000);
                OnStartingPlay();
                markFullRowAndColumnAsNull(row1 - 1, col1);
                markFullRowAndColumnAsNull(row1 - 1, col1 - 1);
                markFullRowAndColumnAsNull(row1 - 1, col1 + 1);
                markFullRowAndColumnAsNull(row1, col1 - 1);
                markFullRowAndColumnAsNull(row1, col1 + 1);
                markFullRowAndColumnAsNull(row1 + 1, col1);
                markFullRowAndColumnAsNull(row1 + 1, col1 - 1);
                markFullRowAndColumnAsNull(row1 + 1, col1 + 1);
                return true;
            }
            else if (_pokemonGrid[row1, col1].GetType() == typeof(DittoToken))
            {
                OnStartingPlay();
                markAllTokensOfSameTypeAsNull(_pokemonGrid[row2, col2]);
                _newPokemonGrid[row1, col1] = null;
                return true;
            }
            else if (_pokemonGrid[row2, col2].GetType() == typeof(DittoToken))
            {
                OnStartingPlay();
                markAllTokensOfSameTypeAsNull(_pokemonGrid[row1, col1]);
                _newPokemonGrid[row2, col2] = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Marks the token at a given location null and adds 10 points to the play score. If the token is a
        /// first evolution pokemon, 30 additional points are added and the surrounding tokens are marked null. 
        /// If the token is a second evolution pokemon, 60 additional points are added and the tokens in the same
        /// row and column are marked null. 
        /// </summary>
        /// <param name="row">The row of the token to mark null. </param>
        /// <param name="col">The column of the token to mark null. </param>
        public virtual void updateToken(int row, int col)
        {
            if (0 <= row && row < gridSize && 0 <= col && col < gridSize && null != _newPokemonGrid[row, col])
            {
                _newPokemonGrid[row, col] = null;
                if (_pokemonGrid[row, col].GetType().GetInterfaces().Contains(typeof(IFirstEvolutionPokemonToken)))
                {
                    OnPointsAdded(30);
                    markSurroundingTokensNull(row, col);
                }
                else if (_pokemonGrid[row, col].GetType().GetInterfaces().Contains(typeof(ISecondEvolutionPokemonToken)))
                {
                    OnPointsAdded(60);
                    markFullRowAndColumnAsNull(row, col);
                }
                OnPointsAdded(10);
            }
        }

        /// <summary>
        /// Based on the number of same tokens in a row or column, evolves the token at the given 
        /// location. 4 of the same evolves to the first evolution and awards 100 bonus points, 
        /// 5 to Ditto with 300 bonus points, and 6 to the second evolution with 600 bonus points. 
        /// </summary>
        /// <param name="row">The row of the token to evolve. </param>
        /// <param name="col">The column of the token to evolve. </param>
        /// <param name="numberOfSameTokens">The number of same tokens in a row or column</param>
        public virtual void evolveToken(int row, int col, int numberOfSameTokens)
        {
            IBasicPokemonToken movedToken = _pokemonGrid[row, col];
            if (3 < numberOfSameTokens)
            {
                OnPointsAdded((int)Math.Pow(2, numberOfSameTokens) * 10);
                switch (numberOfSameTokens)
                {
                    case 4:
                        _newPokemonGrid[row, col] = movedToken.firstEvolvedToken();
                        break;
                    case 5:
                        _newPokemonGrid[row, col] = new DittoToken();
                        break;
                    default:
                        _newPokemonGrid[row, col] = movedToken.secondEvolvedToken();
                        break;
                }
            }
        }

        /// <summary>
        /// Marks a row of tokens null based on the provided location and number tokens to mark null. 
        /// </summary>
        /// <param name="row">The row in which the row of three or more tokens existing. </param>
        /// <param name="colStart">The column at which the row of three or more tokens starts. </param>
        /// <param name="numberOfSameTokens">The number of same tokens in a row. </param>
        public virtual void markNullRow(int row, int colStart, int numberOfSameTokens)
        {
            if (3 <= numberOfSameTokens)
            {
                for (int i = 0; i < numberOfSameTokens; i++)
                {
                    updateToken(row, colStart + i);
                }
            }
        }
        
        /// <summary>
        /// Searches through the entire board and marks any tokens in the same evolution chain
        /// as the given IBasicPokemonToken as null. 
        /// </summary>
        /// <param name="pokemon">The pokemon for which all tokens of the same species will be marked null. </param>
        public virtual void markAllTokensOfSameTypeAsNull(IBasicPokemonToken pokemon)
        {
            int numTokensMarkedNull = 0;
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (_pokemonGrid[row, col].isSameSpecies(pokemon))
                    {
                        numTokensMarkedNull++;
                        updateToken(row, col);
                    }
                }
            }
             OnPointsAdded((int)Math.Pow(numTokensMarkedNull, 2) * 10);
        }

        /// <summary>
        /// Marks all tokens around a given location as null. 
        /// </summary>
        /// <param name="row">The row of the location around which to mark tokens as null. </param>
        /// <param name="col">The column of the location around which to mark tokens as null. </param>
        public virtual void markSurroundingTokensNull(int row, int col)
        {
            updateToken(row - 1, col);
            updateToken(row - 1, col - 1);
            updateToken(row - 1, col + 1);
            updateToken(row, col - 1);
            updateToken(row, col + 1);
            updateToken(row + 1, col);
            updateToken(row + 1, col - 1);
            updateToken(row + 1, col + 1);
        }

        /// <summary>
        /// Marks all tokens in the specified row and column as null. 
        /// </summary>
        /// <param name="row">The row to mark null. </param>
        /// <param name="col">The column to mark null. </param>
        public virtual void markFullRowAndColumnAsNull(int row, int col)
        {
            if (0 <= row && row < gridSize)
            {
                for (int currentRow = 0; currentRow < gridSize; currentRow++)
                {
                    updateToken(currentRow, col);
                }
            }
            if (0 <= col && col < gridSize)
            {
                for (int currentCol = 0; currentCol < gridSize; currentCol++)
                {
                    updateToken(row, currentCol);
                }
            }
        }

        /// <summary>
        /// Searches through the entire more to see if there are any moves left. If so, 
        /// returns true and the location of the move. 
        /// </summary>
        /// <param name="rowHint">The row of the token that can be switched to make a move. </param>
        /// <param name="colHint">The column of the token that can be switched to make a move. </param>
        /// <returns>True if a move is possible, false otherwise. </returns>
        public virtual bool areMovesLeft(IBasicPokemonToken[,] pokemonGrid, out int rowHint, out int colHint)
        {
            PokemonGrid = pokemonGrid;
            NewPokemonGrid = pokemonGrid;
            bool isMove = false;
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize - 1; col++)
                {
                    isMove = testForMove(row, col);
                    if (isMove)
                    {
                        rowHint = row;
                        colHint = col;
                        return true;
                    }
                    GridOperations.invertGrid(_pokemonGrid);
                    GridOperations.invertGrid(_newPokemonGrid);
                    isMove = testForMove(row, col);
                    GridOperations.invertGrid(_pokemonGrid);
                    GridOperations.invertGrid(_newPokemonGrid);
                    if (isMove)
                    {
                        rowHint = col;
                        colHint = row;
                        return true;
                    }
                }
            }
            rowHint = -1;
            colHint = -1;
            return false;
        }

        /// <summary>
        /// Checks to see if swapping the location with the token one to the right makes a valid move. 
        /// </summary>
        /// <param name="row">The row of the token to swap. </param>
        /// <param name="col">The column of the token to swap. </param>
        /// <returns>rue if a move is found, false otherwise. </returns>
        private bool testForMove(int row, int col)
        {
            bool isMove = false;
            int pointsFromLastPlay = _pointsFromCurrentPlay;
            makePlay(row, col, row, col + 1);
            if (!haveGridsStabilized())
            {
                OnPointsAdded(-50 - _pointsFromCurrentPlay);
                GridOperations.copyGrid(_pokemonGrid, _newPokemonGrid);
                isMove = true;
            }
            _pointsFromCurrentPlay = pointsFromLastPlay;
            return isMove;
        }

        /// <summary>
        /// Fired when points are gained.  
        /// </summary>
        protected void OnPointsAdded(int pointsToAdd)
        {
            _pointsFromCurrentPlay += pointsToAdd;
            if (null != PointsAdded)
            {
                PointsAdded(this, new PointsAddedEventArgs(pointsToAdd));
            }
        }

        /// <summary>
        /// Fired when the board changes. 
        /// </summary>
        protected void OnBoardChanged()
        {
            if (null != BoardChanged)
            {
                BoardChanged(this, new MakingPlayEventArgs(PokemonGrid));
            }
        }

        /// <summary>
        /// Fired when a valid play is starting. 
        /// </summary>
        protected void OnStartingPlay()
        {
            if (null != StartingPlay)
            {
                StartingPlay(this, new MakingPlayEventArgs(PokemonGrid));
            }
        }

        /// <summary>
        /// Fired when a play, valid or not, ends.  
        /// </summary>
        protected void OnEndingPlay()
        {
            if (null != EndingPlay)
            {
                EndingPlay(this, new MakingPlayEventArgs(PokemonGrid));
            }
        }

        /// <summary>
        /// A dictionary related integers to the seven types of basic tokens, used for
        /// randomly generating new tokens. 
        /// </summary>
        /// <returns>A dictionary mapping ints to pokemon types. </returns>
        private static List<Type> basicTokens()
        {
            List<Type> list = new List<Type>();
            list.Add(typeof(BulbasaurToken));
            list.Add(typeof(CharmanderToken));
            list.Add(typeof(ChikoritaToken));
            list.Add(typeof(CyndaquilToken));
            list.Add(typeof(PichuToken));
            list.Add(typeof(SquirtleToken));
            list.Add(typeof(TotodileToken));
            return list;
        }
    }
}
