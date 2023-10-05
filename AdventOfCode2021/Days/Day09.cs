namespace AdventOfCode2021.Days
{
    internal class Day09 : IDay
    {
        private readonly HashSet<(int x, int y)> _checkedPoints = new();

        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day09.txt");

            var m = lines.Select(l => l.ToCharArray().Select(c => c - '0').ToArray()).ToArray();
            var sum = 0;

            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m[i].Length; j++)
                {
                    var v = m[i][j];

                    // Check above
                    if (i - 1 >= 0 && m[i - 1][j] <= v) continue;

                    // Check left
                    if (j - 1 >= 0 && m[i][j - 1] <= v) continue;

                    // Check below
                    if (i + 1 < m.Length && m[i + 1][j] <= v) continue;

                    // Check right
                    if (j + 1 < m[i].Length && m[i][j + 1] <= v) continue;

                    sum += 1 + v;
                }
            }

            Console.WriteLine($"Total risk: {sum}");

            var basins = new List<int>();

            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m[i].Length; j++)
                {
                    basins.Add(GetBasinSize(m, i, j));
                }
            }

            var product = 1;

            foreach (var basin in basins.OrderByDescending(x => x).Take(3))
            {
                product *= basin;
            }

            Console.WriteLine($"Total basin sizes: {product}");
        }

        private int GetBasinSize(int[][] m, int i, int j)
        {
            var size = 1;

            // Exit condition: if this is a 9 or it's already been checked
            if (_checkedPoints.Contains((i, j)) || m[i][j] == 9)
            {
                return 0;
            }

            // Set this point as checked
            _checkedPoints.Add((i, j));

            // Check left
            if (i > 0)
            {
                size += GetBasinSize(m, i - 1, j);
            }

            // Check right
            if (i < m.Length - 1)
            {
                size += GetBasinSize(m, i + 1, j);
            }

            // Check up
            if (j > 0)
            {
                size += GetBasinSize(m, i, j - 1);
            }

            // Check down
            if (j < m[0].Length - 1)
            {
                size += GetBasinSize(m, i, j + 1);
            }

            return size;
        }
    }
}
