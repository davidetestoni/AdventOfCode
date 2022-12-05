namespace AdventOfCode2022.Days
{
    internal class Day03 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day03.txt");
            var totalPriority = 0;

            foreach (var line in lines)
            {
                var rucksackSize = line.Length / 2;
                var rucksack1String = line[..rucksackSize];
                var rucksack2String = line[rucksackSize..];

                var rucksack1 = new HashSet<char>(rucksack1String.ToCharArray());
                var rucksack2 = new HashSet<char>(rucksack2String.ToCharArray());
                var intersection = rucksack1.Intersect(rucksack2);
                var priority = intersection.Select(c => GetPriority(c)).Sum();

                totalPriority += priority;
            }

            Console.WriteLine(totalPriority);

            totalPriority = 0;

            foreach (var lineGroup in lines.Chunk(3))
            {
                var rucksack1 = new HashSet<char>(lineGroup[0].ToCharArray());
                var rucksack2 = new HashSet<char>(lineGroup[1].ToCharArray());
                var rucksack3 = new HashSet<char>(lineGroup[2].ToCharArray());
                var intersection = rucksack1.Intersect(rucksack2).Intersect(rucksack3);

                totalPriority += GetPriority(intersection.First());
            }

            Console.WriteLine(totalPriority);
        }

        private static int GetPriority(char c)
            => char.IsLower(c) ? c - 97 + 1 : c - 65 + 27;
    }
}
