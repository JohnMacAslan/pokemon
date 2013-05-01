using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    public delegate void BoardDirtiedEventHandler(object source);
    public delegate void PointsAddedEventHandler(object source);

    public class PokemonBoard
    {
        public static readonly int gridSize = 8;
        public event BoardDirtiedEventHandler BoardDirtied;
        public event PointsAddedEventHandler PointsAdded;
        private bool _undoAllowed = true;
        private Random _rand = new Random();
        private static Dictionary<int, Type> _tokenDict = basicTokens();
        private PokemonGridHistory _pokemonHistory = new PokemonGridHistory();
        private int _pointsFromLastPlay = 0;
        private int _pointsToAdd = 0;
        internal int PointsToAdd
        {
            get { return _pointsToAdd; }
            set { _pointsToAdd = value; }
        }
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

        public PokemonBoard()
        {
            generateGrid();
            _pokemonHistory.Clear();
            _pokemonHistory.Add((IBasicPokemonToken[,])_pokemonGrid.Clone());
        }

        public virtual void generateGrid()
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    _pokemonGrid[row, col] = generateNewPokemon();
                }
            }
            updateBoard();
        }

        private IBasicPokemonToken generateNewPokemon()
        {
            int pokeNumber = _rand.Next(1, 8);
            return (IBasicPokemonToken)Activator.CreateInstance(_tokenDict[pokeNumber]);
        }

        public virtual bool piecesAreAdjacent(int row1, int col1, int row2, int col2)
        {
            if (row1 == row2 && Math.Abs(col1 - col2) == 1)
            {
                return true;
            }
            if (col1 == col2 && Math.Abs(row1 - row2) == 1)
            {
                return true;
            }
            return false;
        }

        public virtual void startPlay(int row1, int col1, int row2, int col2)
        {
            bool madeMove = false;
            IBasicPokemonToken firstToken = _pokemonGrid[row1, col1];
            IBasicPokemonToken secondToken = _pokemonGrid[row2, col2];
            _pokemonGrid[row1, col1] = secondToken;
            _pokemonGrid[row2, col2] = firstToken;
            _newPokemonGrid[row1, col1] = secondToken;
            _newPokemonGrid[row2, col2] = firstToken;
            madeMove |= updateSingleRow(row1, col1, row2, col2);
            madeMove |= updateSingleRow(row2, col2, row1, col1);
            madeMove |= updateSingleColumn(row1, col1, row2, col2);
            madeMove |= updateSingleColumn(row2, col2, row1, col1);
            madeMove |= swapDitto(row1, col1, row2, col2);
            if (!madeMove)
            {
                _newPokemonGrid[row1, col1] = firstToken;
                _newPokemonGrid[row2, col2] = secondToken;
            }
            _pokemonGrid[row1, col1] = firstToken;
            _pokemonGrid[row2, col2] = secondToken;
        }

        public virtual void makePlay(int row1, int col1, int row2, int col2)
        {
            _pointsFromLastPlay = 0;
            if (piecesAreAdjacent(row1, col1, row2, col2))
            {
                startPlay(row1, col1, row2, col2);
                updateBoard();
                _pokemonHistory.Add((IBasicPokemonToken[,])_pokemonGrid.Clone());
                _undoAllowed = true;
            }
        }

        public virtual void undoPlay()
        {
            if (_undoAllowed && 2 <= _pokemonHistory.PokemonHistory.Count)
            {
                GridOperations.copyGrid(_pokemonHistory.NextToLast(), _pokemonGrid);
                GridOperations.copyGrid(_pokemonHistory.NextToLast(), _newPokemonGrid);
                _pokemonHistory.RemoveAt(_pokemonHistory.PokemonHistory.Count - 1);
                _undoAllowed = false;
                OnPointsAdded(-_pointsFromLastPlay);
                OnBoardDirtied();
                _pointsFromLastPlay = 0;
            }
        }

        public virtual void updateBoard()
        {
            while (!haveGridsStabilized())
            {
                pullDownTokens();
                updateAllRows();
                updateAllColumns();
            }
        }

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

        public virtual void pullDownTokens()
        {
            GridOperations.copyGrid(_newPokemonGrid, _pokemonGrid);
            OnBoardDirtied();
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
            GridOperations.copyGrid(_pokemonGrid, _newPokemonGrid);
            OnBoardDirtied();
        }

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

        public virtual void updateAllColumns()
        {
            GridOperations.invertGrid(_pokemonGrid);
            GridOperations.invertGrid(_newPokemonGrid);
            updateAllRows();
            GridOperations.invertGrid(_pokemonGrid);
            GridOperations.invertGrid(_newPokemonGrid);
        }
        
        public virtual bool updateSingleRow(int rowStart, int colStart, int rowEnd, int colEnd)
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
                markNullRow(rowStart, currentCol - numberOfSameTokens, numberOfSameTokens);
                evolveToken(rowStart, colStart, numberOfSameTokens);
                return true;
            }
            return false;
        }

        public virtual bool updateSingleColumn(int rowStart, int colStart, int rowEnd, int colEnd)
        {
            GridOperations.invertGrid(_pokemonGrid);
            GridOperations.invertGrid(_newPokemonGrid);
            bool madeMove = updateSingleRow(colStart, rowStart, colEnd, rowEnd);
            GridOperations.invertGrid(_pokemonGrid);
            GridOperations.invertGrid(_newPokemonGrid);
            return madeMove;
        }

        public virtual bool swapDitto(int row1, int col1, int row2, int col2)
        {
            if (_pokemonGrid[row1, col1].GetType() == typeof(DittoToken))
            {
                markAllTokensOfSameTypeAsNull(_pokemonGrid[row2, col2]);
                _newPokemonGrid[row1, col1] = null;
                return true;
            }
            else if (_pokemonGrid[row2, col2].GetType() == typeof(DittoToken))
            {
                markAllTokensOfSameTypeAsNull(_pokemonGrid[row1, col1]);
                _newPokemonGrid[row2, col2] = null;
                return true;
            }
            return false;
        }

        public virtual void updateToken(int row, int col)
        {
            if (_pokemonGrid[row, col].GetType().GetInterfaces().Contains(typeof(IFirstEvolutionPokemonToken)))
            {
                if (null != _newPokemonGrid[row, col])
                {
                    OnPointsAdded(30);
                    _newPokemonGrid[row, col] = null;
                    markSurroundingTokensNull(row, col);
                }
            }
            else if (_pokemonGrid[row, col].GetType().GetInterfaces().Contains(typeof(ISecondEvolutionPokemonToken)))
            {
                if (null != _newPokemonGrid[row, col])
                {
                    OnPointsAdded(30);
                    _newPokemonGrid[row, col] = null;
                    markFullRowAndColumnAsNull(row, col);
                }
            }
            _newPokemonGrid[row, col] = null;
            OnPointsAdded(10);
        }

        public virtual void evolveToken(int row, int col, int numberOfSameTokens)
        {
            IBasicPokemonToken movedToken = _pokemonGrid[row, col];
            switch (numberOfSameTokens)
            {
                case 4:
                    OnPointsAdded(50);
                    _newPokemonGrid[row, col] = movedToken.firstEvolvedToken();
                    break;
                case 5:
                    OnPointsAdded(100);
                    _newPokemonGrid[row, col] = new DittoToken();
                    break;
                case 6:
                    OnPointsAdded(300);
                    _newPokemonGrid[row, col] = movedToken.secondEvolvedToken();
                    break;
            }
        }

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
            int addPoints = (int)Math.Pow(numTokensMarkedNull, 2) * 10;
            OnPointsAdded(addPoints);
        }

        public virtual void markSurroundingTokensNull(int row, int col)
        {
            if (row - 1 >= 0)
            {
                updateToken(row - 1, col);
                if (col - 1 >= 0) updateToken(row - 1, col - 1); ;
                if (col + 1 < gridSize) updateToken(row - 1, col + 1); ;
            }
            if (col - 1 >= 0) updateToken(row, col - 1);
            if (col + 1 < gridSize) updateToken(row, col + 1);
            if (row + 1 < gridSize)
            {
                updateToken(row + 1, col);
                if (col - 1 >= 0) updateToken(row + 1, col - 1);
                if (col + 1 < gridSize) updateToken(row + 1, col + 1);
            }
        }

        public virtual void markFullRowAndColumnAsNull(int row, int col)
        {
            for (int currentRow = 0; currentRow < gridSize; currentRow++)
            {
                updateToken(currentRow, col);
            }
            for (int currentCol = 0; currentCol < gridSize; currentCol++)
            {
                updateToken(row, currentCol);
            }
        }

        public virtual bool areMovesLeft(out int rowHint, out int colHint)
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize - 1; col++)
                {
                    startPlay(row, col, row, col + 1);
                    if (!haveGridsStabilized())
                    {
                        rowHint = row;
                        colHint = col;
                        OnPointsAdded(-_pointsFromLastPlay - 50);
                        GridOperations.copyGrid(_pokemonGrid, _newPokemonGrid);
                        return true;
                    }
                }
            }
            for (int col = 0; col < gridSize; col++)
            {
                for (int row = 0; row < gridSize - 1; row++)
                {
                    startPlay(row, col, row + 1, col);
                    if (!haveGridsStabilized())
                    {
                        rowHint = row;
                        colHint = col;
                        OnPointsAdded(-_pointsFromLastPlay - 50);
                        GridOperations.copyGrid(_pokemonGrid, _newPokemonGrid);
                        return true;
                    }
                }
            }
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (_pokemonGrid[row, col].GetType() == typeof(DittoToken))
                    {
                        rowHint = row;
                        colHint = col;
                        OnPointsAdded(-_pointsFromLastPlay - 50);
                        return true;
                    }
                }
            }
            rowHint = -1;
            colHint = -1;
            return false;
        }

        protected virtual void OnPointsAdded(int pointsToAdd)
        {
            _pointsToAdd = pointsToAdd;
            _pointsFromLastPlay += pointsToAdd;
            if (null != PointsAdded)
            {
                PointsAdded(this);
            }
        }

        protected virtual void OnBoardDirtied()
        {
            if (BoardDirtied != null)
            {
                BoardDirtied(this);
            }
        }

        private static Dictionary<int, Type> basicTokens()
        {
            Dictionary<int, Type> dict = new Dictionary<int, Type>();
            dict.Add(1, typeof(BulbasaurToken));
            dict.Add(2, typeof(CharmanderToken));
            dict.Add(3, typeof(ChikoritaToken));
            dict.Add(4, typeof(CyndaquilToken));
            dict.Add(5, typeof(PichuToken));
            dict.Add(6, typeof(SquirtleToken));
            dict.Add(7, typeof(TotodileToken));
            return dict;
        }
    }
}
