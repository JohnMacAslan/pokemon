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

        public static void printGrid(IBasicPokemonToken[,] grid)
        {
            int rowLength = grid.GetLength(0);
            int colLength = grid.GetLength(1);
            Dictionary<Type, int> dict = new Dictionary<Type, int>();

            dict.Add(typeof(BulbasaurToken), 1);
            dict.Add(typeof(CharmanderToken), 2);
            dict.Add(typeof(ChikoritaToken), 3);
            dict.Add(typeof(CyndaquilToken), 4);
            dict.Add(typeof(PichuToken), 5);
            dict.Add(typeof(SquirtleToken), 6);
            dict.Add(typeof(TotodileToken), 7);
            Console.Out.WriteLine("--------");
            for (int row = 0; row < rowLength; row++)
            {
                for (int col = 0; col < colLength; col++)
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
