namespace AdventOfCode2021.Days
{
    internal class Day01 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day01.txt");
            var increments = 0;

            for (int i = 1; i < lines.Length; i++)
            {
                var first = int.Parse(lines[i-1]);
                var second = int.Parse(lines[i]);
                
                if ((second - first) > 0)
                {
                    increments++;
                }
            }

            Console.WriteLine(increments);

            increments = 0;

            for (int i = 1; i < lines.Length - 2; i++)
            {
                var firstWindowSum = lines[(i-1)..(i+2)].Select(m => int.Parse(m)).Sum();
                var secondWindowSum = lines[i..(i + 3)].Select(m => int.Parse(m)).Sum();
                
                if ((secondWindowSum - firstWindowSum) > 0)
                {
                    increments++;
                }
            }

            Console.WriteLine(increments);
        }
    }
}
