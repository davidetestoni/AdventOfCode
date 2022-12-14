namespace AdventOfCode2022.Days
{
    internal class Day10 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day10.txt");
            var cycle = 0;
            var x = 1;
            var sum = 0;

            foreach (var line in lines)
            {
                if (line == "noop")
                {
                    cycle++;

                    if ((cycle - 20) % 40 == 0)
                    {
                        sum += x * cycle;
                    }
                }
                else if (line.StartsWith("addx"))
                {
                    cycle++;

                    if ((cycle - 20) % 40 == 0)
                    {
                        sum += x * cycle;
                    }

                    cycle++;

                    if ((cycle - 20) % 40 == 0)
                    {
                        sum += x * cycle;
                    }

                    var value = int.Parse(line[5..]);
                    x += value;
                }
            }

            Console.WriteLine(sum);

            var grid = new char[6, 40];
            x = 1;
            cycle = 0;

            // Cool way of filling the array
            /*
            char[] fillValue = Enumerable.Repeat('.', grid.Length).ToArray();
            Buffer.BlockCopy(fillValue, 0, grid, 0, fillValue.Length * sizeof(char));
            */

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = ' ';
                }
            }

            foreach (var line in lines)
            {
                if (line == "noop")
                {
                    cycle++;
                    UpdateDrawing(grid, cycle, x);
                }
                else if (line.StartsWith("addx"))
                {
                    cycle++;
                    UpdateDrawing(grid, cycle, x);

                    cycle++;
                    UpdateDrawing(grid, cycle, x);

                    var value = int.Parse(line[5..]);
                    x += value;
                }
            }

            DrawImage(grid);
        }

        private static void UpdateDrawing(char[,] grid, int cycle, int x)
        {
            var cursorRow = (cycle - 1) / 40;
            var cursorColumn = (cycle - 1) % 40;

            // Console.WriteLine($"cycle: {cycle} | x: {x}");

            if (Math.Abs(cursorColumn - x) <= 1)
            {
                grid[cursorRow, cursorColumn] = '#';
            }
            else
            {
                grid[cursorRow, cursorColumn] = '.';
            }

            // DrawImage(grid);
        }

        private static void DrawImage(char[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write(grid[i, j]);
                }

                Console.WriteLine();
            }
        }
    }
}
