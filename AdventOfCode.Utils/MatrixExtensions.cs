namespace AdventOfCode.Utils;

public static class MatrixExtensions
{
    /// <summary>
    /// Yields all cells of the matrix.
    /// </summary>
    public static IEnumerable<Cell<T>> Cells<T>(this T[][] m)
    {
        for (var y = 0; y < m.Length; y++)
        {
            for (var x = 0; x < m[y].Length; x++)
            {
                yield return new Cell<T>(y, x, m[y][x]);
            }
        }
    }
    
    /// <summary>
    /// Checks if the given coordinates are within the bounds of the matrix.
    /// </summary>
    public static bool IsWithinBounds<T>(this T[][] m, int y, int x)
        => y >= 0 && y < m.Length && x >= 0 && x < m[0].Length;

    /// <summary>
    /// Yields all cells above the given Y with the same X.
    /// </summary>
    public static IEnumerable<Cell<T>> AboveCells<T>(this T[][] m, Cell<T> cell)
    {
        for (var y = cell.Y - 1; y >= 0; y--)
        {
            yield return new Cell<T>(y, cell.X, m[y][cell.X]);
        }
    }

    /// <summary>
    /// Yields all cells below the given Y with the same X.
    /// </summary>
    public static IEnumerable<Cell<T>> BelowCells<T>(this T[][] m, Cell<T> cell)
    {
        for (var y = cell.Y + 1; y < m.Length; y++)
        {
            yield return new Cell<T>(y, cell.X, m[y][cell.X]);
        }
    }

    /// <summary>
    /// Yields all cells left of the given X with the same Y.
    /// </summary>
    public static IEnumerable<Cell<T>> LeftCells<T>(this T[][] m, Cell<T> cell)
    {
        for (var x = cell.X - 1; x >= 0; x--)
        {
            yield return new Cell<T>(cell.Y, x, m[cell.Y][x]);
        }
    }

    /// <summary>
    /// Yields all cells right of the given X with the same Y.
    /// </summary>
    public static IEnumerable<Cell<T>> RightCells<T>(this T[][] m, Cell<T> cell)
    {
        for (var x = cell.X + 1; x < m[cell.Y].Length; x++)
        {
            yield return new Cell<T>(cell.Y, x, m[cell.Y][x]);
        }
    }

    /// <summary>
    /// Yields all cells in the diagonal from bottom-right to top-left, 
    /// starting from the given cell and moving diagonally up and to the left.
    /// </summary>
    public static IEnumerable<Cell<T>> TopLeftCells<T>(this T[][] m, Cell<T> cell)
    {
        var y = cell.Y - 1;
        var x = cell.X - 1;

        while (y >= 0 && x >= 0)
        {
            yield return new Cell<T>(y, x, m[y][x]);
            y--;
            x--;
        }
    }
    
    /// <summary>
    /// Yields all cells in the diagonal from bottom-left to top-right, 
    /// starting from the given cell and moving diagonally up and to the right.
    /// </summary>
    public static IEnumerable<Cell<T>> TopRightCells<T>(this T[][] m, Cell<T> cell)
    {
        var y = cell.Y - 1;
        var x = cell.X + 1;

        while (y >= 0 && x < m[y].Length)
        {
            yield return new Cell<T>(y, x, m[y][x]);
            y--;
            x++;
        }
    }

    /// <summary>
    /// Yields all cells in the diagonal from top-right to bottom-left, 
    /// starting from the given cell and moving diagonally down and to the left.
    /// </summary>
    public static IEnumerable<Cell<T>> BottomLeftCells<T>(this T[][] m, Cell<T> cell)
    {
        var y = cell.Y + 1;
        var x = cell.X - 1;

        while (y < m.Length && x >= 0)
        {
            yield return new Cell<T>(y, x, m[y][x]);
            y++;
            x--;
        }
    }
    
    /// <summary>
    /// Yields all cells in the diagonal from top-left to bottom-right, 
    /// starting from the given cell and moving diagonally down and to the right.
    /// </summary>
    public static IEnumerable<Cell<T>> BottomRightCells<T>(this T[][] m, Cell<T> cell)
    {
        var y = cell.Y + 1;
        var x = cell.X + 1;

        while (y < m.Length && x < m[y].Length)
        {
            yield return new Cell<T>(y, x, m[y][x]);
            y++;
            x++;
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

public readonly record struct Cell<T>(int Y, int X, T Value)
{
    public override string ToString()
        => $"({Y}, {X}) = {Value}";

    public static implicit operator T(Cell<T> cell)
        => cell.Value;
}
