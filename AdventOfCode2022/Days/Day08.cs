namespace AdventOfCode2022.Days
{
    internal class Day08 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day08.txt");
            var rows = lines.Length;
            var columns = lines[0].Length;
            var grid = new int[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    grid[i, j] = CharToInt(lines[i][j]);
                }
            }

            var visible = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // Console.WriteLine($"Checking visibility of ({i},{j})");
                    if (IsVisible(grid, i, j))
                    {
                        // Console.WriteLine("VISIBLE!");
                        visible++;
                    }
                }
            }

            Console.WriteLine(visible);

            // Unoptimized but cool version that uses Linq
            /*
            var topScenicScore = Enumerable.Range(0, rows)
                .SelectMany(i => Enumerable.Range(0, columns), (i, j) => (i, j))
                .Max(ij => GetScenicScore(grid, ij.i, ij.j));
            */

            var topScenicScore = 0L;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    topScenicScore = Math.Max(
                        topScenicScore, GetScenicScore(grid, i, j));
                }
            }

            Console.WriteLine(topScenicScore);
        }

        private static int CharToInt(char c) => c - '0';

        private static bool IsVisible(int[,] grid, int row, int column)
        {
            var treeHeight = grid[row, column];
            var rows = grid.GetLength(0);
            var columns = grid.GetLength(1);
            var isVisible = true;

            if (row == 0 || row == rows - 1 || column == 0 || column == columns - 1)
            {
                return true;
            }

            // Check upwards
            for (int i = row - 1; i >= 0; i--)
            {
                // Console.WriteLine($"Checking ({i},{column})");
                if (grid[i, column] >= treeHeight)
                {
                    isVisible = false;
                    break;
                }
            }

            if (isVisible)
            {
                return true;
            }

            isVisible = true;

            // Check downwards
            for (int i = row + 1; i < rows; i++)
            {
                // Console.WriteLine($"Checking ({i},{column})");
                if (grid[i, column] >= treeHeight)
                {
                    isVisible = false;
                    break;
                }
            }

            if (isVisible)
            {
                return true;
            }

            isVisible = true;

            // Check left
            for (int j = column - 1; j >= 0; j--)
            {
                // Console.WriteLine($"Checking ({row},{j})");
                if (grid[row, j] >= treeHeight)
                {
                    isVisible = false;
                    break;
                }
            }

            if (isVisible)
            {
                return true;
            }

            isVisible = true;

            // Check right
            for (int j = column + 1; j < columns; j++)
            {
                // Console.WriteLine($"Checking ({row},{j})");
                if (grid[row, j] >= treeHeight)
                {
                    isVisible = false;
                    break;
                }
            }

            if (isVisible)
            {
                return true;
            }

            return false;
        }

        private static long GetScenicScore(int[,] grid, int row, int column)
        {
            long up = 0, down = 0, left = 0, right = 0;
            var treeHeight = grid[row, column];
            var rows = grid.GetLength(0);
            var columns = grid.GetLength(1);

            // Check upwards
            for (int i = row - 1; i >= 0; i--)
            {
                up++;

                if (grid[i, column] >= treeHeight)
                {
                    break;
                }
            }

            // Check downwards
            for (int i = row + 1; i < rows; i++)
            {
                down++;

                if (grid[i, column] >= treeHeight)
                {
                    break;
                }
            }

            // Check left
            for (int j = column - 1; j >= 0; j--)
            {
                left++;

                if (grid[row, j] >= treeHeight)
                {
                    break;
                }
            }

            // Check right
            for (int j = column + 1; j < columns; j++)
            {
                right++;

                if (grid[row, j] >= treeHeight)
                {
                    break;
                }
            }

            return up * down * left * right;
        }
    }
}
