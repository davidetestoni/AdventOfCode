namespace AdventOfCode2021.Days
{
    internal class Day07 : IDay
    {
        public void Run()
        {
            var input = File.ReadAllText("Days/Day07.txt");
            var crabs = input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x))
                .ToArray();

            var maxPos = crabs.Max();
            var positions = Enumerable.Range(0, maxPos - 1);
            
            var minFuelCost = positions.Min(pos => CalcFuelCost(crabs, pos));
            Console.WriteLine($"Min fuel cost: {minFuelCost}");

            minFuelCost = positions.Min(pos => CalcFuelCost2(crabs, pos));
            Console.WriteLine($"Min fuel cost: {minFuelCost}");
        }

        private long CalcFuelCost(int[] crabs, int position)
        {
            var cost = 0L;

            foreach (var crab in crabs)
            {
                cost += Math.Abs(crab - position);
            }

            return cost;
        }

        private long CalcFuelCost2(int[] crabs, int position)
        {
            var cost = 0L;

            foreach (var crab in crabs)
            {
                var distance = Math.Abs(crab - position);

                // It's a bit slow...
                cost += Enumerable.Range(1, distance).Sum();
            }

            return cost;
        }
    }
}
