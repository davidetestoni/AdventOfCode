using System.Diagnostics;

namespace AdventOfCode2023.Days;

internal class Day06 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day06.txt");

        var times = lines[0].Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToList();

        var distances = lines[1].Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToList();

        var races = times.Zip(distances)
            .Select(td => new Race(td.First, td.Second))
            .ToArray();

        var mult = 1L;
        var winnable = 0L;

        foreach (var race in races)
        {
            winnable = 0L;

            for (var i = 0; i <= race.Time; i++)
            {
                // i is the time we hold down the button
                // so it's the speed we reach (mm/ms)
                if (race.Distance < race.CalcDistance(i))
                {
                    winnable++;
                }
            }

            mult *= winnable;
        }

        Console.WriteLine($"Part 1: {mult}");

        var time = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
        var distance = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));
        var bigRace = new Race(time, distance);

        var firstWinnable = 0L;
        var lastWinnable = 0L;

        for (var i = 0; i <= time; i++)
        {
            if (distance < bigRace.CalcDistance(i))
            {
                firstWinnable = i;
                break;
            }
        }

        for (var i = time; i >= 0; i--)
        {
            if (distance < bigRace.CalcDistance(i))
            {
                lastWinnable = i;
                break;
            }
        }

        winnable = lastWinnable - firstWinnable + 1;

        Console.WriteLine($"Part 2: {winnable}");
    }

    struct Race
    {
        public long Time { get; set; }
        public long Distance { get; set; }

        public Race(long time, long distance)
        {
            Time = time;
            Distance = distance;
        }

        public long CalcDistance(long time)
        {
            return time * (Time - time);
        }
    }
}
