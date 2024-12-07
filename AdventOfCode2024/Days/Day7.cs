using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day7(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day7.txt");

    [Fact]
    public void Part1()
    {
        var equations = _lines
            .Select(l => l.Split(":"))
            .Select(l => new CalibrationEquation
            {
                Result = long.Parse(l[0]),
                Numbers = l[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse).ToArray(),
            })
            .ToArray();

        Operator[] operators = [Operator.Add, Operator.Multiply];

        var result = equations
            .Where(e => operators
                .Variations(e.Numbers.Length - 1, withRepetition: true)
                .Any(p => (e with { Operators = p.ToArray() }).IsSatisfied()))
            .Sum(e => e.Result);
        
        output.WriteLine(result.ToString());
        
        Assert.Equal(6083020304036L, result);
    }

    [Fact]
    public void Part2()
    {
        var equations = _lines
            .Select(l => l.Split(":"))
            .Select(l => new CalibrationEquation
            {
                Result = long.Parse(l[0]),
                Numbers = l[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse).ToArray(),
            })
            .ToArray();
        
        Operator[] operators = [Operator.Add, Operator.Multiply, Operator.Concatenate];

        var result = equations
            .Where(e => operators
                .Variations(e.Numbers.Length - 1, withRepetition: true)
                .Any(p => (e with { Operators = p.ToArray() }).IsSatisfied()))
            .Sum(e => e.Result);
        
        output.WriteLine(result.ToString());
        
        Assert.Equal(59002246504791L, result);
    }
    
    private record CalibrationEquation
    {
        public long Result { get; init; }
        public long[] Numbers { get; init; } = [];
        public Operator[] Operators { get; init; } = [];

        public bool IsSatisfied()
        {
            // We assume there are at least 2 numbers and the operators
            // are one less than the numbers
            
            var result = Numbers[0];

            for (var i = 0; i < Numbers.Length - 1; i++)
            {
                result = Operators[i] switch
                {
                    Operator.Add => result + Numbers[i + 1],
                    Operator.Multiply => result * Numbers[i + 1],
                    Operator.Concatenate => Concatenate(result, Numbers[i + 1]),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            
            return result == Result;
        }
        
        private static long Concatenate(long a, long b)
        {
            var digits = (int)Math.Floor(Math.Log10(b) + 1);
            return a * (long)Math.Pow(10, digits) + b;
        }
    }

    private enum Operator
    {
        Add,
        Multiply,
        Concatenate
    }
}
