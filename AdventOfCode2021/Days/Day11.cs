namespace AdventOfCode2021.Days
{
    internal class Day11 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day11.txt");

            var m = lines
                .Select(l => l.ToCharArray().Select(c => c - '0').ToArray())
                .ToArray();

            
            var flashes = Enumerable.Range(0, 100)
                .Select(_ => ExecuteStep(m)).Sum();

            Console.WriteLine($"Flashes: {flashes}");

            var step = 0L;

            m = lines
                .Select(l => l.ToCharArray().Select(c => c - '0').ToArray())
                .ToArray();

            while (!AllFlashed(m))
            {
                step++;
                ExecuteStep(m);
            }

            Console.WriteLine($"All flashed on step: {step}");
        }

        private static long ExecuteStep(int[][] m)
        {
            var alreadyFlashed = new HashSet<(int i, int j)>();

            // First increase all levels by 1
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m[i].Length; j++)
                {
                    m[i][j]++;
                }
            }

            var flashes = 0L;
            long roundFlashes;

            // As long as there were flashes, loop and flash
            do
            {
                roundFlashes = Flash(m, alreadyFlashed);
                flashes += roundFlashes;
            } while (roundFlashes > 0);

            // Reset energy to 0 for all octopi that flashed
            foreach (var (i, j) in alreadyFlashed)
            {
                m[i][j] = 0;
            }

            return flashes;
        }

        private static bool AllFlashed(int[][] m)
            => m.All(l => l.All(x => x == 0));

        private static long Flash(int[][] m, HashSet<(int i, int j)> flashed)
        {
            var flashes = 0L;

            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m[i].Length; j++)
                {
                    // If the energy is low, don't flash
                    if (m[i][j] <= 9)
                    {
                        continue;
                    }

                    // If it already flashed, do not flash again
                    if (flashed.Contains((i, j)))
                    {
                        continue;
                    }

                    // Increase the energy level of neighbours by 1
                    
                    // Top
                    if (i > 0)
                    {
                        m[i - 1][j]++;
                    }

                    // Top-right
                    if (i > 0 && j < m[i].Length - 1)
                    {
                        m[i - 1][j + 1]++;
                    }

                    // Right
                    if (j < m[i].Length - 1)
                    {
                        m[i][j + 1]++;
                    }

                    // Bottom-right
                    if (i < m.Length - 1 && j < m[i].Length - 1)
                    {
                        m[i + 1][j + 1]++;
                    }

                    // Bottom
                    if (i < m.Length - 1)
                    {
                        m[i + 1][j]++;
                    }

                    // Bottom-left
                    if (i < m.Length - 1 && j > 0)
                    {
                        m[i + 1][j - 1]++;
                    }

                    // Left
                    if (j > 0)
                    {
                        m[i][j - 1]++;
                    }

                    // Top-left
                    if (i > 0 && j > 0)
                    {
                        m[i - 1][j - 1]++;
                    }

                    flashes++;
                    flashed.Add((i, j));
                }
            }

            return flashes;
        }
    }
}
