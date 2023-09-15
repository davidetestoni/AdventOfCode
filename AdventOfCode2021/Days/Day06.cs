using System.Numerics;

namespace AdventOfCode2021.Days
{
    internal class Day06 : IDay
    {
        private const int valueAfterBirth = 6;
        private const int newbornValue = 8;

        public void Run()
        {
            var input = File.ReadAllText("Days/Day06.txt");
            var shoal = input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x))
                .ToList();

            var count = Age(shoal, 80);

            Console.WriteLine($"Individuals: {count}");

            count = Age(shoal, 256);

            Console.WriteLine($"Individuals: {count}");
        }

        private BigInteger Age(List<int> shoal, int days)
        {
            // Array with indices from 0 to 9
            var shoalCounts = Enumerable.Repeat(BigInteger.Zero, 9).ToArray();

            // Populate the ages array
            foreach (var s in shoal)
            {
                shoalCounts[s]++;
            }

            for (int i = 0; i < days; i++)
            {
                var zeroCount = shoalCounts[0];

                // Shift all other values
                for (int j = 1; j < shoalCounts.Length; j++)
                {
                    shoalCounts[j - 1] = shoalCounts[j];
                }

                // Newborns
                shoalCounts[8] = zeroCount;

                // Parents
                shoalCounts[6] += zeroCount;
            }

            return shoalCounts.Aggregate((a, b) => a + b);
        }

        private long AgeSlow(List<int> shoal, int days)
        {
            for (int i = 0; i < days; i++)
            {
                var dayCount = shoal.Count;

                Console.WriteLine($"Day: {i} | Count: {dayCount}");

                for (int j = 0; j < dayCount; j++)
                {
                    if (shoal[j] == 0)
                    {
                        shoal[j] = valueAfterBirth;
                        shoal.Add(newbornValue);
                        continue;
                    }

                    shoal[j]--;
                }
            }

            return shoal.Count;
        }
    }
}
