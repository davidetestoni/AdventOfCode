using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day19(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day19.txt");

    [Fact]
    public void Part1()
    {
        var towels = _lines[0].Split(", ");

        var sequences = _lines
            .Skip(2)
            .ToList();

        var towelsRegex = string.Join('|', towels);
        var regex = new Regex($"^(?:{towelsRegex})+$");
        
        var valid = sequences
            .Count(s => regex.IsMatch(s));
        
        output.WriteLine(valid.ToString());
        
        Assert.Equal(216, valid);
    }

    [Fact]
    public void Part2()
    {
        var towels = _lines[0].Split(", ");

        var sequences = _lines
            .Skip(2)
            .ToList();
        
        Dictionary<string, long> cache = [];
        
        var combinations = sequences
            .Sum(s => CountPatterns(towels, s, cache));
        
        output.WriteLine(combinations.ToString());
    }

    private static long CountPatterns(string[] towels, string pattern,
        Dictionary<string, long> cache)
    {
        // If the pattern is empty, return 1
        if (pattern.Length == 0)
        {
            return 1;
        }
        
        // If the pattern is in the cache, return the value
        if (cache.TryGetValue(pattern, out var value))
        {
            return value;
        }
        
        var total = 0L;
        
        foreach (var towel in towels)
        {
            // If the pattern does not start with the towel, skip it
            if (!pattern.StartsWith(towel))
            {
                continue;
            }
            
            var subPattern = pattern[towel.Length..];
            var count = CountPatterns(towels, subPattern, cache);
            total += count;
            cache[subPattern] = count;
        }
        
        return total;
    }
}
