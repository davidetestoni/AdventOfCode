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
    /// Yields all cells above the given Y with the same X.
    /// </summary>
    public static IEnumerable<Cell<T>> AboveCells<T>(this T[][] m, Cell<T> cell)
    {
        for (int y = cell.Y - 1; y >= 0; y--)
        {
            yield return new Cell<T>(y, cell.X, m[y][cell.X]);
        }
    }

    /// <summary>
    /// Yields all cells below the given Y with the same X.
    /// </summary>
    public static IEnumerable<Cell<T>> BelowCells<T>(this T[][] m, Cell<T> cell)
    {
        for (int y = cell.Y + 1; y < m.Length; y++)
        {
            yield return new Cell<T>(y, cell.X, m[y][cell.X]);
        }
    }

    /// <summary>
    /// Yields all cells left of the given X with the same Y.
    /// </summary>
    public static IEnumerable<Cell<T>> LeftCells<T>(this T[][] m, Cell<T> cell)
    {
        for (int x = cell.X - 1; x >= 0; x--)
        {
            yield return new Cell<T>(cell.Y, x, m[cell.Y][x]);
        }
    }

    /// <summary>
    /// Yields all cells right of the given X with the same Y.
    /// </summary>
    public static IEnumerable<Cell<T>> RightCells<T>(this T[][] m, Cell<T> cell)
    {
        for (int x = cell.X + 1; x < m[cell.Y].Length; x++)
        {
            yield return new Cell<T>(cell.Y, x, m[cell.Y][x]);
        }
    }

    /// <summary>
    /// Yields all cells neighboring the given coordinates.
    /// </summary>
    public static IEnumerable<Cell<T>> Neighbors<T>(this T[][] m, Cell<T> cell,
        bool diagonal = true)
    {
        var y = cell.Y;
        var x = cell.X;

        // Top
        if (y > 0)
        {
            yield return new Cell<T>(y - 1, x, m[y - 1][x]);
        }

        // Top-right
        if (diagonal && y > 0 && x < m[y].Length - 1)
        {
            yield return new Cell<T>(y - 1, x + 1, m[y - 1][x + 1]);
        }

        // Right
        if (x < m[y].Length - 1)
        {
            yield return new Cell<T>(y, x + 1, m[y][x + 1]);
        }

        // Bottom-right
        if (diagonal && y < m.Length - 1 && x < m[y].Length - 1)
        {
            yield return new Cell<T>(y + 1, x + 1, m[y + 1][x + 1]);
        }

        // Bottom
        if (y < m.Length - 1)
        {
            yield return new Cell<T>(y + 1, x, m[y + 1][x]);
        }

        // Bottom-left
        if (diagonal && y < m.Length - 1 && x > 0)
        {
            yield return new Cell<T>(y + 1, x - 1, m[y + 1][x - 1]);
        }

        // Left
        if (x > 0)
        {
            yield return new Cell<T>(y, x - 1, m[y][x - 1]);
        }

        // Top-left
        if (diagonal && y > 0 && x > 0)
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

    public override string ToString()
        => $"({Y}, {X}) = {Value}";

    public static implicit operator T(Cell<T> cell)
        => cell.Value;
}
