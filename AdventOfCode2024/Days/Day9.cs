using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day9(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day9.txt");

    [Fact]
    public void Part1()
    {
        var disk = _lines[0]
            .Select(c => c - '0')
            .Select((b, i) => i % 2 == 0
                ? Enumerable.Repeat(i / 2, b)
                : Enumerable.Repeat(-1, b)) // -1 is free space
            .SelectMany(x => x)
            .ToList();

        var left = 0;
        var right = disk.Count - 1;

        while (left < right)
        {
            // If we have free space to the right, move the cursor up
            if (disk[right] == -1)
            {
                right--;
                continue;
            }
            
            // Otherwise, if the right cursor is pointing to a block, check
            // the left cursor
            
            // If it's pointing to a block, move the cursor up
            if (disk[left] != -1)
            {
                left++;
                continue;
            }
            
            // Otherwise, swap the block with the free space
            disk[left] = disk[right];
            disk[right] = -1;
            
            left++;
            right--;
        }

        var checksum = disk
            .TakeWhile(b => b != -1)
            .Select((b, i) => (long)b * i)
            .Sum();
        
        output.WriteLine(checksum.ToString());
        
        Assert.Equal(6283404590840L, checksum);
    }

    [Fact]
    public void Part2()
    {
        var disk = _lines[0]
            .Select(c => c - '0')
            .Select((b, i) => i % 2 == 0
                ? Enumerable.Repeat(i / 2, b)
                : Enumerable.Repeat(-1, b)) // -1 is free space
            .SelectMany(x => x)
            .ToList();
        
        var right = disk.Count - 1;
        
        while (right >= 0)
        {
            // If we have free space to the right, move the cursor up
            if (disk[right] == -1)
            {
                right--;
                continue;
            }
            
            // Otherwise, check how long is the file (number of blocks with the
            // same ID moving to the left)
            var left = right;
            while (left >= 0 && disk[left] == disk[right])
            {
                left--;
            }
            var fileLength = right - left;
            
            // Search for the first free space to the left that can fit the file
            var freeSpaceIndex = disk.IndexOfRepeating(-1, fileLength);
            
            // If there is free space to the left of the file
            // that can fit the file, swap the file with the free space
            if (freeSpaceIndex != -1 && freeSpaceIndex + fileLength <= right)
            {
                for (var i = 0; i < fileLength; i++)
                {
                    disk[freeSpaceIndex + i] = disk[right];
                }

                for (var i = 0; i < fileLength; i++)
                {
                    disk[right - i] = -1;
                }
            }
            
            right = left;
        }

        var checksum = disk
            .Select((b, i) => b == -1 ? 0 : (long)b * i)
            .Sum();
        
        output.WriteLine(checksum.ToString());
        
        Assert.Equal(6304576012713L, checksum);
    }
}
