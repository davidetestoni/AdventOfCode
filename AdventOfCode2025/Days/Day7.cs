using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2025.Days;

public class Day7(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day7.txt");

    [Fact]
    public void Part1()
    {
        // Assumptions:
        // 1. 'S' is always in the first row
        // 2. there's always a line of dots between rows of splitters
        // 3. there's always space to the left and right of a splitter to draw the new beams
        var matrix = _lines.Select(l => l.ToCharArray()).ToArray();
        var splits = 0;

        for (var y = 1; y < matrix.Length; y++)
        {
            for (var x = 0; x < matrix[y].Length; x++)
            {
                var aboveCell = matrix[y - 1][x];
                
                if (matrix[y][x] == '^' && aboveCell == '|')
                {
                    matrix[y][x - 1] = '|';
                    matrix[y][x + 1] = '|';
                    splits++;
                }
                else if (aboveCell is 'S' or '|')
                {
                    matrix[y][x] = '|';
                }
            }
        }
        
        output.WriteLine(splits.ToString());

        Assert.Equal(1504, splits);
    }

    private static long CountTimelines(char[][] matrix, Dictionary<(long y, long x), long> memo, long y, long x)
    {
        // Exit condition: final line
        if (y == matrix.Length - 1)
        {
            return matrix[y][x] == '^' ? 2 : 1;
        }
        
        // Handle beam continuation without splitting
        if (matrix[y][x] != '^')
        {
            if (!memo.TryGetValue((y + 1, x), out var value))
            {
                value = CountTimelines(matrix, memo, y + 1, x); 
                memo[(y + 1, x)] = value;
            }
            
            return value;
        }
        
        // Handle splitter
        if (!memo.TryGetValue((y + 1, x - 1), out var left))
        {
            left = CountTimelines(matrix, memo, y + 1, x - 1);
            memo[(y + 1, x - 1)] = left;
        }

        if (!memo.TryGetValue((y + 1, x + 1), out var right))
        {
            right = CountTimelines(matrix, memo, y + 1, x + 1);
            memo[(y + 1, x + 1)] = right;
        }

        return left + right;
    }

    [Fact]
    public void Part2()
    {
        // Remove lines made up of all dots since they are useless
        var matrix = _lines.Select(l => l.ToCharArray()).Where(l => l.Any(c => c != '.')).ToArray();
        var memo = new Dictionary<(long x, long y), long>();
        var timelines = CountTimelines(matrix, memo, 1, matrix.Row(0).First(c => c.Value is 'S').X);
        
        output.WriteLine(timelines.ToString());

        Assert.Equal(5137133207830, timelines);
    }
}