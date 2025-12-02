using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode2025.Days;

public partial class Day2(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day2.txt");

    [Fact]
    public void Part1()
    {
        var ranges = _lines[0]
            .Split(',')
            .Select(x => x.Split('-'))
            .Select<string[], (long start, long stop)>(x => (long.Parse(x[0]), long.Parse(x[1])));

        var sum = 0L;

        foreach (var range in ranges)
        {
            for (var i = range.start; i <= range.stop; i++)
            {
                if (NumericSequenceTwice().IsMatch(i.ToString()))
                {
                    sum += i;
                }
            }
        }
        
        output.WriteLine(sum.ToString());

        Assert.Equal(41294979841, sum);
    }

    [Fact]
    public void Part2()
    {
        var ranges = _lines[0]
            .Split(',')
            .Select(x => x.Split('-'))
            .Select<string[], (long start, long stop)>(x => (long.Parse(x[0]), long.Parse(x[1])));

        var sum = 0L;

        foreach (var range in ranges)
        {
            for (var i = range.start; i <= range.stop; i++)
            {
                if (NumericSequenceTwiceOrMore().IsMatch(i.ToString()))
                {
                    sum += i;
                }
            }
        }
        
        output.WriteLine(sum.ToString());

        Assert.Equal(66500947346, sum);
    }

    [GeneratedRegex(@"^(\d+)\1$")]
    private static partial Regex NumericSequenceTwice();
    
    [GeneratedRegex(@"^(\d+)\1+$")]
    private static partial Regex NumericSequenceTwiceOrMore();
}
