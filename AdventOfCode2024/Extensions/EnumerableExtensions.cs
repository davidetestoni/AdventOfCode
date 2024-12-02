using System.Numerics;

namespace AdventOfCode2024.Extensions;

internal static class EnumerableExtensions
{
    public static bool IsMonotonic<T>(this IEnumerable<T> source,
        bool increasing, bool strict = false)
        where T : INumber<T>
    {
        using var enumerator = source.GetEnumerator();
        
        // If there are no elements, it is considered monotonic
        if (!enumerator.MoveNext())
        {
            return true;
        }

        var previous = enumerator.Current;
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;

            if (strict && current == previous)
            {
                return false;
            }
            
            if (increasing && current < previous)
            {
                return false;
            }

            if (!increasing && current > previous)
            {
                return false;
            }

            previous = current;
        }

        return true;
    }
    
    public static IEnumerable<(T First, T Second)> AdjacentPairs<T>(
        this IEnumerable<T> source)
    {
        using var enumerator = source.GetEnumerator();
        
        if (!enumerator.MoveNext())
        {
            yield break;
        }

        var previous = enumerator.Current;
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            yield return (previous, current);
            previous = current;
        }
    }
    
    public static IEnumerable<T> ExcludingIndex<T>(this IEnumerable<T> source,
        int index)
    {
        using var enumerator = source.GetEnumerator();
        
        for (var i = 0; enumerator.MoveNext(); i++)
        {
            if (i == index)
            {
                continue;
            }

            yield return enumerator.Current;
        }
    }
}
