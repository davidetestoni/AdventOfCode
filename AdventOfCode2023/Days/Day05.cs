namespace AdventOfCode2023.Days;

internal class Day05 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day05.txt");

        var seeds = new List<long>();
        var seedSoilMap = new ResourceMap("seed-to-soil");
        var currentMap = seedSoilMap;

        // Part 1
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) || line.StartsWith("seed-to-soil"))
            {
                continue;
            }

            if (line.StartsWith("seeds:"))
            {
                seeds = line.Split(':')[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToList();
            }
            else if (line.EndsWith("map:"))
            {
                var newMap = new ResourceMap(line.Split(' ')[0]);
                currentMap.Next = newMap;
                currentMap = newMap;
            }
            else
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var destRangeStart = long.Parse(parts[0]);
                var sourceRangeStart = long.Parse(parts[1]);
                var rangeLength = long.Parse(parts[2]);

                currentMap.Add(new RangeInfo(
                    destRangeStart, sourceRangeStart, rangeLength));
            }
        }

        var minLocation = seeds.Select(seedSoilMap.Get).Min();
        Console.WriteLine($"Part 1: {minLocation}");
    }

    class ResourceMap
    {
        private readonly List<RangeInfo> _ranges = new();
        public ResourceMap? Next { get; set; }
        public string Name { get; }

        public ResourceMap(string name)
        {
            Name = name;
        }

        public void Add(RangeInfo rangeInfo)
        {
            _ranges.Add(rangeInfo);
        }

        public long Get(long value)
        {
            var mappedValue = value;

            foreach (var range in _ranges)
            {
                if (value >= range.SourceRangeStart &&
                    value < range.SourceRangeStart + range.RangeLength)
                {
                    mappedValue = range.DestinationRangeStart + 
                        (value - range.SourceRangeStart);

                    break;
                }
            }

            return Next is not null ? Next.Get(mappedValue) : mappedValue;
        }
    }

    struct RangeInfo
    {
        public long DestinationRangeStart { get; }
        public long SourceRangeStart { get; }
        public long RangeLength { get; }

        public RangeInfo(long destinationRangeStart,
            long sourceRangeStart, long rangeLength)
        {
            DestinationRangeStart = destinationRangeStart;
            SourceRangeStart = sourceRangeStart;
            RangeLength = rangeLength;
        }
    }
}
