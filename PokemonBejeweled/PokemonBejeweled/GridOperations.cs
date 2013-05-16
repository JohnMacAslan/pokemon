using System;
using PokemonBejeweled.Pokemon;

namespace PokemonBejeweled
{
    class GridOperations
    {
        /// <summary>
        /// Checks to see if two locations are adjacent. 
        /// </summary>
        /// <param name="row1">The row of the first location on the grid. </param>
        /// <param name="col1">The column of the first location on the grid. </param>
        /// <param name="row2">The row of the second location on the grid. </param>
        /// <param name="col2">The column of the second location on the grid. </param>
        /// <returns>True if the two locations are adjacent, false otherwise. </returns>
        public static bool arePiecesAdjacent(int row1, int col1, int row2, int col2)
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
        
        /// <summary>
        /// Copies a 2-d array of IBasicPokemonTokens to another 2-d array. 
        /// </summary>
        /// <param name="gridToCopy">The 2-d array from which to copy. </param>
        /// <param name="gridDestination">The 2-d array to copy to. </param>
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

        /// <summary>
        /// Inverts a square 2-dimensional array of IBasicPokemonTokens. 
        /// </summary>
        /// <param name="gridToInvert">The 2-d array to invert. </param>
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
