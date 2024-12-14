using System.Numerics;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public partial class Day13(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day13.txt");

    [Fact]
    public void Part1()
    {
        var games = _lines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Chunk(3)
            .Select(c => Game.Parse(c.ToArray()))
            .ToList();

        var tokens = games
            .Select(g => g.Solve())
            .Where(s => s.HasValue)
            .Sum(s => GetSolutionCost(s!.Value));
        
        output.WriteLine(tokens.ToString());
        
        Assert.Equal(26299, tokens);
    }

    [Fact]
    public void Part2()
    {
        var games = _lines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Chunk(3)
            .Select(c =>
            {
                var game = Game.Parse(c.ToArray());
                return game with
                {
                    Prize = new Coordinates(
                        game.Prize.X + 10000000000000,
                        game.Prize.Y + 10000000000000)
                };
            })
            .ToList();

        var tokens = games
            .Select(g => g.Solve())
            .Where(s => s.HasValue)
            .Sum(s => GetSolutionCost(s!.Value));
        
        output.WriteLine(tokens.ToString());
        
        Assert.Equal(107824497933339, tokens);
    }
    
    private static long GetSolutionCost((long aCount, long bCount) solution)
        => solution.aCount * 3 + solution.bCount;

    private partial record Game(Coordinates A, Coordinates B, Coordinates Prize)
    {
        public static Game Parse(string[] lines)
        {
            var aMatch = ButtonRegex().Match(lines[0]);
            var bMatch = ButtonRegex().Match(lines[1]);
            var prizeMatch = PrizeRegex().Match(lines[2]);
            
            return new Game(
                new Coordinates(
                    long.Parse(aMatch.Groups[1].Value),
                    long.Parse(aMatch.Groups[2].Value)),
                new Coordinates(
                    long.Parse(bMatch.Groups[1].Value),
                    long.Parse(bMatch.Groups[2].Value)),
                new Coordinates(
                    long.Parse(prizeMatch.Groups[1].Value),
                    long.Parse(prizeMatch.Groups[2].Value)));
        }

        public (long aCount, long bCount)? Solve()
        {
            // The system of linear equations is:
            // X * A.X + Y * A.Y = Prize.X
            // X * B.X + Y * B.Y = Prize.Y
            
            // Create the coefficient matrix
            var matrix = Matrix<double>.Build.DenseOfArray(new double[,] 
            {
                {A.X, B.X},
                {A.Y, B.Y}
            });

            // Create the constants vector
            var vector = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense([Prize.X, Prize.Y]);

            try
            {
                // Solve the system Ax = B
                var solutionVector = matrix.Solve(vector);
                
                // If the solution is not an integer, it's not valid
                // (use the epsilon to account for floating-point errors)
                if (solutionVector.Any(v => Math.Abs(v - Math.Round(v)) > 1e-2))
                {
                    return null;
                }
                
                // Round the solution to the nearest integer
                return (
                    aCount: (long)Math.Round(solutionVector[0]),
                    bCount: (long)Math.Round(solutionVector[1])
                );
            }
            catch (InvalidOperationException)
            {
                // No solution
                return null;
            }
        }

        [GeneratedRegex(@"Button .: X\+(\d+), Y\+(\d+)")]
        private static partial Regex ButtonRegex();
        
        [GeneratedRegex(@"Prize: X=(\d+), Y=(\d+)")]
        private static partial Regex PrizeRegex();
    }

    private record Coordinates(long X, long Y);
}
