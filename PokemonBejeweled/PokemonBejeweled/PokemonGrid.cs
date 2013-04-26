using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    public delegate void BoardDirtiedEventHandler(object source);

    public class PokemonGrid
    {
        public static int gridSize = 8;
        private int _gamePlayScore = 0;
        public int GamePlayScore
        {
            get { return _gamePlayScore; }
            set { _gamePlayScore = value; }
        }
        private IBasicPokemonToken[,] _pokemon = new IBasicPokemonToken[gridSize, gridSize];
        internal IBasicPokemonToken[,] Pokemon
        {
            get { return _pokemon; }
            set { copyGrid(value, _pokemon); }
        }
        private IBasicPokemonToken[,] _newPokemon = new IBasicPokemonToken[gridSize, gridSize];
        internal IBasicPokemonToken[,] NewPokemon
        {
            get { return _newPokemon; }
            set { copyGrid(value, _newPokemon); }
        }
        private PokemonGridHistory _pokemonHistory = new PokemonGridHistory();
        private Random rand = new Random();
        private static Dictionary<int, Type> dict = basicTokens();
        public event BoardDirtiedEventHandler BoardDirtied;

        public PokemonGrid()
        {
            _gamePlayScore = 0;
            generateGrid();
            _pokemonHistory.Clear();
            _pokemonHistory.Add((IBasicPokemonToken[,])_pokemon.Clone());
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
                    if (null == _pokemon[row, col] || null == _newPokemon[row, col] || !_pokemon[row, col].Equals(_newPokemon[row, col]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public virtual void pullDownTokens()
        {
            copyGrid(_newPokemon, _pokemon);
            OnBoardDirtied();
            int numberOfTokensToPullDown;
            for (int col = 0; col < gridSize; col++)
            {
                for (int row = gridSize - 1; row >= 0; row--)
                {
                    if (null == _pokemon[row, col])
                    {
                        numberOfTokensToPullDown = 0;
                        while (row >= numberOfTokensToPullDown && null == _pokemon[row - numberOfTokensToPullDown, col])
                        {
                            numberOfTokensToPullDown++;
                        }
                        if (row >= numberOfTokensToPullDown)
                        {
                            _pokemon[row, col] = _pokemon[row - numberOfTokensToPullDown, col];
                            _pokemon[row - numberOfTokensToPullDown, col] = null;
                        }
                        else
                        {
                            while (numberOfTokensToPullDown > 0)
                            {
                                _pokemon[--numberOfTokensToPullDown, col] = generateNewPokemon();
                            }
                        }
                    }
                }
            }
            _pokemonHistory.Add((IBasicPokemonToken[,])_pokemon.Clone());
            copyGrid(_pokemon, _newPokemon);
            OnBoardDirtied();
        }

        public virtual void updateAllRows()
        {
            int numberOfSameTokens;
            IBasicPokemonToken currentToken;
            for (int row = 0; row < gridSize; row++)
            {
                currentToken = _pokemon[row, 0];
                numberOfSameTokens = 1;
                for (int col = 1; col < gridSize; col++)
                {
                    if (currentToken.isSameSpecies(_pokemon[row, col]))
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
                        currentToken = _pokemon[row, col];
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
            copyGrid(invertPokemon(_pokemon), _pokemon);
            copyGrid(invertPokemon(_newPokemon), _newPokemon);
            updateAllRows();
            copyGrid(invertPokemon(_pokemon), _pokemon);
            copyGrid(invertPokemon(_newPokemon), _newPokemon);
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
                int col = colStart;
                while (col < colStart + numberOfSameTokens)
                {
                    _newPokemon[row, col++] = null;
                }
            }
        }

        public virtual void evolveToken(int row, int col, int numberOfSameTokens)
        {
            IBasicPokemonToken movedToken = _pokemon[row, col];
            switch (numberOfSameTokens)
            {
                case 4:
                    _newPokemon[row, col] = movedToken.firstEvolvedToken();
                    break;
                case 5:
                    _newPokemon[row, col] = new DittoToken();
                    break;
                case 6:
                    _newPokemon[row, col] = movedToken.secondEvolvedToken();
                    break;
            }
        }

        public void makePlay(int row1, int col1, int row2, int col2)
        {
            if (piecesAreAdjacent(row1, col1, row2, col2))
            {
                IBasicPokemonToken firstToken = _pokemon[row1, col1];
                IBasicPokemonToken secondToken = _pokemon[row2, col2];
                _pokemon[row1, col1] = secondToken;
                _pokemon[row2, col2] = firstToken;
                updateSingleRow(row1, col1, row2, col2);
                updateSingleRow(row2, col2, row1, col1);
                updateSingleColumn(row1, col1, row2, col2);
                updateSingleColumn(row2, col2, row1, col1);
                swapDitto(row1, col1, row2, col2);
                _pokemon[row1, col1] = firstToken;
                _pokemon[row2, col2] = secondToken;
                updateBoard();
            }
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
            IBasicPokemonToken startToken = _pokemon[rowStart, colStart];
            int numberOfSameTokens = 1;

            int currentCol = colStart - 1;
            while (currentCol >= 0 && startToken.isSameSpecies(_pokemon[rowStart, currentCol]))
            {
                numberOfSameTokens++;
                currentCol--;
            }
            currentCol = colStart + 1;
            while (currentCol < gridSize && startToken.isSameSpecies(_pokemon[rowStart, currentCol]))
            {
                numberOfSameTokens++;
                currentCol++;
            }
            if (3 <= numberOfSameTokens)
            {
                _newPokemon[rowStart, colStart] = _pokemon[rowStart, colStart];
                _newPokemon[rowEnd, colEnd] = _pokemon[rowEnd, colEnd];
                markNullRow(rowStart, currentCol - numberOfSameTokens, numberOfSameTokens);
                markRowSpecials(rowStart, currentCol - numberOfSameTokens, numberOfSameTokens);
                evolveToken(rowStart, colStart, numberOfSameTokens);
            }
        }

        public virtual void updateSingleColumn(int rowStart, int colStart, int rowEnd, int colEnd)
        {
            copyGrid(invertPokemon(_pokemon), _pokemon);
            copyGrid(invertPokemon(_newPokemon), _newPokemon);
            updateSingleRow(colStart, rowStart, colEnd, rowEnd);
            copyGrid(invertPokemon(_pokemon), _pokemon);
            copyGrid(invertPokemon(_newPokemon), _newPokemon);
        }

        public virtual void swapDitto(int row1, int col1, int row2, int col2)
        {
            if (_pokemon[row1, col1].GetType() == typeof(DittoToken))
            {
                markAllTokensOfSameTypeAsNull(_pokemon[row2, col2].GetType());
                _newPokemon[row1, col1] = null;
            }
            else if (_pokemon[row2, col2].GetType() == typeof(DittoToken))
            {
                markAllTokensOfSameTypeAsNull(_pokemon[row1, col1].GetType());
                _newPokemon[row2, col2] = null;
            }
        }

        public virtual void markRowSpecials(int row, int colStart, int numberOfSameTokens)
        {
            if (3 <= numberOfSameTokens)
            {
                IBasicPokemonToken currentToken;
                for (int i = 0; i < numberOfSameTokens; i++)
                {
                    currentToken = _pokemon[row, colStart + i];
                    if (currentToken.GetType().GetInterfaces().Contains(typeof(IFirstEvolutionPokemonToken)))
                    {
                        markSurroundingTokensNull(row, colStart + i);
                    }
                    else if (currentToken.GetType().GetInterfaces().Contains(typeof(ISecondEvolutionPokemonToken)))
                    {
                        markFullRowAndColumnAsNull(row, colStart + i);
                    }
                }
            }
        }

        public virtual void markAllTokensOfSameTypeAsNull(Type type)
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (_pokemon[row, col].GetType() == type)
                    {
                        _newPokemon[row, col] = null;
                    }
                }
            }
        }
        
        public virtual void markSurroundingTokensNull(int row, int col)
        {
            if (row - 1 > 0)
            {
                _newPokemon[row - 1, col] = null;
                if (col - 1 > 0) _newPokemon[row - 1, col - 1] = null;
                if (col + 1 < gridSize) _newPokemon[row - 1, col + 1] = null;
            }
            if (col - 1 > 0) _newPokemon[row, col - 1] = null;
            if (col + 1 < gridSize) _newPokemon[row, col + 1] = null;
            if (row + 1 < gridSize)
            {
                _newPokemon[row + 1, col] = null;
                if (col - 1 > 0) _newPokemon[row + 1, col - 1] = null;
                if (col + 1 < gridSize) _newPokemon[row + 1, col + 1] = null;
            }
        }

        public virtual void markFullRowAndColumnAsNull(int row, int col)
        {
            for (int currentRow = 0; currentRow < gridSize; currentRow++)
            {
                _newPokemon[currentRow, col] = null;
            }
            for (int currentCol = 0; currentCol < gridSize; currentCol++)
            {
                _newPokemon[row, currentCol] = null;
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
