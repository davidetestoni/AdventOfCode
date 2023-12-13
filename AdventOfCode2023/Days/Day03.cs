using AdventOfCode.Utils;

namespace AdventOfCode2023.Days;

internal class Day03 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day03.txt");

        var m = lines.Select(x => x.ToCharArray()).ToArray();

        // Part 1
        var numbers = new List<long>();

        foreach (var cell in m.Cells())
        {
            // If symbol, check neighbors and reconstruct numbers
            if (IsSymbol(cell.Value))
            {
                foreach (var neighbor in m.Neighbors(cell))
                {
                    if (char.IsDigit(neighbor.Value))
                    {
                        numbers.Add(ParseNumber(neighbor.Y, neighbor.X, m));
                    }
                }
            }
        }

        Console.WriteLine($"Sum: {numbers.Sum()}");

        // Part 2
        m = lines.Select(x => x.ToCharArray()).ToArray();
        var gearRatios = new List<long>();

        foreach (var cell in m.Cells().Where(c => IsGear(c.Value)))
        {
            numbers = new List<long>();

            foreach (var neighbor in m.Neighbors(cell))
            {
                if (char.IsDigit(neighbor.Value))
                {
                    numbers.Add(ParseNumber(neighbor.Y, neighbor.X, m));
                }
            }

            if (numbers.Count == 2)
            {
                gearRatios.Add(numbers[0] * numbers[1]);
            }
        }

        Console.WriteLine($"Total Ratio: {gearRatios.Sum()}");
    }

    private static long ParseNumber(int y, int x, char[][] m)
    {
        // Here x is somewhere inside the number, it might be a middle digit as well
        var number = "";
        var currX = x;

        // Rewind the x until we find the first digit of the number
        while (currX >= 0 && char.IsDigit(m[y][currX]))
        {
            currX--;
        }

        // Increase by 1 to get the correct position
        currX++;

        // Now go forward and parse the number from here
        while (currX <= m[y].Length - 1 && char.IsDigit(m[y][currX]))
        {
            number += m[y][currX];

            // Replace the digit with a dot so we don't count it again
            m[y][currX] = '.';

            currX++;
        }

        return long.Parse(number);
    }

    private static bool IsSymbol(char c) => c != '.' && !char.IsDigit(c);

    private static bool IsGear(char c) => c == '*';
}
