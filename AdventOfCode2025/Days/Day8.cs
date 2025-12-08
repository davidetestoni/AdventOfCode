using Xunit.Abstractions;
using Point = (int x, int y, int z);

namespace AdventOfCode2025.Days;

public class Day8(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day8.txt");

    private static double Distance(Point p, Point q)
        => Math.Sqrt(Math.Pow(p.x - q.x, 2) + Math.Pow(p.y - q.y, 2) + Math.Pow(p.z - q.z, 2));
    
    [Fact]
    public void Part1()
    {
        var points = _lines
            .Select(l => l.Split(','))
            .Select(s => new Point(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2])))
            .ToList();

        var pairs = points
            .SelectMany((p, i) => points.Skip(i + 1).Select(q => (p, q)))
            .OrderBy(pair => Distance(pair.p, pair.q))
            .ToList();

        var mappings = new Dictionary<Point, int>();
        var nextCircuit = 1;

        foreach (var _ in Enumerable.Range(0, 1_000))
        {
            if (pairs.Count == 0)
            {
                break;
            }
            
            var pair = pairs[0];
            pairs.RemoveAt(0);

            mappings.TryGetValue(pair.p, out var pCircuit);
            mappings.TryGetValue(pair.q, out var qCircuit);
            
            // If both are in a circuit
            if (pCircuit != 0 && qCircuit != 0)
            {
                if (pCircuit == qCircuit)
                {
                    continue;
                }

                // If they don't belong to the same circuit, connect the circuits by
                // replacing pCircuit with qCircuit everywhere
                foreach (var key in mappings.Keys.ToList().Where(key => mappings[key] == pCircuit))
                {
                    mappings[key] = qCircuit;
                }
            }
            // If any of the two is already in a circuit, assign to the same circuit
            else if (pCircuit != 0)
            {
                mappings.Add(pair.q, pCircuit);
            }
            else if (qCircuit != 0)
            {
                mappings.Add(pair.p, qCircuit);
            }
            // Otherwise, create a new circuit
            else
            {
                mappings.Add(pair.p ,nextCircuit);
                mappings.Add(pair.q, nextCircuit);
                nextCircuit++;   
            }
        }
        
        var total = mappings
            .GroupBy(c => c.Value)
            .Select(g => g.Count())
            .OrderByDescending(s => s)
            .Take(3)
            .Aggregate((x, y) => x * y);
        
        output.WriteLine(total.ToString());

        Assert.Equal(47040, total);
    }

    [Fact]
    public void Part2()
    {
        // This is a bit slow, takes about 5 seconds on my laptop, but I guess it's still acceptable...
        
        var points = _lines
            .Select(l => l.Split(','))
            .Select(s => new Point(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2])))
            .ToList();

        var pairs = points
            .SelectMany((p, i) => points.Skip(i + 1).Select(q => (p, q)))
            .OrderBy(pair => Distance(pair.p, pair.q))
            .ToList();

        var mappings = new Dictionary<Point, int>();
        var circuits = new Dictionary<int, int>(); // Circuit ID to their size
        var nextCircuit = 1;

        while (pairs.Count > 0)
        {
            var pair = pairs[0];
            pairs.RemoveAt(0);

            mappings.TryGetValue(pair.p, out var pCircuit);
            mappings.TryGetValue(pair.q, out var qCircuit);
            
            // If both are in a circuit
            if (pCircuit != 0 && qCircuit != 0)
            {
                if (pCircuit == qCircuit)
                {
                    continue;
                }

                // If they don't belong to the same circuit, connect the circuits by
                // replacing pCircuit with qCircuit everywhere
                foreach (var key in mappings.Keys.ToList().Where(key => mappings[key] == pCircuit))
                {
                    mappings[key] = qCircuit;
                    circuits[pCircuit]--;
                    circuits[qCircuit]++;
                }
            }
            // If any of the two is already in a circuit, assign to the same circuit
            else if (pCircuit != 0)
            {
                mappings.Add(pair.q, pCircuit);
                circuits[pCircuit]++;
            }
            else if (qCircuit != 0)
            {
                mappings.Add(pair.p, qCircuit);
                circuits[qCircuit]++;
            }
            // Otherwise, create a new circuit
            else
            {
                mappings.Add(pair.p ,nextCircuit);
                mappings.Add(pair.q, nextCircuit);
                circuits[nextCircuit] = 2;
                nextCircuit++;
            }

            // Check if we made a complete circuit
            if (circuits.Any(c => c.Value == points.Count))
            {
                var solution = (long)pair.p.x * pair.q.x;
                output.WriteLine(solution.ToString());
                output.WriteLine(pair.ToString());
                Assert.Equal(4884971896, solution);
                return;
            }
        }

        Assert.False(true);
    }
}