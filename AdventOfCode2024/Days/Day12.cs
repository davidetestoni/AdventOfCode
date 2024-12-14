using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day12(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day12.txt");

    [Fact]
    public void Part1()
    {
        var matrix = _lines
            .Select(l => l.ToArray())
            .ToArray();

        var price = GetRegions(matrix)
            .Sum(r => r.Count * GetPerimeter(r, matrix));
        
        output.WriteLine(price.ToString());
        
        Assert.Equal(1446042, price);
    }

    [Fact]
    public void Part2()
    {
        var matrix = _lines
            .Select(l => l.ToArray())
            .ToArray();

        var price = GetRegions(matrix)
            .Sum(r => r.Count * GetSides(r, matrix));
        
        output.WriteLine(price.ToString());
    }

    private static List<List<Cell<char>>> GetRegions(char[][] matrix)
    {
        var visited = new HashSet<(int y, int x)>();
        var regions = new List<List<Cell<char>>>();

        foreach (var cell in matrix.Cells())
        {
            // If the cell has already been visited, skip it
            if (!visited.Add((cell.Y, cell.X)))
            {
                continue;
            }

            // Create a new region
            List<Cell<char>> region = [cell];
            GrowRegion(cell, matrix, region, visited);
            regions.Add(region);
        }
        
        return regions;
    }

    private static void GrowRegion(
        Cell<char> cell, char[][] matrix,
        List<Cell<char>> region, HashSet<(int y, int x)> visited)
    {
        var validCells = matrix
            .Neighbors(cell, diagonal: false)
            .Where(n => n.Value == cell.Value && !visited.Contains((n.Y, n.X)))
            .ToList();
        
        region.AddRange(validCells);
        
        validCells.ForEach(c => visited.Add((c.Y, c.X)));
        validCells.ForEach(c => GrowRegion(c, matrix, region, visited));
    }

    private static int GetPerimeter(IEnumerable<Cell<char>> region, char[][] matrix)
    {
        var perimeter = 0;

        foreach (var cell in region)
        {
            var neighbors = matrix
                .Neighbors(cell, diagonal: false)
                .ToArray();
            
            perimeter += neighbors.Count(n => n.Value != cell.Value)
                + (4 - neighbors.Length); // Borders of the matrix
        }
        
        return perimeter;
    }

    private static int GetSides(IEnumerable<Cell<char>> region, char[][] matrix)
    {
        // TODO: Implement
        return 0;
    }
}
