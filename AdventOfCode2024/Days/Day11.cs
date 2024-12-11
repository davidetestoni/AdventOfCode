using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day11(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day11.txt");

    [Fact]
    public void Part1()
    {
        var stones = _lines[0]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
        
        var cache = new Dictionary<(long, int), long>();
        
        // Perform 25 iterations
        var count = stones
            .Select(s => CountStones(s, 25, cache))
            .Sum();
        
        output.WriteLine(count.ToString());
        
        Assert.Equal(190865, count);
    }
    
    [Fact]
    public void Part2()
    {
        var stones = _lines[0]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
        
        var cache = new Dictionary<(long, int), long>();
        
        // Perform 75 iterations
        var count = stones
            .Select(s => CountStones(s, 75, cache))
            .Sum();
        
        output.WriteLine(count.ToString());
        
        Assert.Equal(225404711855335L, count);
    }

    private static long CountStones(
        long value, int iteration, Dictionary<(long, int), long> cache)
    {
        // If we already have it in the cache, return it
        if (cache.TryGetValue((value, iteration), out var cachedValue))
        {
            return cachedValue;
        }
        
        // If it's the last iteration, return 1
        if (iteration == 0)
        {
            return 1;
        }
        
        // If the stone is engraved with the number 0,
        // it is replaced by a stone engraved with the number 1.
        if (value == 0)
        {
            var result = CountStones(1, iteration - 1, cache);
            cache[(value, iteration)] = result;
            return result;
        }
        
        // If the stone is engraved with a number that has an even number of digits,
        // it is replaced by two stones. The left half of the digits are engraved
        // on the new left stone, and the right half of the digits are engraved
        // on the new right stone. (The new numbers don't keep extra leading zeroes:
        // 1000 would become stones 10 and 0.)
        var nDigits = Math.Floor(Math.Log10(value) + 1);
        
        if (nDigits % 2 == 0)
        {
            var firstStone = value / (long)Math.Pow(10, nDigits / 2);
            var secondStone = value % (long)Math.Pow(10, nDigits / 2);
            
            var result = CountStones(firstStone, iteration - 1, cache) +
                         CountStones(secondStone, iteration - 1, cache);
            
            cache[(value, iteration)] = result;
            return result;
        }
        
        // If none of the other rules apply, the stone is replaced by a new stone;
        // the old stone's number multiplied by 2024 is engraved on the new stone.
        cache[(value, iteration)] = CountStones(value * 2024, iteration - 1, cache);
        return cache[(value, iteration)];
    }
}
