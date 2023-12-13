namespace AdventOfCode2023.Days;

internal class Day09 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day09.txt");

        var sequences = lines
            .Select(l => l.Split(' ').Select(long.Parse).ToArray())
            .ToArray();

        // Part 1
        var sum = sequences.Select(ComputeNextValue).Sum();

        Console.WriteLine($"Sum: {sum}");

        // Part 2
        sum = sequences.Select(ComputePreviousValue).Sum();

        Console.WriteLine($"Sum: {sum}");
    }

    private static long ComputeNextValue(long[] sequence)
    {
        if (sequence.All(i => i == 0))
        {
            return 0;
        }

        var nextSequence = sequence.Skip(1).Zip(
            sequence.Take(sequence.Length - 1), (a, b) => a - b)
            .ToArray();

        return sequence.Last() + ComputeNextValue(nextSequence);
    }

    private static long ComputePreviousValue(long[] sequence)
    {
        if (sequence.All(i => i == 0))
        {
            return 0;
        }

        var nextSequence = sequence.Skip(1).Zip(
            sequence.Take(sequence.Length - 1), (a, b) => a - b)
            .ToArray();

        return sequence.First() - ComputePreviousValue(nextSequence);
    }
}
