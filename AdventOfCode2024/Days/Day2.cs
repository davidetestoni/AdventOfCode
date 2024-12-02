using System.Text.Json;
using AdventOfCode2024.Extensions;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day2(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day2.txt");

    [Fact]
    public void Part1()
    {
        var reports = _lines
            .Select(r => r.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .Select(r => r.Select(int.Parse).ToArray())
            .ToList();
        
        // There are no negative numbers, and there are at least 2
        // levels in each report.
        var safeReportsCount = reports.Count(IsSafe);
        
        output.WriteLine(safeReportsCount.ToString());
        
        Assert.Equal(224, safeReportsCount);
    }

    [Fact]
    public void Part2()
    {
        var reports = _lines
            .Select(r => r.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .Select(r => r.Select(int.Parse).ToArray())
            .ToList();

        var safeReportsCount = reports
            // Map each report to all its variants, removing one
            // element at a time, and check if any of them is safe.
            .Count(r => Enumerable.Range(0, r.Length)
                .Select(i => r.ExcludingIndex(i).ToArray())
                .Any(IsSafe));
        
        output.WriteLine(safeReportsCount.ToString());
        
        Assert.Equal(293, safeReportsCount);
    }

    private static bool IsSafe(int[] report)
        => report.IsMonotonic(report[1] > report[0], strict: true) &&
           report.AdjacentPairs()
               .All(p => Math.Abs(p.Second - p.First) <= 3);
}
