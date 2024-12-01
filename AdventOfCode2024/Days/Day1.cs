using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day1(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day1.txt");

    [Fact]
    public void Part1()
    {
        var pairs = _lines
            .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .Select(l => (int.Parse(l[0]), int.Parse(l[1])))
            .ToList();
        
        var firstList = pairs
            .Select(p => p.Item1)
            .ToList();
        
        var secondList = pairs
            .Select(p => p.Item2)
            .ToList();
        
        firstList.Sort();
        secondList.Sort();

        var totalDistance = firstList
            .Zip(secondList)
            .Select(v => Math.Abs(v.First - v.Second))
            .Sum();
        
        output.WriteLine(totalDistance.ToString());
        
        Assert.Equal(2970687, totalDistance);
    }

    [Fact]
    public void Part2()
    {
        var pairs = _lines
            .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .Select(l => (int.Parse(l[0]), int.Parse(l[1])))
            .ToList();
        
        var firstList = pairs
            .Select(p => p.Item1)
            .ToList();
        
        var secondList = pairs
            .Select(p => p.Item2)
            .ToList();

        var occurrences = secondList
            .GroupBy(v => v)
            .ToDictionary(g => g.Key, g => g.Count());

        var totalSimilarityScore = firstList
            .Select(v => (long)v * occurrences.GetValueOrDefault(v, 0))
            .Sum();
        
        output.WriteLine(totalSimilarityScore.ToString());
        
        Assert.Equal(23963899, totalSimilarityScore);
    }
}