namespace AdventOfCode.Utils;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> items, int limit)
    {
        if (limit <= 0)
        {
            yield break;
        }

        var itemsList = items.ToList();
        var indices = Enumerable.Range(0, itemsList.Count);

        foreach (var indicesPerm in Permutations(indices, limit))
        {
            yield return indicesPerm.Select(i => itemsList[i]);
        }
    }

    private static IEnumerable<IEnumerable<int>> Permutations(IEnumerable<int> indices, int limit)
    {
        if (limit == 1)
        {
            foreach (var index in indices)
            {
                yield return [index];
            }
            
            yield break;
        }

        var indicesList = indices.ToList();
        var subPermutations = Permutations(indicesList, limit - 1).ToList();

        foreach (var index in indicesList)
        {
            foreach (var subPerm in subPermutations)
            {
                var subPermList = subPerm.ToList();
                
                if (!subPermList.Contains(index))
                {
                    yield return subPermList.Append(index);
                }
            }
        }
    }
}
