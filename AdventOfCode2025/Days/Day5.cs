using Xunit.Abstractions;
using Range = (long start, long end);

namespace AdventOfCode2025.Days;

public class Day5(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day5.txt");

    [Fact]
    public void Part1()
    {
        var ranges = _lines
            .Where(l => l.Contains('-'))
            .Select(l => l.Split('-'))
            .Select<string[], Range>(s => (long.Parse(s[0]), long.Parse(s[1])))
            .ToList();

        var ingredients = _lines
            .Where(l => !string.IsNullOrWhiteSpace(l) && !l.Contains('-'))
            .Select(long.Parse)
            .ToList();

        var fresh = 0;

        foreach (var ingredient in ingredients)
        {
            foreach (var range in ranges)
            {
                if (range.start <= ingredient && ingredient <= range.end)
                {
                    fresh++;
                    break;
                }
            }
        }
        
        output.WriteLine(fresh.ToString());

        Assert.Equal(733, fresh);
    }
    
    // |-------------------| outer
    //     |-----------|     inner
    private static bool IsIncluded(Range inner, Range outer)
        => outer.start <= inner.start && outer.end >= inner.end;
    
    // |------------|       left
    //        |-----------| right
    private static bool Overlaps(Range left, Range right)
        => left.start <= right.start 
           && left.end >= right.start // Also account for adjacent ranges
           && right.end >= left.end;

    private static bool TryMergeOverlappingRange(List<Range> ranges)
    {
        // We only do one pair at a time since the list gets modified
        // (this is not ideal in terms of speed but it works)
        
        var pairs = ranges.SelectMany((x, i) =>
            ranges.Skip(i + 1).Select(y => new { First = x, Second = y }));

        foreach (var pair in pairs)
        {
            // If one of the ranges is completely included in the other one, remove it
            if (IsIncluded(pair.First, pair.Second))
            {
                ranges.Remove(pair.First);
                return true;
            }

            if (IsIncluded(pair.Second, pair.First))
            {
                ranges.Remove(pair.Second);
                return true;
            }

            if (Overlaps(pair.First, pair.Second))
            {
                ranges.Remove(pair.First);
                ranges.Remove(pair.Second);
                ranges.Add((pair.First.start, pair.Second.end));
                return true;
            }
            
            if (Overlaps(pair.Second, pair.First))
            {
                ranges.Remove(pair.First);
                ranges.Remove(pair.Second);
                ranges.Add((pair.Second.start, pair.First.end));
                return true;
            }
        }

        return false;
    }

    [Fact]
    public void Part2()
    {
        var ranges = _lines
            .Where(l => l.Contains('-'))
            .Select(l => l.Split('-'))
            .Select<string[], Range>(s => (long.Parse(s[0]), long.Parse(s[1])))
            .ToList();

        while (TryMergeOverlappingRange(ranges)) { }

        var total = ranges.Sum(r => r.end - r.start + 1);
        
        output.WriteLine(total.ToString());

        Assert.Equal(345821388687084, total);
    }
}