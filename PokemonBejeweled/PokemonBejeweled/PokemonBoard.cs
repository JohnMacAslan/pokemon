using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    public delegate void BoardDirtiedEventHandler(object source);

    public class PokemonBoard
    {
        public static int gridSize = 8;
        private int _gamePlayScore = 0;
        public int GamePlayScore
        {
            get { return _gamePlayScore; }
            set { _gamePlayScore = value; }
        }
        private IBasicPokemonToken[,] _pokemonGrid = new IBasicPokemonToken[gridSize, gridSize];
        internal IBasicPokemonToken[,] PokemonGrid
        {
            get { return _pokemonGrid; }
            set { copyGrid(value, _pokemonGrid); }
        }
        private IBasicPokemonToken[,] _newPokemonGrid = new IBasicPokemonToken[gridSize, gridSize];
        internal IBasicPokemonToken[,] NewPokemonGrid
        {
            get { return _newPokemonGrid; }
            set { copyGrid(value, _newPokemonGrid); }
        }
        private PokemonGridHistory _pokemonHistory = new PokemonGridHistory();
        private Random rand = new Random();
        private static Dictionary<int, Type> dict = basicTokens();
        public event BoardDirtiedEventHandler BoardDirtied;

        public PokemonBoard()
        {
            generateGrid();
            _pokemonHistory.Clear();
            _pokemonHistory.Add((IBasicPokemonToken[,])_pokemonGrid.Clone());
        }

        public virtual void generateGrid()
        {
            IBasicPokemonToken[,] pokemon = new IBasicPokemonToken[gridSize, gridSize];
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    pokemon[row, col] = generateNewPokemon();
                }
            }
            updateBoard();
        }

        private IBasicPokemonToken generateNewPokemon()
        {
            int pokeNumber = rand.Next(1, 8);
            return (IBasicPokemonToken)Activator.CreateInstance(dict[pokeNumber]);
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
            copyGrid(_newPokemonGrid, _pokemonGrid);
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
            _pokemonHistory.Add((IBasicPokemonToken[,])_pokemonGrid.Clone());
            copyGrid(_pokemonGrid, _newPokemonGrid);
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
            copyGrid(invertPokemon(_pokemonGrid), _pokemonGrid);
            copyGrid(invertPokemon(_newPokemonGrid), _newPokemonGrid);
            updateAllRows();
            copyGrid(invertPokemon(_pokemonGrid), _pokemonGrid);
            copyGrid(invertPokemon(_newPokemonGrid), _newPokemonGrid);
        }

        public static void copyGrid(IBasicPokemonToken[,] gridToCopy, IBasicPokemonToken[,] gridDestination)
        {
            int rowLength = gridToCopy.GetLength(0);
            int colLength = gridToCopy.GetLength(1);
            if (rowLength != gridDestination.GetLength(0) || colLength != gridDestination.GetLength(1))
            {
                throw new ArithmeticException("Dimensions of grid did not match dimensions of destination grid");
            }
            else
            {
                for (int row = 0; row < rowLength; row++)
                {
                    for (int col = 0; col < colLength; col++)
                    {
                        gridDestination[row, col] = gridToCopy[row, col];
                    }
                }
            }
        }

        protected virtual void OnBoardDirtied()
        {
            if (BoardDirtied != null)
            {
                BoardDirtied(this);
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

        public virtual void updateToken(int row, int col)
        {
            if (_pokemonGrid[row, col].GetType().GetInterfaces().Contains(typeof(IFirstEvolutionPokemonToken)))
            {
                markSurroundingTokensNull(row, col);
            }
            else if (_pokemonGrid[row, col].GetType().GetInterfaces().Contains(typeof(ISecondEvolutionPokemonToken)))
            {
                markFullRowAndColumnAsNull(row, col);
            }
            _newPokemonGrid[row, col] = null;
            _gamePlayScore += 10;
        }

        public virtual void evolveToken(int row, int col, int numberOfSameTokens)
        {
            IBasicPokemonToken movedToken = _pokemonGrid[row, col];
            switch (numberOfSameTokens)
            {
                case 4:
                    _gamePlayScore += 50;
                    _newPokemonGrid[row, col] = movedToken.firstEvolvedToken();
                    break;
                case 5:
                    _gamePlayScore += 100;
                    _newPokemonGrid[row, col] = new DittoToken();
                    break;
                case 6:
                    _gamePlayScore += 300;
                    _newPokemonGrid[row, col] = movedToken.secondEvolvedToken();
                    break;
            }
        }

        public int makePlay(int row1, int col1, int row2, int col2)
        {
            _gamePlayScore = 0;
            if (piecesAreAdjacent(row1, col1, row2, col2))
            {
                IBasicPokemonToken firstToken = _pokemonGrid[row1, col1];
                IBasicPokemonToken secondToken = _pokemonGrid[row2, col2];
                _pokemonGrid[row1, col1] = secondToken;
                _pokemonGrid[row2, col2] = firstToken;
                updateSingleRow(row1, col1, row2, col2);
                updateSingleRow(row2, col2, row1, col1);
                updateSingleColumn(row1, col1, row2, col2);
                updateSingleColumn(row2, col2, row1, col1);
                swapDitto(row1, col1, row2, col2);
                _pokemonGrid[row1, col1] = firstToken;
                _pokemonGrid[row2, col2] = secondToken;
                updateBoard();
            }
            return _gamePlayScore;
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

        public virtual void updateSingleRow(int rowStart, int colStart, int rowEnd, int colEnd)
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
                _newPokemonGrid[rowStart, colStart] = _pokemonGrid[rowStart, colStart];
                _newPokemonGrid[rowEnd, colEnd] = _pokemonGrid[rowEnd, colEnd];
                markNullRow(rowStart, currentCol - numberOfSameTokens, numberOfSameTokens);
                evolveToken(rowStart, colStart, numberOfSameTokens);
            }
        }

        public virtual void updateSingleColumn(int rowStart, int colStart, int rowEnd, int colEnd)
        {
            copyGrid(invertPokemon(_pokemonGrid), _pokemonGrid);
            copyGrid(invertPokemon(_newPokemonGrid), _newPokemonGrid);
            updateSingleRow(colStart, rowStart, colEnd, rowEnd);
            copyGrid(invertPokemon(_pokemonGrid), _pokemonGrid);
            copyGrid(invertPokemon(_newPokemonGrid), _newPokemonGrid);
        }

        public virtual void swapDitto(int row1, int col1, int row2, int col2)
        {
            if (_pokemonGrid[row1, col1].GetType() == typeof(DittoToken))
            {
                markAllTokensOfSameTypeAsNull(_pokemonGrid[row2, col2]);
                _newPokemonGrid[row1, col1] = null;
            }
            else if (_pokemonGrid[row2, col2].GetType() == typeof(DittoToken))
            {
                markAllTokensOfSameTypeAsNull(_pokemonGrid[row1, col1]);
                _newPokemonGrid[row2, col2] = null;
            }
        }

        public virtual void markAllTokensOfSameTypeAsNull(IBasicPokemonToken pokemon)
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (_pokemonGrid[row, col].isSameSpecies(pokemon))
                    {
                        updateToken(row, col);
                    }
                }
            }
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

        public static void printGrid(IBasicPokemonToken[,] grid)
        {
            Dictionary<Type, int> dict = new Dictionary<Type, int>();

            dict.Add(typeof(BulbasaurToken), 1);
            dict.Add(typeof(CharmanderToken), 2);
            dict.Add(typeof(ChikoritaToken), 3);
            dict.Add(typeof(CyndaquilToken), 4);
            dict.Add(typeof(PichuToken), 5);
            dict.Add(typeof(SquirtleToken), 6);
            dict.Add(typeof(TotodileToken), 7);
            Console.Out.WriteLine("--------");
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (null == grid[row, col])
                    {
                        Console.Out.Write(" ");
                    }
                    else if (dict.ContainsKey(grid[row, col].GetType()))
                    {
                        Console.Out.Write(dict[grid[row, col].GetType()]);
                    }
                    else
                    {
                        Console.Out.Write("F");
                    }
                }
                Console.Out.WriteLine();
            }
            Console.Out.WriteLine("--------");
        }

        public static IBasicPokemonToken[,] invertPokemon(IBasicPokemonToken[,] _pokemonToInvert)
        {
            int rowLength = _pokemonToInvert.GetLength(0);
            int colLength = _pokemonToInvert.GetLength(1);
            if (rowLength != colLength)
            {
                throw new ArithmeticException("Grid is not square.");
            }
            else
            {
                IBasicPokemonToken[,] _invertedPokemon = new IBasicPokemonToken[rowLength, colLength];
                copyGrid(_pokemonToInvert, _invertedPokemon);
                for (int row = 0; row < rowLength; row++)
                {
                    for (int col = 0; col < colLength; col++)
                    {
                        _invertedPokemon[row, col] = _pokemonToInvert[col, row];
                    }
                }
                return _invertedPokemon;
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
