using Xunit.Abstractions;

namespace AdventOfCode2025.Days;

public class Day3(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day3.txt");

    [Fact]
    public void Part1()
    {
        var banks = _lines
            .Select(x => x.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray());

        var sum = 0L;

        foreach (var bank in banks)
        {
            var leftIndex = FindMaxIndex(bank[..^1]);
            var rightIndex = FindMaxIndex(bank[(leftIndex + 1)..]) + leftIndex + 1;

            sum += bank[leftIndex] * 10 + bank[rightIndex];
        }
        
        output.WriteLine(sum.ToString());

        Assert.Equal(17244, sum);
    }

    [Fact]
    public void Part2()
    {
        var banks = _lines
            .Select(x => x.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray());

        var sum = 0L;

        foreach (var bank in banks)
        {
            var currentIndex = -1;
            var indices = new List<int>();

            for (var i = 0; i < 12; i++)
            {
                var nextIndex = FindMaxIndex(bank[(currentIndex + 1)..^(11 - i)]) + currentIndex + 1;
                indices.Add(nextIndex);
                currentIndex = nextIndex;
            }

            sum += indices.Select((index, i) => bank[index] * (long)Math.Pow(10, 11 - i)).Sum();
        }
        
        output.WriteLine(sum.ToString());

        Assert.Equal(171435596092638, sum);
    }

    private static int FindMaxIndex(IList<int> bank)
        => bank.IndexOf(bank.Max());
}