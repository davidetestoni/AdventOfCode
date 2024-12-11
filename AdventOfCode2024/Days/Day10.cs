using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day10(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day10.txt");

    [Fact]
    public void Part1()
    {
        var matrix = _lines
            .Select(l => l.Select(c => c - '0').ToArray())
            .ToArray();

        var trailHeads = matrix
            .Cells()
            .Where(c => c.Value == 0)
            .ToArray();
        
        var totalScore = trailHeads
            .Select(c => CalculateScore(c, matrix, []))
            .Sum();
        
        output.WriteLine(totalScore.ToString());
        
        Assert.Equal(468, totalScore);
    }

    [Fact]
    public void Part2()
    {
        var matrix = _lines
            .Select(l => l.Select(c => c - '0').ToArray())
            .ToArray();
        
        var trailHeads = matrix
            .Cells()
            .Where(c => c.Value == 0)
            .ToArray();

        var totalRating = trailHeads
            .Select(c => CalculateRating(c, matrix))
            .Sum();
        
        output.WriteLine(totalRating.ToString());
    }

    private static int CalculateScore(
        Cell<int> cell, int[][] matrix, HashSet<(int, int)> visited)
    {
        // If the current cell is a 9, exit with score 1 if it's the first
        // time we visit it, otherwise return 0
        if (cell.Value == 9)
        {
            return visited.Add((cell.X, cell.Y)) ? 1 : 0;
        }
        
        // Otherwise, the score is the sum of the scores calculated by
        // moving to an adjacent cell, given that the value of the adjacent
        // is greater than the current cell by 1
        return matrix
            .Neighbors(cell, diagonal: false)
            .Where(n => n.Value == cell.Value + 1)
            .Sum(n => CalculateScore(n, matrix, visited));
    }

    private static int CalculateRating(Cell<int> cell, int[][] matrix)
    {
        // If the current cell is a 9, exit with score 1
        if (cell.Value == 9)
        {
            return 1;
        }
        
        // Otherwise, the score is the sum of the scores calculated by
        // moving to an adjacent cell, given that the value of the adjacent
        // is greater than the current cell by 1
        return matrix
            .Neighbors(cell, diagonal: false)
            .Where(n => n.Value == cell.Value + 1)
            .Sum(n => CalculateRating(n, matrix));
    }
}
