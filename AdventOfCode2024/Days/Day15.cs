using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day15(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day15.txt");

    [Fact]
    public void Part1()
    {
        // # = Obstacle
        // . = Empty cell
        // @ = Robot
        // O = Box
        var matrix = _lines
            .TakeWhile(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.ToCharArray())
            .ToArray();

        var movements = _lines
            .SkipWhile(l => !string.IsNullOrWhiteSpace(l))
            .Skip(1)
            .Select(l => l.ToCharArray())
            .SelectMany(c => c)
            .Select(c => c switch
            {
                '^' => Direction.Up,
                'v' => Direction.Down,
                '<' => Direction.Left,
                '>' => Direction.Right,
                _ => throw new InvalidOperationException()
            });
        
        foreach (var movement in movements)
        {
            ExecuteMovement(matrix, movement);
        }
        
        var gpsSum = matrix.Cells()
            .Where(c => c.Value == 'O')
            .Sum(c => c.Y * 100 + c.X);
        
        output.WriteLine(gpsSum.ToString());
        
        Assert.Equal(1505963, gpsSum);
    }

    private static void ExecuteMovement(char[][] matrix, Direction movement)
    {
        var robot = matrix.Cells().First(c => c.Value == '@');
        
        // Get the cell in front of the robot in the direction it's going to move
        // The robot is never at the edge of the matrix, since it's surrounded
        // by obstacles, so we don't need to check for out-of-bounds
        var nextCell = movement switch
        {
            Direction.Up => matrix.AboveCells(robot).First(),
            Direction.Down => matrix.BelowCells(robot).First(),
            Direction.Left => matrix.LeftCells(robot).First(),
            Direction.Right => matrix.RightCells(robot).First(),
            _ => throw new InvalidOperationException()
        };
        
        // If the next cell has an obstacle, the robot can't move
        if (nextCell.Value == '#')
        {
            return;
        }
        
        // If the next cell has a free space, move the robot
        if (nextCell.Value == '.')
        {
            matrix[robot.Y][robot.X] = '.';
            matrix[nextCell.Y][nextCell.X] = '@';
            return;
        }
        
        // Otherwise, get all the cells until the next obstacle
        var nextCells = (movement switch
        {
            Direction.Up => matrix.AboveCells(robot),
            Direction.Down => matrix.BelowCells(robot),
            Direction.Left => matrix.LeftCells(robot),
            Direction.Right => matrix.RightCells(robot),
            _ => throw new InvalidOperationException()
        }).ToList();
        
        // Check what's after a series of consecutive boxes
        var cellAfterBoxes = nextCells
            .SkipWhile(c => c.Value == 'O')
            .First();
        
        // If it's an obstacle, we can't move the boxes
        if (cellAfterBoxes.Value == '#')
        {
            return;
        }
        
        var firstBox = nextCells.First();
        
        // Otherwise, move all those boxes one cell forward, and then move the robot
        matrix[robot.Y][robot.X] = '.';
        matrix[firstBox.Y][firstBox.X] = '@';
        matrix[cellAfterBoxes.Y][cellAfterBoxes.X] = 'O';
    }
    
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
