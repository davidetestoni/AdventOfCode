using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

internal class Day08 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day08.txt");

        var instructions = lines[0]
            .Select(l => l == 'L' ? Instruction.Left : Instruction.Right)
            .ToArray();

        var nodes = new Dictionary<string, Node>();

        foreach (var line in lines.Skip(2))
        {
            var match = Regex.Match(line, @"(\w+) = \((\w+), (\w+)\)");
            var name = match.Groups[1].Value;
            var left = match.Groups[2].Value;
            var right = match.Groups[3].Value;
            var node = new Node(name, left, right);

            nodes.Add(name, node);
        }

        // Part 1
        var steps = 0L;
        var currentNode = nodes["AAA"];

        while (true)
        {
            var instruction = instructions[steps % instructions.Length];
            steps++;

            currentNode = instruction == Instruction.Left
                ? nodes[currentNode.Left] 
                : nodes[currentNode.Right];

            if (currentNode.Name == "ZZZ")
            {
                break;
            }
        }

        Console.WriteLine($"Steps: {steps}");

        // Part 2
        var currentNodes = nodes.Values.Where(n => n.Name.EndsWith("A")).ToArray();
        var allSteps = currentNodes.Select(n =>
        {
            var steps = 0L;

            while (true)
            {
                var instruction = instructions[steps % instructions.Length];
                steps++;

                n = instruction == Instruction.Left
                    ? nodes[n.Left] 
                    : nodes[n.Right];

                if (n.Name.EndsWith("Z"))
                {
                    break;
                }
            }

            return steps;
        }).ToArray();

        // Find the least common multiple of all steps
        steps = Lcm(allSteps);

        Console.WriteLine($"Steps: {steps}");
    }

    private static long Lcm(long[] longs) =>
        longs.Aggregate((a, b) => a * b / Gcd(a, b));

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }

        return a;
    }

    record Node(string Name, string Left, string Right);

    enum Instruction
    {
        Left,
        Right
    }
}
