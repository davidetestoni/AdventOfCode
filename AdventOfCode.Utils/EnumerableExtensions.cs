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
}
