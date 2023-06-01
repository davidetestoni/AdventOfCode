namespace AdventOfCode2021.Days
{
    internal class Day04 : IDay
    {
        class Number
        {
            public int Value { get; set; }
            public bool Marked { get; set; }

            public Number(int value)
            {
                Value = value;
            }
        }

        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day04.txt");

            // The first line has the extractions
            var extractedNumbers = lines[0].Split(',')
                .Select(n => int.Parse(n));

            var boards = new List<Board>();
            Board currentBoard = new();

            foreach (var line in lines.Skip(2))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    currentBoard = new();
                    boards.Add(currentBoard);
                    continue;
                }

                var row = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => new Number(int.Parse(n))).ToArray();

                currentBoard.Rows.Add(row);
            }

            foreach (var extractedNumber in extractedNumbers)
            {
                foreach (var board in boards)
                {
                    board.MarkNumbers(extractedNumber);

                    if (board.CheckWin())
                    {
                        var score = board.UnmarkedSum * extractedNumber;
                        Console.WriteLine($"Score: {score}");

                        goto part2;
                    }
                }
            }

            // PART 2
            part2:
            boards.ForEach(b => b.Reset());

            var winningBoards = new HashSet<Guid>();

            foreach (var extractedNumber in extractedNumbers)
            {
                foreach (var board in boards)
                {
                    board.MarkNumbers(extractedNumber);

                    if (!winningBoards.Contains(board.Id) && board.CheckWin())
                    {
                        winningBoards.Add(board.Id);

                        // If it's the last board to win
                        if (winningBoards.Count == boards.Count)
                        {
                            var score = board.UnmarkedSum * extractedNumber;
                            Console.WriteLine($"Score: {score}");
                        }
                    }
                }
            }
        }

        class Board
        {
            public Guid Id { get; set; } = Guid.NewGuid();

            public List<Number[]> Rows { get; set; } = new();

            public int UnmarkedSum => Rows.SelectMany(r => r)
                .Where(n => !n.Marked).Sum(n => n.Value);

            public void MarkNumbers(int extractedNumber)
            {
                foreach (var row in Rows)
                {
                    foreach (var num in row)
                    {
                        if (num.Value == extractedNumber)
                        {
                            num.Marked = true;
                        }
                    }
                }
            }

            public void Reset()
            {
                foreach (var row in Rows)
                {
                    foreach (var num in row)
                    {
                        num.Marked = false;
                    }
                }
            }

            public bool CheckWin()
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    if (Rows[i].All(n => n.Marked))
                    {
                        return true;
                    }
                }

                // Columns
                for (int i = 0; i < Rows[0].Length; i++)
                {
                    if (Rows.All(r => r[i].Marked))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
