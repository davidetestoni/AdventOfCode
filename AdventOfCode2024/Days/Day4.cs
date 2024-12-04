using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day4(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day4.txt");

    [Fact]
    public void Part1()
    {
        var matrix = _lines.Select(x => x.ToCharArray()).ToArray();
        var occurrences = 0;
        char[] sequence = ['X', 'M', 'A', 'S'];
        
        foreach (var cell in matrix.Cells())
        {
            Cell<char>[] cellSequence = [cell];
            
            // Check to the right
            var right = cellSequence
                .Concat(matrix.RightCells(cell).Take(3))
                .Select(c => c.Value)
                .ToArray();
            
            if (right.SequenceEqual(sequence) || right.SequenceEqual(sequence.Reverse()))
            {
                occurrences++;
            }
            
            // Check down
            var down = cellSequence
                .Concat(matrix.BelowCells(cell).Take(3))
                .Select(c => c.Value)
                .ToArray();

            if (down.SequenceEqual(sequence) || down.SequenceEqual(sequence.Reverse()))
            {
                occurrences++;
            }
            
            // Check down-right
            var downRight = cellSequence
                .Concat(matrix.BottomRightCells(cell).Take(3))
                .Select(c => c.Value)
                .ToArray();

            if (downRight.SequenceEqual(sequence) || downRight.SequenceEqual(sequence.Reverse()))
            {
                occurrences++;
            }
            
            // Check down-left
            var downLeft = cellSequence
                .Concat(matrix.BottomLeftCells(cell).Take(3))
                .Select(c => c.Value)
                .ToArray();

            if (downLeft.SequenceEqual(sequence) || downLeft.SequenceEqual(sequence.Reverse()))
            {
                occurrences++;
            }
        }
        
        output.WriteLine(occurrences.ToString());
        
        Assert.Equal(2414, occurrences);
    }

    [Fact]
    public void Part2()
    {
        var matrix = _lines.Select(x => x.ToCharArray()).ToArray();
        var occurrences = 0;
        char[] sequence = ['M', 'A', 'S'];

        foreach (var cell in matrix.Cells())
        {
            // We need 'A' to be in the middle
            if (cell.Value is not 'A')
            {
                continue;
            }
            
            // Check the top-left, center, bottom-right sequence
            var diagonal1 = matrix.TopLeftCells(cell).Take(1)
                .Concat([cell])
                .Concat(matrix.BottomRightCells(cell).Take(1))
                .Select(c => c.Value)
                .ToArray();
            
            var diagonal1Matches = diagonal1.SequenceEqual(sequence) || diagonal1.SequenceEqual(sequence.Reverse());
            
            // Check the top-right, center, bottom-left sequence
            var diagonal2 = matrix.TopRightCells(cell).Take(1)
                .Concat([cell])
                .Concat(matrix.BottomLeftCells(cell).Take(1))
                .Select(c => c.Value)
                .ToArray();

            var diagonal2Matches = diagonal2.SequenceEqual(sequence) || diagonal2.SequenceEqual(sequence.Reverse());
            
            if (diagonal1Matches && diagonal2Matches)
            {
                occurrences++;
            }
        }
        
        output.WriteLine(occurrences.ToString());
        
        Assert.Equal(1871, occurrences);
    }
}
