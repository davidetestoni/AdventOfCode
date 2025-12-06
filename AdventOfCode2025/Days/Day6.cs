using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2025.Days;

public class Day6(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day6.txt");

    [Fact]
    public void Part1()
    {
        // We assume all split lines have the same amount of numbers
        var splitLines = _lines
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        var sum = 0L;

        for (var i = 0; i < splitLines[0].Length; i++)
        {
            var operation = splitLines[^1][i][0];

            sum += splitLines[..^1]
                .SelectMany<string[], long>(l => [long.Parse(l[i])])
                .Aggregate((a, b) => operation == '+' ? a + b : a * b);
        }
        
        output.WriteLine(sum.ToString());

        Assert.Equal(4805473544166, sum);
    }

    [Fact]
    public void Part2()
    {
        var matrix = _lines.Select(l => l.ToCharArray()).ToArray();
        var sum = 0L;
        var operands = new List<long>();
        
        for (var i = matrix[0].Length - 1; i >= 0; i--)
        {
            // Continue on empty column
            if (matrix.Column(i).All(c => c.Value == ' '))
            {
                continue;
            }
            
            // Number column: parse the number
            var number = long.Parse(
                new string(
                    matrix
                        .Column(i)
                        .Take(matrix.Length - 1)
                        .Select(c => c.Value)
                        .ToArray()));

            operands.Add(number);
            
            // If we have an operator in the last row, we're done with the group of numbers
            if (matrix[^1][i] != ' ')
            {
                sum += operands.Aggregate((a, b) => matrix[^1][i] == '+' ? a + b : a * b);
                operands.Clear();
            }
        }
        
        output.WriteLine(sum.ToString());

        Assert.Equal(8907730960817, sum);
    }
}