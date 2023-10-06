namespace AdventOfCode2021.Days
{
    internal class Day10 : IDay
    {
        private readonly Dictionary<char, char> _mappings = new()
        {
            ['('] = ')',
            [')'] = '(',
            ['['] = ']',
            [']'] = '[',
            ['{'] = '}',
            ['}'] = '{',
            ['<'] = '>',
            ['>'] = '<'
        };

        private readonly Dictionary<char, int> _syntaxErrorScores = new()
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137
        };

        private readonly Dictionary<char, int> _autoCompletionScores = new()
        {
            [')'] = 1,
            [']'] = 2,
            ['}'] = 3,
            ['>'] = 4
        };

        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day10.txt");

            var score = lines
                .Select(l => GetSyntaxErrorScore(l))
                .Sum();

            Console.WriteLine($"Syntax error score: {score}");

            var incompleteLines = lines
                .Where(l => GetSyntaxErrorScore(l) == 0)
                .ToList();

            score = incompleteLines
                .Select(l => GetAutoCompletionScore(l))
                .OrderBy(x => x)
                .ToArray()
                [incompleteLines.Count / 2];

            Console.WriteLine($"Auto completion score: {score}");
        }

        private long GetSyntaxErrorScore(string line)
        {
            var stack = new Stack<char>();

            foreach (var c in line)
            {
                // If we find an open parenthesis, push to stack
                if (c == '(' || c == '[' || c == '{' || c == '<')
                {
                    stack.Push(c);
                    continue;
                }

                // If it's a close parenthesis, pop the last parenthesis
                // from the stack and compare it
                var match = stack.Pop();

                // If it's not the correct match, return the syntax error score
                if (_mappings[c] != match)
                {
                    return _syntaxErrorScores[c];
                }
            }

            return 0;
        }

        private long GetAutoCompletionScore(string line)
        {
            var stack = new Stack<char>();

            foreach (var c in line)
            {
                // If we find an open parenthesis, push to stack
                if (c == '(' || c == '[' || c == '{' || c == '<')
                {
                    stack.Push(c);
                    continue;
                }

                // If it's a close parenthesis, pop the stack
                stack.Pop();
            }

            var score = 0L;

            // Now the stack has the incomplete open parenthesis
            while (stack.Count > 0)
            {
                var match = _mappings[stack.Pop()];

                score *= 5;
                score += _autoCompletionScores[match];
            }

            return score;
        }
    }
}
