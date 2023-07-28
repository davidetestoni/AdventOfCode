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

            Age(shoal, 80);

            Console.WriteLine($"Individuals: {shoal.Count}");

            // TODO: Takes too long, it's exponential, use different approach
            // (aggregate fish with same age into counters)

            Age(shoal, 256 - 80);

            Console.WriteLine($"Individuals: {shoal.Count}");
        }

        private void Age(List<int> shoal, int days)
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
        }
    }
}
