namespace AdventOfCode2022.Days
{
    internal class Day11 : IDay
    {
        public void Run()
        {
            var monkeys = CreateMonkeys();

            foreach (var _ in Enumerable.Range(1, 20))
            {
                PlayRound(monkeys, adjustWorryLevel: x => x / 3);
            }

            var monkeyBusiness = monkeys
                .Select(m => m.TotalInspected)
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate((x1, x2) => x1 * x2);

            Console.WriteLine(monkeyBusiness);

            monkeys = CreateMonkeys();
            var moduloProduct = monkeys.Aggregate(1, (mod, monkey) => mod * monkey.DivisibleBy);

            foreach (var _ in Enumerable.Range(1, 10000))
            {
                PlayRound(monkeys, adjustWorryLevel: x => x % moduloProduct);
            }

            monkeyBusiness = monkeys
                .Select(m => m.TotalInspected)
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate((x1, x2) => x1 * x2);

            Console.WriteLine(monkeyBusiness);
        }

        private static List<Monkey> CreateTestMonkeys() => new List<Monkey>
            {
                new ()
                {
                    Items = new Queue<long>(new[] { 79L, 98L }),
                    Operation = x => x * 19,
                    DivisibleBy = 23,
                    OnTrueRecipient = 2,
                    OnFalseRecipient = 3
                },
                new()
                {
                    Items = new Queue<long>(new[] { 54L, 65L, 75L, 74L }),
                    Operation = x => x + 6,
                    DivisibleBy = 19,
                    OnTrueRecipient = 2,
                    OnFalseRecipient = 0
                },
                new()
                {
                    Items = new Queue<long>(new[] { 79L, 60L, 97L }),
                    Operation = x => x * x,
                    DivisibleBy = 13,
                    OnTrueRecipient = 1,
                    OnFalseRecipient = 3
                },
                new()
                {
                    Items = new Queue<long>(new[] { 74L }),
                    Operation = x => x + 3,
                    DivisibleBy = 17,
                    OnTrueRecipient = 0,
                    OnFalseRecipient = 1
                },
            };

        private static List<Monkey> CreateMonkeys() => new List<Monkey>
            {
                // Monkey 0
                new()
                {
                    Items = new Queue<long>(new[] { 54L, 82, 90, 88, 86, 54 }),
                    Operation = x => x * 7,
                    DivisibleBy = 11,
                    OnTrueRecipient = 2,
                    OnFalseRecipient = 6
                },

                // Monkey 1
                new()
                {
                    Items = new Queue<long>(new[] { 91L, 65 }),
                    Operation = x => x * 13,
                    DivisibleBy = 5,
                    OnTrueRecipient = 7,
                    OnFalseRecipient = 4
                },

                // Monkey 2
                new()
                {
                    Items = new Queue<long>(new[] { 62L, 54, 57, 92, 83, 63, 63 }),
                    Operation = x => x + 1,
                    DivisibleBy = 7,
                    OnTrueRecipient = 1,
                    OnFalseRecipient = 7
                },

                // Monkey 3
                new()
                {
                    Items = new Queue<long>(new[] { 67L, 72, 68 }),
                    Operation = x => x * x,
                    DivisibleBy = 2,
                    OnTrueRecipient = 0,
                    OnFalseRecipient = 6
                },

                // Monkey 4
                new()
                {
                    Items = new Queue<long>(new[] { 68L, 89, 90, 86, 84, 57, 72, 84 }),
                    Operation = x => x + 7,
                    DivisibleBy = 17,
                    OnTrueRecipient = 3,
                    OnFalseRecipient = 5
                },

                // Monkey 5
                new()
                {
                    Items = new Queue<long>(new[] { 79L, 83, 64, 58 }),
                    Operation = x => x + 6,
                    DivisibleBy = 13,
                    OnTrueRecipient = 3,
                    OnFalseRecipient = 0
                },

                // Monkey 6
                new()
                {
                    Items = new Queue<long>(new[] { 96L, 72, 89, 70, 88 }),
                    Operation = x => x + 4,
                    DivisibleBy = 3,
                    OnTrueRecipient = 1,
                    OnFalseRecipient = 2
                },

                // Monkey 7
                new()
                {
                    Items = new Queue<long>(new[] { 79L }),
                    Operation = x => x + 8,
                    DivisibleBy = 19,
                    OnTrueRecipient = 4,
                    OnFalseRecipient = 5
                }
            };

        private static void PlayRound(List<Monkey> monkeys, Func<long, long> adjustWorryLevel)
            => monkeys.ForEach(m => m.InspectItems(monkeys, adjustWorryLevel));

        private class Monkey
        {
            public long TotalInspected { get; private set; } = 0;
            public Queue<long> Items { get; set; } = new();
            public Func<long, long> Operation { get; set; } = _ => 0;
            public int DivisibleBy { get; set; } = 0;
            public int OnTrueRecipient { get; set; } = 0;
            public int OnFalseRecipient { get; set; } = 0;

            public void InspectItems(List<Monkey> monkeys, Func<long, long> adjustWorryLevel)
            {
                while (Items.Any())
                {
                    // Monkey takes the item out for inspection
                    var item = Items.Dequeue();
                    TotalInspected++;

                    // Worry level increases
                    item = Operation(item);

                    // Monkey gets bored
                    item = adjustWorryLevel(item);

                    // Monkey throws the item to the correct recipient
                    var recipient = item % DivisibleBy == 0
                        ? monkeys[OnTrueRecipient]
                        : monkeys[OnFalseRecipient];

                    recipient.Items.Enqueue(item);
                }
            }
        }
    }
}
