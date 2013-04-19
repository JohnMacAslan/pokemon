using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    public class PokemonGrid
    {
        public delegate void PullDownTokensEventHandler(object source);
        public event PullDownTokensEventHandler PullDownTokens;
        public static int gridSize = 8;
        private Dictionary<int, Type> dict = new Dictionary<int, Type>();
        public int GamePlayScore { get; set; }
        private Random rand = new Random();
        private IBasicPokemonToken[,] _pokemon = new IBasicPokemonToken[gridSize, gridSize];
        private IBasicPokemonToken[,] _newPokemon = new IBasicPokemonToken[gridSize, gridSize];
        internal IBasicPokemonToken[,] NewPokemon
        {
            get
            {
                return _newPokemon;
            }
            set
            {
                copyGrid(value, _newPokemon);
            }
        }
        internal IBasicPokemonToken[,] Pokemon
        {
            get
            {
                return _pokemon;
            }
            set
            {
                copyGrid(value, _pokemon);
            }
        }

        public PokemonGrid()
        {
            GamePlayScore = 0;
            dict.Add(1, typeof(BulbasaurToken));
            dict.Add(2, typeof(CharmanderToken));
            dict.Add(3, typeof(ChikoritaToken));
            dict.Add(4, typeof(CyndaquilToken));
            dict.Add(5, typeof(PichuToken));
            dict.Add(6, typeof(SquirtleToken));
            dict.Add(7, typeof(TotodileToken));
            generateGrid();
        }

        public void generateGrid()
        {
            _pokemon = new IBasicPokemonToken[gridSize, gridSize];
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    _pokemon[row, col] = generateNewPokemon();
                }
            }
            copyGrid(_pokemon, _newPokemon);
            updateAllColumns();
            updateAllRows();
            while (!haveGridsStabilized())
            {
                pullDownTokens();
                copyGrid(_pokemon, _newPokemon);
                updateAllColumns();
                updateAllRows();
            }
        }

        public bool haveGridsStabilized()
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

        public void updateBoard()
        {
            if (!haveGridsStabilized())
            {
                pullDownTokens();
                copyGrid(_pokemon, _newPokemon);
                updateAllColumns();
                updateAllRows();
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
                markSpecials(rowStart, colStart, numberOfSameTokens);
            }
        }

        public virtual void markNullRow(int rowStart, int colStart, int numberOfSameTokens)
        {
            if (3 <= numberOfSameTokens)
            {
                int col = colStart;
                while (col < colStart + numberOfSameTokens)
                {
                    _newPokemon[rowStart, col++] = null;
                }
            }
        }

        public virtual void updateSingleColumn(int rowStart, int colStart, int rowEnd, int colEnd)
        {
            IBasicPokemonToken startToken = _pokemon[rowStart, colStart];
            int numberOfSameTokens = 1;

            int currentRow = rowStart - 1;
            while (currentRow >= 0 && startToken.isSameSpecies(_pokemon[currentRow, colStart]))
            {
                numberOfSameTokens++;
                currentRow--;
            }
            currentRow = rowStart + 1;
            while (currentRow < gridSize && startToken.isSameSpecies(_pokemon[currentRow, colStart]))
            {
                numberOfSameTokens++;
                currentRow++;
            }
            if (3 <= numberOfSameTokens)
            {
                _newPokemon[rowStart, colStart] = _pokemon[rowStart, colStart];
                _newPokemon[rowEnd, colEnd] = _pokemon[rowEnd, colEnd];
                markNullColumn(currentRow - numberOfSameTokens, colStart, numberOfSameTokens);
                markSpecials(rowStart, colStart, numberOfSameTokens);
            }
        }

        public virtual void markNullColumn(int rowStart, int colStart, int numberOfSameTokens)
        {
            if (3 <= numberOfSameTokens)
            {
                int row = rowStart;
                while (row < rowStart + numberOfSameTokens)
                {
                    _newPokemon[row++, colStart] = null;
                }
            }
        }

        public virtual void markSpecials(int row, int col, int numberOfSameTokens)
        {
            IBasicPokemonToken movedToken = _pokemon[row, col];
            if (movedToken.GetType().GetInterfaces().Contains(typeof(IFirstEvolutionPokemonToken)))
            {
                markSurroundingTokensNull(row, col);
            }
            else if (movedToken.GetType().GetInterfaces().Contains(typeof(ISecondEvolutionPokemonToken)))
            {
                markFullRowAndColumnAsNull(row, col);
            }
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
                        markSpecials(row, col - numberOfSameTokens, numberOfSameTokens);
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
                    markSpecials(row, gridSize - numberOfSameTokens, numberOfSameTokens);
                }
            }
        }

        public virtual void updateAllColumns()
        {
            int numberOfSameTokens;
            IBasicPokemonToken currentToken;
            for (int col = 0; col < gridSize; col++)
            {
                currentToken = _pokemon[0, col];
                numberOfSameTokens = 1;
                for (int row = 1; row < gridSize; row++)
                {
                    if (currentToken.isSameSpecies(_pokemon[row, col]))
                    {
                        numberOfSameTokens++;
                    }
                    else if (3 <= numberOfSameTokens)
                    {
                        markNullColumn(row - numberOfSameTokens, col, numberOfSameTokens);
                        markSpecials(row - numberOfSameTokens, col, numberOfSameTokens);
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
                    markNullColumn(gridSize - numberOfSameTokens, col, numberOfSameTokens);
                    markSpecials(gridSize - numberOfSameTokens, col, numberOfSameTokens);
                }
            }
        }

        internal void pullDownTokens()
        {
            copyGrid(_newPokemon, _pokemon);
            printGrid(_pokemon);
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
            if (PullDownTokens != null) PullDownTokens(this);
            printGrid(_pokemon);
        }

        private IBasicPokemonToken generateNewPokemon()
        {
            int pokeNumber = rand.Next(1, 8);
            return (IBasicPokemonToken)Activator.CreateInstance(dict[pokeNumber]);
        }

        public static void copyGrid(IBasicPokemonToken[,] gridToCopy, IBasicPokemonToken[,] gridDestination)
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    gridDestination[row, col] = gridToCopy[row, col];
                }
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
    }
}
