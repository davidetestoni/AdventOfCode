namespace AdventOfCode.Utils;

public static class MatrixExtensions
{
    /// <summary>
    /// Yields all cells of the matrix.
    /// </summary>
    public static IEnumerable<Cell<T>> Cells<T>(this T[][] m)
    {
        for (int y = 0; y < m.Length; y++)
        {
            for (int x = 0; x < m[y].Length; x++)
            {
                yield return new Cell<T>(y, x, m[y][x]);
            }
        }
    }

    /// <summary>
    /// Yields all cells neighboring the given coordinates.
    /// </summary>
    public static IEnumerable<Cell<T>> Neighbors<T>(this T[][] m, int y, int x)
    {
        // Top
        if (y > 0)
        {
            yield return new Cell<T>(y - 1, x, m[y - 1][x]);
        }

        // Top-right
        if (y > 0 && x < m[y].Length - 1)
        {
            yield return new Cell<T>(y - 1, x + 1, m[y - 1][x + 1]);
        }

        // Right
        if (x < m[y].Length - 1)
        {
            yield return new Cell<T>(y, x + 1, m[y][x + 1]);
        }

        // Bottom-right
        if (y < m.Length - 1 && x < m[y].Length - 1)
        {
            yield return new Cell<T>(y + 1, x + 1, m[y + 1][x + 1]);
        }

        // Bottom
        if (y < m.Length - 1)
        {
            yield return new Cell<T>(y + 1, x, m[y + 1][x]);
        }

        // Bottom-left
        if (y < m.Length - 1 && x > 0)
        {
            yield return new Cell<T>(y + 1, x - 1, m[y + 1][x - 1]);
        }

        // Left
        if (x > 0)
        {
            yield return new Cell<T>(y, x - 1, m[y][x - 1]);
        }

        // Top-left
        if (y > 0 && x > 0)
        {
            yield return new Cell<T>(y - 1, x - 1, m[y - 1][x - 1]);
        }
    }
}

public struct Cell<T>
{
    public int Y { get; set; }

    public int X { get; set; }

    public T Value { get; set; }

    public Cell(int y, int x, T value)
    {
        Y = y;
        X = x;
        Value = value;
    }
}
