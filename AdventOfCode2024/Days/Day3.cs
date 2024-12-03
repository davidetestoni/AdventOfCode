using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public partial class Day3(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day3.txt");

    [Fact]
    public void Part1()
    {
        var total = MultiplicationRegex().Matches(string.Join("", _lines))
            .Select(m => long.Parse(m.Groups[1].Value) * long.Parse(m.Groups[2].Value))
            .Sum();
        
        output.WriteLine(total.ToString());
        
        Assert.Equal(184122457L, total);
    }

    [Fact]
    public void Part2()
    {
        var matches = OperationRegex().Matches(string.Join("", _lines));

        var enabled = true;
        var total = 0L;

        foreach (Match match in matches)
        {
            switch (match.Groups[0].Value)
            {
                case var m when m.Contains("mul(") && enabled:
                    total += long.Parse(match.Groups[1].Value) * long.Parse(match.Groups[2].Value);
                    break;
                case var m when m.Contains("do()"):
                    enabled = true;
                    break;
                case var m when m.Contains("don't()"):
                    enabled = false;
                    break;
            }
        }
        
        output.WriteLine(total.ToString());
        
        Assert.Equal(107862689L, total);
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MultiplicationRegex();

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)")]
    private static partial Regex OperationRegex();
}
