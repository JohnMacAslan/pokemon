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
        public static int gridSize = 8;
        private Dictionary<int, Type> dict = new Dictionary<int, Type>();
        public int GamePlayScore { get; set; }
        private IBasicPokemonToken[,] _pokemon = new IBasicPokemonToken[gridSize, gridSize];
        public IBasicPokemonToken[,] Pokemon
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
            Pokemon = new IBasicPokemonToken[gridSize, gridSize];
            GamePlayScore = 0;

            dict.Add(1, typeof(BulbasaurToken));
            dict.Add(2, typeof(CharmanderToken));
            dict.Add(3, typeof(ChikoritaToken));
            dict.Add(4, typeof(CyndaquilToken));
            dict.Add(5, typeof(PichuToken));
            dict.Add(6, typeof(SquirtleToken));
            dict.Add(7, typeof(TotodileToken));
        }

        private bool piecesAreAdjacent(int row1, int col1, int row2, int col2)
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

        private int numberOfSameTokensFromToken(int row, int col)
        {
            Type tokenType = _pokemon[row, col].GetType();
            int numberOfSameColumnTokens = 1;
            int numberOfSameRowTokens = 1;

            int currentRow = row - 1;
            while (currentRow >= 0 && tokenType == _pokemon[currentRow, col].GetType())
            {
                numberOfSameColumnTokens++;
                currentRow--;
            }
            currentRow = row + 1;
            while (currentRow < gridSize && tokenType == _pokemon[currentRow, col].GetType())
            {
                numberOfSameColumnTokens++;
                currentRow++;
            }

            int currentCol = col - 1;
            while (currentCol >= 0 && tokenType == _pokemon[row, currentCol].GetType())
            {
                numberOfSameRowTokens++;
                currentCol--;
            }
            currentCol = col + 1;
            while (currentCol < gridSize && tokenType == _pokemon[row, currentCol].GetType())
            {
                numberOfSameRowTokens++;
                currentCol++;
            }
            return Math.Max(numberOfSameColumnTokens, numberOfSameRowTokens);
        }

        private IBasicPokemonToken updateMovedToken(int row, int col, int numberOfSameTokens)
        {
            Dictionary<int, Delegate> updateMovedToken;
            IBasicPokemonToken movedToken = _pokemon[row, col];
            switch (numberOfSameTokens)
            {
                case 3:
                    return null;
                case 4:
                    return movedToken.firstEvolvedToken();
                case 5:
                    return new DittoToken();
                case 6:
                    return movedToken.secondEvolvedToken();
                default:
                    return movedToken;
            }
        }

        public void updateBoard(int row1, int col1, int row2, int col2)
        {
            if (piecesAreAdjacent(row1, col1, row2, col2))
            {
                IBasicPokemonToken firstNewToken;
                IBasicPokemonToken secondNewToken;
                int numberOfSameTokensFromFirstToken = numberOfSameTokensFromToken(row1, col1);
                int numberOfSameTokensFromSecondToken = numberOfSameTokensFromToken(row2, col2);
                if (3 >= numberOfSameTokensFromFirstToken || 3 >= numberOfSameTokensFromSecondToken)
                {
                    firstNewToken = updateMovedToken(row1, col1, numberOfSameTokensFromFirstToken);
                    secondNewToken = updateMovedToken(row1, col1, numberOfSameTokensFromSecondToken);
                    IBasicPokemonToken[,] newPokemon = new IBasicPokemonToken[gridSize, gridSize];
                    copyGrid(_pokemon, newPokemon);
                    markColumnsOfSameTokenAsNull(newPokemon);
                    markRowsOfSameTokenAsNull(newPokemon);
                    copyGrid(newPokemon, _pokemon);
                    _pokemon[row1, col1] = firstNewToken;
                    _pokemon[row2, col2] = secondNewToken;
                    pullDownTokens();
                    addNewTokens();
                }
            }
        }

        private void markRowsOfSameTokenAsNull(IBasicPokemonToken[,] newPokemon)
        {
            int numberOfSameTokens;
            IBasicPokemonToken currentToken;
            for (int row = 0; row < gridSize; row++)
            {
                currentToken = _pokemon[row, 0];
                numberOfSameTokens = 1;
                for (int col = 1; col < gridSize; col++)
                {
                    if (currentToken.GetType() == _pokemon[row, col].GetType())
                    {
                        numberOfSameTokens++;
                    }
                    else if (3 <= numberOfSameTokens)
                    {
                        while (numberOfSameTokens > 0)
                        {
                            newPokemon[row, col - numberOfSameTokens] = null;
                            numberOfSameTokens--;
                        }
                    }
                    else
                    {
                        currentToken = _pokemon[row, col];
                    }
                }
                if (3 <= numberOfSameTokens)
                {
                    while (numberOfSameTokens > 0)
                    {
                        newPokemon[row, gridSize - numberOfSameTokens] = null;
                        numberOfSameTokens--;
                    }
                }
            }
        }

        private void markColumnsOfSameTokenAsNull(IBasicPokemonToken[,] newPokemon)
        {
            int numberOfSameTokens;
            IBasicPokemonToken currentToken;
            for (int col = 0; col < gridSize; col++)
            {
                currentToken = _pokemon[0, col];
                numberOfSameTokens = 1;
                for (int row = 1; row < gridSize; row++)
                {
                    if (currentToken.GetType() == _pokemon[row, col].GetType())
                    {
                        numberOfSameTokens++;
                    }
                    else if (3 <= numberOfSameTokens)
                    {
                        while (numberOfSameTokens > 0)
                        {
                            newPokemon[row - numberOfSameTokens, col] = null;
                            numberOfSameTokens--;
                        }
                    }
                    else
                    {
                        currentToken = _pokemon[row, col];
                    }
                }
                if (3 <= numberOfSameTokens)
                {
                    while (numberOfSameTokens > 0)
                    {
                        newPokemon[gridSize - numberOfSameTokens, col] = null;
                        numberOfSameTokens--;
                    }
                }
            }
        }

        private void pullDownTokens()
        {
            for (int col = 0; col < gridSize; col++)
            {
                for (int row = gridSize - 1; row > 0; row--)
                {
                    if (_pokemon[row, col] == null)
                    {
                        _pokemon[row, col] = _pokemon[row - 1, col];
                        _pokemon[row - 1, col] = null;
                    }
                }
            }
        }

        private void addNewTokens()
        {
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (_pokemon[row, col] == null)
                    {
                        _pokemon[row, col] = new CharmanderToken(); // Cause charmander rocks!
                    }
                }
            }
        }

        private IBasicPokemonToken generateNewPokemon()
        {
            Random rand = new Random();
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
    }
}
