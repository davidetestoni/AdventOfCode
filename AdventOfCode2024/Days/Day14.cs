using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public partial class Day14(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day14.txt");
    private const int Rows = 103;
    private const int Columns = 101;

    [Fact]
    public void Part1()
    {
        var robots = _lines
            .Select(Robot.Parse)
            .ToList();

        // Let 100 seconds pass
        for (var i = 0; i < 100; i++)
        {
            foreach (var robot in robots)
            {
                robot.TakeStep();
            }
        }
        
        var safetyFactor = robots
            .GroupBy(r => r.GetQuadrant())
            .Where(g => g.Key != 0)
            .Aggregate(1L, (acc, group) => acc * group.Count());
        
        output.WriteLine(safetyFactor.ToString());
        
        Assert.Equal(224357412, safetyFactor);
    }

    [Fact]
    public void Part2()
    {
        var robots = _lines
            .Select(Robot.Parse)
            .ToList();
        
        var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "Day14");
        Directory.Delete(baseFolder, true);
        Directory.CreateDirectory(baseFolder);
        
        // Let 10000 seconds pass
        for (var i = 0; i < 10000; i++)
        {
            foreach (var robot in robots)
            {
                robot.TakeStep();
            }
            
            // If there are many robots on the same horizontal line,
            // this might be an interesting configuration for the Christmas Tree
            
            if (robots
                .GroupBy(r => r.Position.Y)
                .Any(g => g.Count() > 30))
            {
                PrintLayout(robots, i + 1, baseFolder);
            }
        }
        
        output.WriteLine($"Check the images in {baseFolder}");
        
        Assert.True(true);
    }

    private static void PrintLayout(IEnumerable<Robot> robots, int second,
        string baseFolder)
    {
        // Print the matrix of the robots' positions as colored pixels
        // in a bitmap image, then save it with the name "second.png"
        var layout = new char[Rows, Columns];
        
        foreach (var robot in robots)
        {
            layout[robot.Position.Y, robot.Position.X] = '#';
        }
        
        // Use ImageSharp
        using var image = new Image<Rgba32>(Columns, Rows);
        
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Columns; x++)
            {
                var color = layout[y, x] switch
                {
                    // Let's give it a Christmas-y vibe
                    '#' => (x + y) % 2 == 0 
                        ? Rgba32.ParseHex("#0F0")
                        : Rgba32.ParseHex("#F00"),
                    _ => Rgba32.ParseHex("#000")
                };
                
                image[x, y] = color;
            }
        }
        
        image.Save(Path.Combine(baseFolder, $"{second}.png"));
    }

    private partial class Robot
    {
        public Coordinates Position { get; private set; }
        private Coordinates Velocity { get; }

        private Robot(Coordinates position, Coordinates velocity)
        {
            Position = position;
            Velocity = velocity;
        }
        
        public static Robot Parse(string line)
        {
            var match = RobotRegex().Match(line);
            return new Robot(
                new Coordinates(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value)),
                new Coordinates(
                    int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value)));
        }

        public void TakeStep()
        {
            var x = (Position.X + Velocity.X) % Columns;

            if (x < 0)
            {
                x += Columns;
            }
            
            var y = (Position.Y + Velocity.Y) % Rows;
            
            if (y < 0)
            {
                y += Rows;
            }
            
            Position = new Coordinates(x, y);
        }

        public int GetQuadrant()
            => Position switch
            {
                { X: < (Columns - 1) / 2, Y: < (Rows - 1) / 2 } => 1,
                { X: > (Columns - 1) / 2, Y: < (Rows - 1) / 2 } => 2,
                { X: > (Columns - 1) / 2, Y: > (Rows - 1) / 2 } => 3,
                { X: < (Columns - 1) / 2, Y: > (Rows - 1) / 2 } => 4,
                _ => 0
            };

        [GeneratedRegex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)")]
        private static partial Regex RobotRegex();
    }
    private record Coordinates(int X, int Y);
}
