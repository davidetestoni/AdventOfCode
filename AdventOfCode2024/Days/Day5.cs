using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day5(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day5.txt");

    [Fact]
    public void Part1()
    {
        var dividerIndex = Array.IndexOf(_lines, string.Empty);

        var rules = _lines
            .Take(dividerIndex)
            .Select(l => l.Split('|'))
            .Select(s => (int.Parse(s[0]), int.Parse(s[1])))
            .ToArray();

        var updates = _lines
            .Skip(dividerIndex + 1)
            .Select(l => l.Split(',').Select(int.Parse).ToArray())
            .ToArray();
        
        var total = updates
            .Where(u => IsCorrectlyOrdered(u, rules))
            .Select(u => u[(u.Length - 1) / 2])
            .Sum();
        
        output.WriteLine(total.ToString());
        
        Assert.Equal(4924, total);
    }

    [Fact]
    public void Part2()
    {
        // I tried to do this part with a topological sort, but it didn't work out,
        // so I'm brute-forcing it
        
        var dividerIndex = Array.IndexOf(_lines, string.Empty);

        var rules = _lines
            .Take(dividerIndex)
            .Select(l => l.Split('|'))
            .Select(s => (int.Parse(s[0]), int.Parse(s[1])))
            .ToArray();

        var updates = _lines
            .Skip(dividerIndex + 1)
            .Select(l => l.Split(',').Select(int.Parse).ToArray())
            .Where(u => !IsCorrectlyOrdered(u, rules))
            .ToArray();

        var correctUpdates = updates
            .Select(u => u
                .Permutations()
                .Select(p => p.ToArray())
                .Single(p => IsCorrectlyOrdered(p, rules)));
        
        var total = correctUpdates
            .Select(u => u[(u.Length - 1) / 2])
            .Sum();
        
        output.WriteLine(total.ToString());
        
        // TODO: Takes too long, find another way...
    }

    private static bool IsCorrectlyOrdered(int[] update, (int, int)[] rules)
    {
        HashSet<int> seen = [];
        
        foreach (var page in update)
        {
            // If the rule is about this page being after
            // another page, make sure we've seen the other page
            if (rules.Any(rule => 
                    page == rule.Item2 &&
                    update.Contains(rule.Item1) &&
                    !seen.Contains(rule.Item1)))
            {
                return false;
            }

            seen.Add(page);
        }
        
        return true;
    }
}
