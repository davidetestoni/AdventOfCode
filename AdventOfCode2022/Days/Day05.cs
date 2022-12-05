using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days
{
    internal class Day05 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day05.txt");

            /*
            [N]         [C]     [Z]            
            [Q] [G]     [V]     [S]         [V]
            [L] [C]     [M]     [T]     [W] [L]
            [S] [H]     [L]     [C] [D] [H] [S]
            [C] [V] [F] [D]     [D] [B] [Q] [F]
            [Z] [T] [Z] [T] [C] [J] [G] [S] [Q]
            [P] [P] [C] [W] [W] [F] [W] [J] [C]
            [T] [L] [D] [G] [P] [P] [V] [N] [R]
             1   2   3   4   5   6   7   8   9 
            */

            var stacks = new Stack<char>[]
            {
                new("TPZCSLQN".ToCharArray()),
                new("LPTVHCG".ToCharArray()),
                new("DCZF".ToCharArray()),
                new("GWTDLMVC".ToCharArray()),
                new("PWC".ToCharArray()),
                new("PFJDCTSZ".ToCharArray()),
                new("VWGBD".ToCharArray()),
                new("NJSQHW".ToCharArray()),
                new("RCQFSLV".ToCharArray())
            };

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
                var amount = int.Parse(match.Groups[1].Value);
                var from = int.Parse(match.Groups[2].Value);
                var to = int.Parse(match.Groups[3].Value);

                for (int i = 0; i < amount; i++)
                {
                    stacks[to-1].Push(stacks[from-1].Pop());
                }
            }

            var result = string.Join(
                string.Empty, stacks.Select(s => s.Peek()));

            Console.WriteLine(result);

            stacks = new Stack<char>[]
            {
                new("TPZCSLQN".ToCharArray()),
                new("LPTVHCG".ToCharArray()),
                new("DCZF".ToCharArray()),
                new("GWTDLMVC".ToCharArray()),
                new("PWC".ToCharArray()),
                new("PFJDCTSZ".ToCharArray()),
                new("VWGBD".ToCharArray()),
                new("NJSQHW".ToCharArray()),
                new("RCQFSLV".ToCharArray())
            };

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
                var amount = int.Parse(match.Groups[1].Value);
                var from = int.Parse(match.Groups[2].Value);
                var to = int.Parse(match.Groups[3].Value);
                var buffer = new List<char>();

                for (int i = 0; i < amount; i++)
                {
                    buffer.Insert(0, stacks[from - 1].Pop());
                }

                buffer.ForEach(c => stacks[to - 1].Push(c));
            }

            result = string.Join(
                string.Empty, stacks.Select(s => s.Peek()));

            Console.WriteLine(result);
        }
    }
}
