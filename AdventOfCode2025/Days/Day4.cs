using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2025.Days;

public class Day4(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day4.txt");

    private static bool CanAccess(char[][] matrix, Cell<char> cell)
        => cell == '@' && matrix.Neighbors(cell).Count(n => n == '@') < 4;
    
    [Fact]
    public void Part1()
    {
        var matrix = _lines.Select(l => l.ToCharArray()).ToArray();
        var accessibleRolls = matrix.Cells().Count(c => CanAccess(matrix, c));
        
        output.WriteLine(accessibleRolls.ToString());

        Assert.Equal(1428, accessibleRolls);
    }

    private bool TryRemoveRolls(char[][] matrix)
    {
        var count = 0;
        
        foreach (var cell in matrix.Cells())
        {
            if (CanAccess(matrix, cell))
            {
                matrix[cell.Y][cell.X] = 'x';
                count++;
            }
        }

        return count > 0;
    }

    [Fact]
    public void Part2()
    {
        // This could be optimized further by registering the neighbors in a dictionary
        // so that, once a neighbor is removed, we look it up and recalculate the
        // accessibility of the nearby rolls without having to scan the matrix each time.
        // Since it's just a few dozens scans, we can avoid doing it in this case
        
        var matrix = _lines.Select(l => l.ToCharArray()).ToArray();
        while (TryRemoveRolls(matrix)) { }

        var removed = matrix.Cells().Count(c => c == 'x');
        
        output.WriteLine(removed.ToString());

        Assert.Equal(8936, removed);
    }
}