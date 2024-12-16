using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day16(ITestOutputHelper output)
{
    // It takes about a minute to get all the paths, but the result is correct.
    // A more correct solution would be to use a stateful shortest path algorithm.
    
    private readonly string[] _lines = File.ReadAllLines("Data/Day16.txt");

    [Fact]
    public void Part1()
    {
        var matrix = _lines
            .Select(l => l.ToArray())
            .ToArray();
        
        var start = matrix.Cells().First(c => c.Value == 'S');
        var shortest = int.MaxValue;
        WalkMaze(start, Direction.East, [(start.Y, start.X)], [],
            matrix, 0, ref shortest, [], [start]);
        
        output.WriteLine(shortest.ToString());
        
        Assert.Equal(99448, shortest);
    }

    [Fact]
    public void Part2()
    {
        var matrix = _lines
            .Select(l => l.ToArray())
            .ToArray();
        
        var start = matrix.Cells().First(c => c.Value == 'S');
        List<(List<Cell<char>> path, int cost)> paths = [];
        var shortest = int.MaxValue;
        WalkMaze(start, Direction.East, [(start.Y, start.X)], [],
            matrix, 0, ref shortest, paths, [start]);
        
        // Find all the shortest paths in the list of paths, then count
        // the cells that were visited at least by one of the paths
        var visited = paths
            .Where(p => p.cost == shortest)
            .SelectMany(p => p.path)
            .Distinct()
            .Count();
        
        output.WriteLine(visited.ToString());
        
        Assert.Equal(498, visited);
    }

    private static void WalkMaze(Cell<char> position, Direction direction,
        HashSet<(int y, int x)> visited, Dictionary<(int y, int x, Direction dir), int> cache,
        char[][] matrix, int current, ref int shortest,
        List<(List<Cell<char>> path, int cost)> paths, Cell<char>[] currentPath)
    {   
        // If we've reached the end, no more points to get
        if (matrix.At(position) == 'E')
        {
            shortest = Math.Min(shortest, current);
            paths.Add((currentPath.ToList(), current));
            return;
        }
        
        var neighbors = new Dictionary<Direction, Cell<char>>
        {
            [Direction.East] = matrix.RightCells(position).First(),
            [Direction.West] = matrix.LeftCells(position).First(),
            [Direction.North] = matrix.AboveCells(position).First(),
            [Direction.South] = matrix.BelowCells(position).First()
        };

        var viableCells = neighbors
            .Where(n => matrix.At(n.Value) != '#' && visited.Add((n.Value.Y, n.Value.X)))
            .ToList();

        if (viableCells.Count == 0)
        {
            return;
        }
        
        foreach (var (dir, cell) in viableCells)
        {
            // Calculate the cost of moving to this cell
            var cost = current + (dir == direction ? 0 : 1000) + 1;
            
            // If the cost is already higher than the shortest path, skip this cell
            if (cost > shortest)
            {
                continue;
            }
            
            // If the cache already has a better solution, skip this one
            if (cache.TryGetValue((cell.Y, cell.X, dir), out var cached) && cached < cost)
            {
                continue;
            }
            
            cache[(cell.Y, cell.X, dir)] = cost;
            
            WalkMaze(cell, dir, [..visited], cache, matrix,
                cost, ref shortest, paths, [..currentPath, cell]);
        }
    }

    private enum Direction
    {
        North,
        East,
        South,
        West
    }
}
