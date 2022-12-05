namespace AdventOfCode2022.Days
{
    internal class Day04 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day04.txt");
            var included = 0;
            var overlapped = 0;
            
            foreach (var line in lines)
            {
                var rangeStrings = line.Split(',');
                var firstRangeString = rangeStrings[0].Split('-');
                var secondRangeString = rangeStrings[1].Split('-');
                var firstRange = (int.Parse(firstRangeString[0]), int.Parse(firstRangeString[1]));
                var secondRange = (int.Parse(secondRangeString[0]), int.Parse(secondRangeString[1]));

                if (RangeIncludes(firstRange, secondRange) || RangeIncludes(secondRange, firstRange))
                {
                    included++;
                }

                if (RangeOverlaps(firstRange, secondRange))
                {
                    overlapped++;
                }
            }

            Console.WriteLine(included);
            Console.WriteLine(overlapped);
        }

        // Check if included [3, 4] in [1, 4]
        private static bool RangeIncludes((int, int) outer, (int, int) inner)
            => inner.Item1 >= outer.Item1 && inner.Item2 <= outer.Item2;

        // [6, 8] overlaps with [4, 7]
        // [6, 8] overlaps with [7, 9]
        // [6, 8] does not overlap with [9, 11]
        // [6, 8] does not overlap with [3, 5]
        private static bool RangeOverlaps((int, int) first, (int, int) second)
            => first.Item2 >= second.Item1 && first.Item1 <= second.Item2;
    }
}
