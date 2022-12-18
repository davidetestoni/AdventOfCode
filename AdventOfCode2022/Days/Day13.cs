using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days
{
    internal class Day13 : IDay
    {
        // Not used but might be useful in the future
        // It's a regex to match the outer list e.g. [1,[2,3]]
        // without stopping at the first ']' by keeping a counter
        private static readonly Regex OUTER_LIST_REGEX = 
            new(@"\[(?>\[(?<c>)|[^\[\]]+|\](?<-c>))*(?(c)(?!))\]",
                RegexOptions.Compiled);

        private static readonly string DIVIDER_1 = "[[2]]";
        private static readonly string DIVIDER_2 = "[[6]]";

        public void Run()
        {
            var lines = File.ReadLines("Days/Day13.txt");
            
            var pairs = lines
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Chunk(2)
                .Select<string[], (string left, string right)>(c => (c[0], c[1]))
                .ToArray();

            var correctOrder = 0;

            for (var i = 0; i < pairs.Length; i++)
            {
                var leftSyntaxTree = ParseSyntaxTree(ref pairs[i].left);
                var rightSyntaxTree = ParseSyntaxTree(ref pairs[i].right);

                if (IsCorrectOrder(leftSyntaxTree, rightSyntaxTree)
                    is Outcome.Correct or Outcome.Undetermined)
                {
                    correctOrder += i + 1;
                }
            }

            Console.WriteLine(correctOrder);

            var packets = lines
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Concat(new string[] { DIVIDER_1, DIVIDER_2 })
                .ToList();

            // Cache the syntax trees to not compute them multiple times
            var cache = new Dictionary<string, Elem>();

            packets.Sort(delegate (string left, string right)
            {
                if (!cache.TryGetValue(left, out Elem? leftSyntaxTree))
                {
                    leftSyntaxTree = ParseSyntaxTree(ref left);
                }

                if (!cache.TryGetValue(left, out Elem? rightSyntaxTree))
                {
                    rightSyntaxTree = ParseSyntaxTree(ref right);
                }

                return (int)IsCorrectOrder(leftSyntaxTree, rightSyntaxTree);
            });

            var firstDividerIndex = packets.IndexOf(DIVIDER_1) + 1;
            var secondDividerIndex = packets.IndexOf(DIVIDER_2) + 1;

            Console.WriteLine(firstDividerIndex * secondDividerIndex);
        }

        private static Elem ParseSyntaxTree(ref string input)
        {
            // If it's a list
            if (input.StartsWith('['))
            {
                input = input[1..];

                // Declare the list element
                var list = new ListElem();

                // While the list didn't end, parse element by element
                while (!input.StartsWith(']'))
                {
                    list.Items.Add(ParseSyntaxTree(ref input));
                }

                // Consume the ']'
                input = input[1..];

                // If there is a trailing ',' consume that as well
                if (input.StartsWith(','))
                {
                    input = input[1..];
                }

                return list;
            }

            // Otherwise if it's a simple integer, just parse it
            var match = Regex.Match(input, @"^(\d+),?");
            var intValue = int.Parse(match.Groups[1].Value);
            input = input[match.Length..];
            return new IntElem { Value = intValue };
        }

        private static Outcome IsCorrectOrder(Elem left, Elem right)
        {
            // If they are two integers, simply check if left <= right
            if (left is IntElem leftInt && right is IntElem rightInt)
            {
                // Console.WriteLine($"Comparing {(left as IntElem)?.Value} with {(right as IntElem)?.Value}");

                if (leftInt.Value == rightInt.Value)
                {
                    return Outcome.Undetermined;
                }
                
                return leftInt.Value <= rightInt.Value
                    ? Outcome.Correct
                    : Outcome.Incorrect;
            }

            // If left is an integer while right is a list,
            // convert left to a single-value list
            else if (left is IntElem && right is ListElem)
            {
                left = new ListElem { Items = new() { left } };
            }

            // If right is an integer while left is a list,
            // convert right to a single-value list
            else if (left is ListElem && right is IntElem)
            {
                right = new ListElem { Items = new() { right } };
            }

            // If they are two lists, compare each item
            if (left is ListElem leftList && right is ListElem rightList)
            {
                var maxLength = Math.Max(leftList.Length, rightList.Length);

                for (int i = 0; i < maxLength; i++)
                {
                    // If the left list ran out and we're still in the loop,
                    // this means the right list is longer, so it's correct
                    if (i >= leftList.Length)
                    {
                        return Outcome.Correct;
                    }

                    // If the right list ran out and we're still in the loop,
                    // this means the right list is shorted, so it's incorrect
                    else if (i >= rightList.Length)
                    {
                        return Outcome.Incorrect;
                    }

                    // Otherwise simply compare the two
                    var outcome = IsCorrectOrder(leftList.Items[i], rightList.Items[i]);
                    
                    // If the outcome is undetermined, continue checking the
                    // next element
                    if (outcome is Outcome.Undetermined)
                    {
                        continue;
                    }

                    return outcome;
                }

                // If we are here, it means all the elements were the same,
                // so the outcome is still undetermined.
                return Outcome.Undetermined;
            }

            throw new Exception($"This should not happen");
        }

        private enum Outcome
        {
            Correct = -1, // left < right
            Undetermined = 0, // left > right
            Incorrect = 1 // left == right
        }

        private class Elem
        {

        }

        private class IntElem : Elem
        {
            public int Value { get; set; }
        }

        private class ListElem : Elem
        {
            public List<Elem> Items { get; set; } = new();

            public int Length => Items.Count;
        }
    }
}
