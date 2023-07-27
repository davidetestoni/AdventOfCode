namespace AdventOfCode2021.Days
{
    record Point(int X, int Y)
    {

    }

    record Line(Point P, Point Q)
    {
        public bool IsHorizontal => P.Y == Q.Y;
        public bool IsVertical => P.X == Q.X;
    }

    class Grid
    {
        private int[,] _grid = new int[0, 0] { };
        private int _maxIndex = 0;

        public void Init(int maxIndex)
        {
            _maxIndex = maxIndex;

            for (int i = 0; i < maxIndex; i++)
            {
                for (int j = 0; j < maxIndex; j++)
                {
                    _grid[i, j] = 0;
                }
            }
        }

        // Draws a generic horizonta, vertical or diagonal line on the grid
        public void DrawLine(Line line)
        {

        }

        public int CountOverlaps()
        {
            var overlaps = 0;

            for (int i = 0; i < _maxIndex; i++)
            {
                for (int j = 0; j < _maxIndex; j++)
                {
                    if (_grid[i, j] >= 2)
                    {
                        overlaps++;
                    }
                }
            }

            return overlaps;
        }
    }

    internal class Day05 : IDay
    {
        public void Run()
        {
            Run(calcDiagonals: false);
            Run(calcDiagonals: true);
        }

        private void Run(bool calcDiagonals)
        {
            var allPairs = File.ReadAllLines("Days/Day05.txt");
            var lines = allPairs.Select(ParseLineFromPairs);

            var maxIndex = lines.Max(l => new int[] { l.P.X, l.P.Y, l.Q.X, l.Q.Y }.Max()) + 1;
            var grid = new int[maxIndex, maxIndex];

            for (int i = 0; i < maxIndex; i++)
            {
                for (int j = 0; j < maxIndex; j++)
                {
                    grid[i, j] = 0;
                }
            }

            var horizontalLines = lines.Where(l => l.IsHorizontal);
            var verticalLines = lines.Where(l => l.IsVertical);

            foreach (var line in horizontalLines)
            {
                var min = Math.Min(line.P.X, line.Q.X);
                var max = Math.Max(line.P.X, line.Q.X);

                for (int i = min; i <= max; i++)
                {
                    grid[i, line.P.Y]++;
                }
            }

            foreach (var line in verticalLines)
            {
                var min = Math.Min(line.P.Y, line.Q.Y);
                var max = Math.Max(line.P.Y, line.Q.Y);

                for (int i = min; i <= max; i++)
                {
                    grid[line.P.X, i]++;
                }
            }

            if (calcDiagonals)
            {
                var diagonalLines = lines.Where(l => !l.IsHorizontal && !l.IsVertical);

                foreach (var line in diagonalLines)
                {
                    // Calc the left and right point
                    var leftPoint = line.P.X < line.Q.X ? line.P : line.Q;
                    var rightPoint = line.P.X < line.Q.X ? line.Q : line.P;

                    var xDist = rightPoint.X - leftPoint.X;

                    // (1,3) -> (3,1) line going North-East
                    if (leftPoint.Y > rightPoint.Y)
                    {
                        for (int i = 0; i <= xDist; i++)
                        {
                            grid[leftPoint.X + i, leftPoint.Y - i]++;
                        }
                    }

                    // (1,1) -> (3,3) line going South-East
                    else
                    {
                        for (int i = 0; i <= xDist; i++)
                        {
                            grid[leftPoint.X + i, leftPoint.Y + i]++;
                        }
                    }
                }
            }

            var overlaps = 0;

            for (int i = 0; i < maxIndex; i++)
            {
                for (int j = 0; j < maxIndex; j++)
                {
                    if (grid[i, j] >= 2)
                    {
                        overlaps++;
                    }   
                }
            }

            Console.WriteLine($"Overlaps: {overlaps}");
        }

        private static Line ParseLineFromPairs(string value)
        {
            var split = value.Replace(" -> ", ",").Split(',');
            return new Line(
                new Point(int.Parse(split[0]), int.Parse(split[1])),
                new Point(int.Parse(split[2]), int.Parse(split[3]))
            );
        }
    }
}
