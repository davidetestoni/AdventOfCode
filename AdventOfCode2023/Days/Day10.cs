using AdventOfCode.Utils;

namespace AdventOfCode2023.Days;

internal class Day10 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day10.txt");
        var matrix = lines
            .Select(l => l.ToCharArray())
            .ToArray();

        // Replace characters so it's easier to see
        for (var i = 0; i < matrix.Length; i++)
        {
            for (var j = 0; j < matrix[i].Length; j++)
            {
                matrix[i][j] = matrix[i][j] switch
                {
                    '|' => '│',
                    '-' => '─',
                    'L' => '└',
                    'J' => '┘',
                    '7' => '┐',
                    'F' => '┌',
                    '.' => 'o',
                    'S' => 'x',
                    _ => matrix[i][j]
                };
            }
        }

        var distances = new Dictionary<Cell<char>, long>();

        // Part 1
        var start = matrix.Cells().First(c => c.Value == 'x');
        distances.Add(start, 0);

        var loopCells = new List<Cell<char>>() { start };

        while (loopCells.Count > 0)
        {
            var nextCells = new List<Cell<char>>();

            foreach (var cell in loopCells)
            {
                var connectedNeighbors = matrix.Neighbors(cell, diagonal: false)
                    .Where(n => !distances.ContainsKey(n) && IsConnected(cell, n))
                    .ToList();

                connectedNeighbors.ForEach(n => distances.Add(n, distances[cell] + 1));

                nextCells.AddRange(connectedNeighbors);
            }

            loopCells = nextCells;
        }

        var maxDistance = distances.Values.Max();

        Console.WriteLine($"Max distance: {maxDistance}");

        // Part 2
        var enclosed = matrix.Cells()
            .Where(c => IsEnclosed(c, matrix, distances))
            .Count();

        Console.WriteLine($"Enclosed: {enclosed}");

        for (var i = 0; i < matrix.Length; i++)
        {
            for (var j = 0; j < matrix[i].Length; j++)
            {
                var c = matrix[i][j];

                Console.ForegroundColor = distances.ContainsKey(new Cell<char>(i, j, matrix[i][j]))
                    ? ConsoleColor.Green
                    : ConsoleColor.White;

                // Enclosed are blue
                if (IsEnclosed(new Cell<char>(i, j, matrix[i][j]), matrix, distances))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                Console.Write(c);
            }

            Console.WriteLine();
        }
    }

    private static bool IsEnclosed(Cell<char> cell, char[][] matrix,
        Dictionary<Cell<char>, long> distances)
    {
        // TODO: Implement this
        return false;
    }

    private static bool IsConnected(Cell<char> a, Cell<char> b)
    {
        var aConnections = GetConnections(a);
        var bConnections = GetConnections(b);

        // If a is above b, check if a has south and b has north.
        if (a.Y == b.Y - 1 && a.X == b.X)
        {
            return aConnections.HasFlag(Connections.South) &&
                   bConnections.HasFlag(Connections.North);
        }

        // If a is below b, check if a has north and b has south.
        if (a.Y == b.Y + 1 && a.X == b.X)
        {
            return aConnections.HasFlag(Connections.North) &&
                   bConnections.HasFlag(Connections.South);
        }

        // If a is left of b, check if a has east and b has west.
        if (a.Y == b.Y && a.X == b.X - 1)
        {
            return aConnections.HasFlag(Connections.East) &&
                   bConnections.HasFlag(Connections.West);
        }

        // If a is right of b, check if a has west and b has east.
        if (a.Y == b.Y && a.X == b.X + 1)
        {
            return aConnections.HasFlag(Connections.West) &&
                   bConnections.HasFlag(Connections.East);
        }

        return false;
    }

    private static Connections GetConnections(char c)
    {
        return c switch
        {
            '│' => Connections.North | Connections.South,
            '─' => Connections.East | Connections.West,
            '└' => Connections.North | Connections.East,
            '┘' => Connections.North | Connections.West,
            '┐' => Connections.South | Connections.West,
            '┌' => Connections.South | Connections.East,
            'o' => Connections.None,
            'x' => Connections.North | Connections.East | Connections.South | Connections.West,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }

    [Flags]
    enum Connections
    {
        None = 0,
        North = 1 << 0,
        East = 1 << 1,
        South = 1 << 2,
        West = 1 << 3
    }
}
