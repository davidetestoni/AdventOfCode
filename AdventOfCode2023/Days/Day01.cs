namespace AdventOfCode2023.Days;

public class Day01 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day01.txt");

        var sum = lines.Select(l => l.ToCharArray().Where(char.IsDigit))
            .Select(c =>
            {
                var enumerable = c.ToList();
                
                // Safeguard for part 2 input
                if (enumerable.Count == 0)
                {
                    return 0;
                }
                
                return (enumerable.First() - '0') * 10 + (enumerable.Last() - '0');
            })
            .Sum();

        // Part 1
        Console.WriteLine($"Sum: {sum}");
        
        // Part 2
        sum = 0;
        var words = new Dictionary<string, int>()
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9
        };
        
        foreach (var line in lines)
        {
            var numbers = new List<int>();
            
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                
                // If the character is a digit, add it to the list directly
                if (char.IsDigit(c))
                {
                    numbers.Add(c - '0');
                    continue;
                }

                // Otherwise, try to parse the word
                foreach (var (word, value) in words)
                {
                    var remainingLength = line.Length - i;
                    
                    // If the word is longer than the remaining length, skip it
                    if (remainingLength < word.Length)
                    {
                        continue;
                    }

                    if (word == line.Substring(i, word.Length))
                    {
                        numbers.Add(value);
                        
                        // We don't add the length of the word to i because
                        // otherwise we miss stuff like 'eightwo' (eight two)
                        
                        break;
                    }
                }
            }
            
            sum += numbers.First() * 10 + numbers.Last();
        }

        Console.WriteLine($"Sum: {sum}");
    }
}