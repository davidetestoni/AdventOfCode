using Xunit.Abstractions;
using Rotation = (char direction, int distance);
using RotationResult = (int knobPosition, int zeroes); 

namespace AdventOfCode2025.Days;

public class Day1(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day1.txt");

    [Fact]
    public void Part1()
    {
        var password = _lines
            .Select<string, Rotation>(line => (line[0], int.Parse(line[1..])))
            .Aggregate<Rotation, RotationResult>((50, 0), (acc, rotation) =>
            {
                acc.knobPosition += rotation.direction == 'L' ? -rotation.distance : rotation.distance;
                acc.knobPosition %= 100;

                if (acc.knobPosition == 0)
                {
                    acc.zeroes++;
                }

                return acc;
            }).zeroes;

        output.WriteLine(password.ToString());

        Assert.Equal(1018, password);
    }
    
    [Fact]
    public void Part2Slow()
    {
        var password = _lines
            .Select<string, Rotation>(line => (line[0], int.Parse(line[1..])))
            .Aggregate<Rotation, RotationResult>((50, 0), (acc, rotation) =>
            {
                while (rotation.distance > 0)
                {
                    acc.knobPosition += rotation.direction == 'L' ? -1 : 1;

                    if (acc.knobPosition < 0)
                    {
                        acc.knobPosition += 100;
                    }
                    
                    acc.knobPosition %= 100;

                    if (acc.knobPosition == 0)
                    {
                        acc.zeroes++;
                    }

                    rotation.distance--;
                }
                
                return acc;
            }).zeroes;

        output.WriteLine(password.ToString());

        Assert.Equal(5815, password);
    }
    
    [Fact]
    public void Part2Fast()
    {
        var password = _lines
            .Select<string, Rotation>(line => (line[0], int.Parse(line[1..])))
            .Aggregate<Rotation, RotationResult>((50, 0), (acc, rotation) =>
            {
                var startingPosition = acc.knobPosition;
                if (rotation.direction == 'R')
                {
                    acc.knobPosition += rotation.distance;
                    acc.zeroes += acc.knobPosition / 100;
                    acc.knobPosition %= 100;
                }
                else
                {
                    acc.knobPosition -= rotation.distance;
            
                    if (acc.knobPosition < 0)
                    {
                        // Sign change
                        if (startingPosition > 0)
                        {
                            acc.zeroes++;
                        }
                        
                        acc.zeroes += (Math.Abs(acc.knobPosition) - 1) / 100;
                        acc.knobPosition %= 100;
                        acc.knobPosition += 100;
                        acc.knobPosition %= 100;
                    }

                    if (acc.knobPosition == 0)
                    {
                        acc.zeroes++;
                    }
                }

                return acc;
            }).zeroes;

        output.WriteLine(password.ToString());

        Assert.Equal(5815, password);
    }

    // Helper tests with all possible cases
    [Theory]
    [InlineData(50, 'L', 100, 50, 1)]
    [InlineData(50, 'L', 200, 50, 2)]
    [InlineData(50, 'R', 100, 50, 1)]
    [InlineData(50, 'R', 200, 50, 2)]
    [InlineData(50, 'R', 50, 0, 1)]
    [InlineData(50, 'L', 50, 0, 1)]
    [InlineData(0, 'L', 100, 0, 1)]
    [InlineData(0, 'L', 200, 0, 2)]
    [InlineData(0, 'R', 100, 0, 1)]
    [InlineData(0, 'R', 200, 0, 2)]
    [InlineData(0, 'R', 75, 75, 0)]
    [InlineData(75, 'L', 85, 90, 1)]
    [InlineData(90, 'L', 210, 80, 2)]
    [InlineData(80, 'R', 210, 90, 2)]
    [InlineData(90, 'R', 105, 95, 1)]
    [InlineData(50, 'R', 150, 0, 2)]
    [InlineData(50, 'L', 150, 0, 2)]
    [InlineData(50, 'L', 120, 30, 1)]
    [InlineData(50, 'L', 160, 90, 2)]
    [InlineData(50, 'R', 160, 10, 2)]
    public void Test(int start, char dir, int distance, int end, int zeroes)
    {
        var myEnd = start;
        var myZeroes = 0;

        if (dir == 'R')
        {
            myEnd += distance;
            myZeroes += myEnd / 100;
            myEnd %= 100;
        }
        else
        {
            myEnd -= distance;
            
            if (myEnd < 0)
            {
                // Sign change
                if (start > 0)
                {
                    myZeroes++;
                }
                
                myZeroes += (Math.Abs(myEnd) - 1) / 100;
                myEnd %= 100;
                myEnd += 100;
                myEnd %= 100;
            }

            if (myEnd == 0)
            {
                myZeroes++;
            }
        }
       
        Assert.Equal(end, myEnd);
        Assert.Equal(zeroes, myZeroes);
    }
}
