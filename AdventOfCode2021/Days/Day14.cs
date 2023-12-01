using System.Text;

namespace AdventOfCode2021.Days;

public class Day14 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day14.txt");
        var sequence = lines[0];
        var replacements = lines
            .Skip(2)
            .Select(line => line.Split(" -> "))
            .ToDictionary(
                split => (split[0][0], split[0][1]), split => split[1][0]);
        
        // Part 1
        Console.WriteLine($"10 rounds: {Polymerize(sequence, 10, replacements)}");
        
        // Part 2
        // Console.WriteLine($"40 rounds: {Polymerize(sequence, 40, replacements)}");
        var occurrences = new Dictionary<char, long>();

        for (var i = 0; i < sequence.Length; i++)
        {
            var c = sequence[i];
            
            if (!occurrences.ContainsKey(c))
            {
                occurrences[c] = 1;
            }
            else
            {
                occurrences[c]++;
            }

            if (i == sequence.Length - 1)
            {
                break;
            }
            
            // TODO: We need to use memoization here!
            Polymerize2(sequence[i], sequence[i + 1],
                40, replacements, occurrences);
        }

        var mostFrequent = occurrences.Max(kvp => kvp.Value);
        var leastFrequent = occurrences.Min(kvp => kvp.Value);

        Console.WriteLine($"40 rounds: {mostFrequent - leastFrequent}");
    }

    private static long Polymerize(string sequence, int rounds, 
        IReadOnlyDictionary<(char, char), char> replacements)
    {
        for (var i = 0; i < rounds; i++)
        {
            // Use a StringBuilder to reduce allocations
            var sb = new StringBuilder();
            
            for (var j = 0; j < sequence.Length; j++)
            {
                // First of all, append the current char
                sb.Append(sequence[j]);
                
                // If it's the end of the string, we're done
                if (j + 1 >= sequence.Length)
                {
                    continue;
                }
                
                // Check if the character and the following one
                // have a replacement
                var pair = (sequence[j], sequence[j + 1]);
                if (replacements.TryGetValue(pair, out var replacement))
                {
                    sb.Append(replacement);
                }
            }

            sequence = sb.ToString();
        }

        var orderedChars = sequence
            .ToCharArray()
            .GroupBy(c => c)
            .Select(g =>
                new {
                    Character = g.Key,
                    Count = g.Count()
                })
            .OrderByDescending(c => c.Count)
            .ToList();

        return orderedChars.First().Count - orderedChars.Last().Count;
    }
    
    // e.g. NNCB -> NCNBCHB
    // NN -> C
    // NC -> B
    // CB -> H
    // Divide by chunks like NN, NC, CB
    // For each chunk, check if it's in the map
    // If it is, produce new pairs
    // (N[C] [C]N), (N[B] [B]C), (C[H] [H]B)
    // Add 1 to the C, B and H count
    private static void Polymerize2(char c1, char c2, int round,
        IReadOnlyDictionary<(char, char), char> replacements,
        Dictionary<char, long> occurrences)
    {
        var mostFrequent = occurrences.Max(kvp => kvp.Value);
        var leastFrequent = occurrences.Min(kvp => kvp.Value);
        Console.WriteLine($"{mostFrequent - leastFrequent}");
        
        if (round == 0)
        {
            return;
        }
        
        if (replacements.TryGetValue((c1, c2), out var c))
        {
            if (!occurrences.ContainsKey(c))
            {
                occurrences[c] = 1;
            }
            else
            {
                occurrences[c]++;
            }
            
            Polymerize2(c1, c, round - 1,
                replacements, occurrences);
            
            Polymerize2(c, c2, round - 1,
                replacements, occurrences);
        }
    }
}