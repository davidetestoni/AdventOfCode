namespace AdventOfCode2022.Days
{
    internal class Day01 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day01.txt");
            var set = new SortedSet<int>();
            var count = 0;

            foreach (var line in lines)
            {
                if (line == "")
                {
                    set.Add(count);
                    count = 0;
                    continue;
                }

                count += int.Parse(line);
            }

            Console.WriteLine(set.Last());
            
            // PART 2
            Console.WriteLine(set.TakeLast(3).Sum());
        }
    }
}
