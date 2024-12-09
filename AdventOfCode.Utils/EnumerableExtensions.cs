using System.Collections;
using Combinatorics.Collections;

namespace AdventOfCode.Utils;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Permutations<T>(
        this IEnumerable<T> items, bool withRepetition = false)
        => new Permutations<T>(items, withRepetition 
            ? GenerateOption.WithRepetition 
            : GenerateOption.WithoutRepetition);

    public static IEnumerable<IEnumerable<T>> Combinations<T>(
        this IEnumerable<T> items, int length, bool withRepetition = false)
        => new Combinations<T>(
            items, length, withRepetition 
                ? GenerateOption.WithRepetition 
                : GenerateOption.WithoutRepetition);
    
    public static IEnumerable<IEnumerable<T>> Variations<T>(
        this IEnumerable<T> items, int length, bool withRepetition = false)
        => new Variations<T>(
            items, length, withRepetition
                ? GenerateOption.WithRepetition 
                : GenerateOption.WithoutRepetition);
    
    /// <summary>
    /// Searches for a sequence made of the given element repeated and returns
    /// the index of the first element of the sequence.
    /// Returns -1 if the sequence is not found.
    /// </summary>
    public static int IndexOfRepeating<T>(this IEnumerable<T> collection,
        T element, int length)
    {
        var index = 0;
        var occurrences = 0;
        var matching = false;

        using var enumerator = collection.GetEnumerator();
        
        while (enumerator.MoveNext())
        {
            var currentElement = enumerator.Current;
            
            if (currentElement is not null && currentElement.Equals(element))
            {
                if (!matching)
                {
                    matching = true;
                    occurrences = 1;
                }
                else
                {
                    occurrences++;
                }
            }
            else
            {
                occurrences = 0;
                matching = false;
            }
            
            if (occurrences == length)
            {
                return index - length + 1;
            }
            
            index++;
        }
        
        return -1;
    }
}
