using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day6(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day6.txt");

    [Fact]
    public void Part1()
    {
        var patrol = new Patrol(
            _lines.Select(l => l.ToCharArray()).ToArray());

        while (!patrol.Exited)
        {
            patrol.PerformIteration();
        }
        
        output.WriteLine(patrol.VisitedCount.ToString());
        
        Assert.Equal(5269, patrol.VisitedCount);
    }

    [Fact]
    public void Part2()
    {
        var matrix = _lines.Select(l => l.ToCharArray()).ToArray();
        var loops = 0;
        
        // For each empty cell, create a new matrix with an obstacle in that cell
        foreach (var cell in matrix.Cells().Where(c => c.Value == '.'))
        {
            var newMatrix = matrix
                .Select(row => row.ToArray())
                .ToArray();
            
            newMatrix[cell.Y][cell.X] = '#';
            
            var patrol = new Patrol(newMatrix);
            var maxIterations = 1_000; // Empirical value for brute-forcing the solution
            
            // If the guard doesn't exit within 10,000 iterations, we assume it's stuck
            // in a loop, so this counts as a valid case
            while (!patrol.Exited && maxIterations-- > 0)
            {
                patrol.PerformIteration();
            }
            
            if (!patrol.Exited)
            {
                loops++;
            }
        }
        
        output.WriteLine(loops.ToString());
        
        Assert.Equal(1957, loops);
    }

    private enum Direction { Up, Down, Left, Right }

    private class Patrol
    {
        private readonly char[][] _matrix;
        private readonly HashSet<(int, int)> _visited = [];
        private Cell<char> _guardCell;
        private Direction _currentDirection = Direction.Up;

        public Patrol(char[][] matrix)
        {
            _matrix = matrix;
            
            // We assume the guard is always looking up at the start
            _guardCell = _matrix.Cells().First(c => c.Value == '^');
        }

        public int VisitedCount => _visited.Count;
        public bool Exited { get; private set; }

        public void PerformIteration()
        {
            var cellsEnumeration = _currentDirection switch
            {
                Direction.Up => _matrix.AboveCells(_guardCell),
                Direction.Down => _matrix.BelowCells(_guardCell),
                Direction.Left => _matrix.LeftCells(_guardCell),
                Direction.Right => _matrix.RightCells(_guardCell),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            // Take cells until we reach a #
            var walkableCells = cellsEnumeration
                .TakeWhile(c => c.Value != '#')
                .ToList();
            
            // Mark all cells as visited
            _visited.UnionWith(walkableCells.Select(c => (c.Y, c.X)));
            
            // Move the guard to the last cell (if any)
            if (walkableCells.Count > 0)
            {
                _guardCell = walkableCells.Last();
            }
            
            // Take the cell we're facing
            cellsEnumeration = _currentDirection switch
            {
                Direction.Up => _matrix.AboveCells(_guardCell),
                Direction.Down => _matrix.BelowCells(_guardCell),
                Direction.Left => _matrix.LeftCells(_guardCell),
                Direction.Right => _matrix.RightCells(_guardCell),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            // If we're not facing anything, we're exiting
            if (!cellsEnumeration.Any())
            {
                Exited = true;
                return;
            }
            
            // Otherwise, we must be facing a wall.
            // Turn 90 degrees to the right.
            _currentDirection = _currentDirection switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
