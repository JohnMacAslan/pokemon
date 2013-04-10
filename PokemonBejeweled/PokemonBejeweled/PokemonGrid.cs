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
        public int GamePlayScore { get; set; }
        private PokemonToken[,] _pokemon = new PokemonToken[gridSize, gridSize];
        public PokemonToken[,] Pokemon
        {
            get
            {
                return _pokemon;
            }
            set
            {
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        _pokemon[i, j] = value[i, j];
                    }
                }
            }
        }

        public PokemonGrid()
        {
            Pokemon = new PokemonToken[gridSize, gridSize];
            GamePlayScore = 0;
        }


        public void updateBoard()
        {
        }

        public Boolean isValidMove(int row1, int col1, int row2, int col2)
        {
           
            return false;
        }

        public void updateBoardAlgorithm()
        {
            PokemonToken[,] _newPokemon = _pokemon;
            markRowsOfSameTokenAsNull(_newPokemon);
            Pokemon = _newPokemon;
        }

        public void markRowsOfSameTokenAsNull(PokemonToken[,] _newPokemon)
        {
            int numberOfSameTokens;
            PokemonToken currentToken;
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
                            _newPokemon[row, col - numberOfSameTokens] = null;
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
                        _newPokemon[row, gridSize - numberOfSameTokens] = null;
                        numberOfSameTokens--;
                    }
                }
            }
        }

        public void markColumnsOfSameTokenAsNull(PokemonToken[,] _newPokemon)
        {
            int numberOfSameTokens;
            PokemonToken currentToken;
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
                            _newPokemon[row, col - numberOfSameTokens] = null;
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
                        _newPokemon[gridSize - numberOfSameTokens, col] = null;
                        numberOfSameTokens--;
                    }
                }
            }
        }


        public Boolean areThreeTokensInARow(int row, int col)
        {
            if (col > gridSize - 3 || row > gridSize - 1)
            {
                return false;
            } else {
                if (null == _pokemon[row, col] || null == _pokemon[row, col + 1] || null == _pokemon[row, col + 2])
                {
                    return false;
                }
                else
                {
                    Type pokemonType = _pokemon[row, col].GetType();
                    if (_pokemon[row, col + 1].GetType() == pokemonType && _pokemon[row, col + 2].GetType() == pokemonType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool areThreeTokensInACol(int row, int col)
        {
            if (row > gridSize - 3 || col > gridSize - 1)
            {
                return false;
            }
            else
            {
                if (null == _pokemon[row, col] || null == _pokemon[row + 1, col] || null == _pokemon[row + 2, col])
                {
                    return false;
                }
                else
                {
                    Type pokemonType = _pokemon[row, col].GetType();
                    if (_pokemon[row + 1, col].GetType() == pokemonType && _pokemon[row + 2, col].GetType() == pokemonType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
