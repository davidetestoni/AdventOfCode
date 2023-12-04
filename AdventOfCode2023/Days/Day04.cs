namespace AdventOfCode2023.Days;

internal class Day04 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day04.txt");

        var cards = new List<Card>();
        var points = 0L;

        // Part 1
        for (int i = 0; i < lines.Length; i++)
        {
            var input = lines[i].Split(':')[1];

            var winningNumbers = input.Split('|')[0].Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x))
                .ToArray();

            var numbers = input.Split('|')[1].Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x))
                .ToArray();

            var card = new Card(winningNumbers, numbers);
            cards.Add(card);

            points += card.CalcPoints();
        }

        Console.WriteLine($"Points: {points}");

        // Part 2
        for (int i = 0; i < cards.Count; i++)
        {
            var matches = cards[i].CalcMatches();

            for (int j = i + 1; j <= i + matches; j++)
            {
                cards[j].Quantity += cards[i].Quantity;
            }

            Console.WriteLine($"Index: {i} | Quantity: {cards[i].Quantity}");
        }

        var count = cards.Sum(c => c.Quantity);
        Console.WriteLine($"Count: {count}");
    }

    class Card
    {
        public long Quantity { get; set; } = 1;

        public int[] WinningNumbers { get; set; }

        public int[] Numbers { get; set; }

        public Card(int[] winningNumbers, int[] numbers)
        {
            WinningNumbers = winningNumbers;
            Numbers = numbers;
        }

        public long CalcPoints()
        {
            var matches = CalcMatches();

            return matches > 0
                ? (long)Math.Pow(2, matches - 1)
                : 0;
        }

        public long CalcMatches()
            => Numbers.Count(n => WinningNumbers.Contains(n));
    }
}
