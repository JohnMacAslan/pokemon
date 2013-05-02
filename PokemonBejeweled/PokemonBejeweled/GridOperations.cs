using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    class GridOperations
    {
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

        public static void invertGrid(IBasicPokemonToken[,] gridToInvert)
        {
            int rowLength = gridToInvert.GetLength(0);
            int colLength = gridToInvert.GetLength(1);
            if (rowLength != colLength)
            {
                throw new ArithmeticException("Grid is not square.");
            }
            else
            {
                IBasicPokemonToken[,] invertedGrid = new IBasicPokemonToken[rowLength, colLength];
                for (int row = 0; row < rowLength; row++)
                {
                    for (int col = 0; col < colLength; col++)
                    {
                        invertedGrid[row, col] = gridToInvert[col, row];
                    }
                }
                copyGrid(invertedGrid, gridToInvert);
            }
        }
    }
}
