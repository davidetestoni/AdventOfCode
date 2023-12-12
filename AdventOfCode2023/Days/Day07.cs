using System.ComponentModel.Design;

namespace AdventOfCode2023.Days;

internal class Day07 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day07.txt");

        // Part 1
        var hands = lines
            .Select(l => l.Split(' '))
            .Select(l => new Hand(l[0], long.Parse(l[1])))
            .ToArray();

        var totalWinnings = hands.Order()
            .Select((h, i) => h.Bid * (i + 1))
            .Sum();

        Console.WriteLine($"Total winnings: {totalWinnings}");

        // Part 2
        var hands2 = lines
            .Select(l => l.Split(' '))
            .Select(l => new Hand2(l[0], long.Parse(l[1])))
            .ToArray();

        foreach (var hand in hands2.Order())
        {
            foreach (var card in hand.Cards)
            {
                Console.ForegroundColor = card switch
                {
                    'J' => ConsoleColor.Green,
                    _ => ConsoleColor.White
                };

                Console.Write(card);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($" ({hand.Type})");
        }

        var totalWinnings2 = hands2.Order()
            .Select((h, i) => h.Bid * (i + 1))
            .Sum();

        Console.WriteLine($"Total winnings: {totalWinnings2}");
    }

    class Hand : IComparable<Hand>
    {
        public char[] Cards { get; }

        public long Bid { get; }

        public HandType Type { get; }

        public Hand(string cards, long bid)
        {
            Cards = cards.ToCharArray();
            Bid = bid;

            Type = GetHandType();
        }

        private HandType GetHandType() => Cards.Distinct().Count() switch
        {
            1 => HandType.FiveOfAKind,
            2 => Cards.GroupBy(c => c).Any(g => g.Count() == 4) ? HandType.FourOfAKind : HandType.FullHouse,
            3 => Cards.GroupBy(c => c).Any(g => g.Count() == 3) ? HandType.ThreeOfAKind : HandType.TwoPairs,
            4 => HandType.OnePair,
            _ => HandType.HighCard
        };

        public int CompareTo(Hand? other)
        {
            // If the hand types are different, compare them basing
            // on the enum value
            if (Type != other?.Type)
            {
                return Type.CompareTo(other?.Type);
            }

            // Otherwise, compare the cards
            for (var i = 0; i < Cards.Length; i++)
            {
                if (Cards[i] != other.Cards[i])
                {
                    return CompareCard(Cards[i], other.Cards[i]);
                }
            }

            return 0;
        }

        private static int CompareCard(char a, char b)
        {
            var order = "23456789TJQKA";
            return order.IndexOf(a).CompareTo(order.IndexOf(b));
        }
    }

    class Hand2 : IComparable<Hand2>
    {
        public char[] Cards { get; }

        public long Bid { get; }

        public HandType Type { get; }

        public Hand2(string cards, long bid)
        {
            Cards = cards.ToCharArray();
            Bid = bid;

            Type = GetHandType();
        }

        private HandType GetHandType()
        {
            // Now the jokers are wildcards
            var distinct = Cards.Distinct().Count();
            var jokers = Cards.Count(c => c == 'J');
            var biggestGroup = Cards.GroupBy(c => c).Max(g => g.Count());

            // If all the cards are the same it's a five of a kind
            if (distinct == 1)
            {
                return HandType.FiveOfAKind;
            }
            else if (distinct == 2)
            {
                // If one of the cards is a joker, it's a five of a kind
                if (jokers > 0)
                {
                    return HandType.FiveOfAKind;
                }

                // Otherwise, it's either a four of a kind or a full house
                return biggestGroup == 4
                    ? HandType.FourOfAKind
                    : HandType.FullHouse;
            }
            else if (distinct == 3)
            {
                // e.g. JJJ12 or JJ112 = four of a kind
                if (jokers == 2 || jokers == 3)
                {
                    return HandType.FourOfAKind;
                }

                // e.g. J1112 is four of a kind, J1122 is a full house
                else if (jokers == 1)
                {
                    return biggestGroup == 3
                        ? HandType.FourOfAKind
                        : HandType.FullHouse;
                }

                // Otherwise, it's either a three of a kind or two pairs
                return biggestGroup == 3
                    ? HandType.ThreeOfAKind
                    : HandType.TwoPairs;
            }
            else if (distinct == 4)
            {
                // e.g. JJ123 or J1123 = three of a kind
                if (jokers > 0)
                {
                    return HandType.ThreeOfAKind;
                }

                // Otherwise, it's a pair
                return HandType.OnePair;
            }
            else
            {
                // If one of the cards is a joker, it's a one pair
                if (jokers > 0)
                {
                    return HandType.OnePair;
                }

                // Otherwise, it's a high card
                return HandType.HighCard;
            }
        }

        public int CompareTo(Hand2? other)
        {
            // If the hand types are different, compare them basing
            // on the enum value
            if (Type != other?.Type)
            {
                return Type.CompareTo(other?.Type);
            }

            // Otherwise, compare the cards
            for (var i = 0; i < Cards.Length; i++)
            {
                if (Cards[i] != other.Cards[i])
                {
                    return CompareCard(Cards[i], other.Cards[i]);
                }
            }

            return 0;
        }

        private static int CompareCard(char a, char b)
        {
            // This time, the Jokers are the lowest cards
            var order = "AKQT98765432J";
            return order.IndexOf(b).CompareTo(order.IndexOf(a));
        }
    }

    enum HandType
    {
        FiveOfAKind = 6,
        FourOfAKind = 5,
        FullHouse = 4,
        ThreeOfAKind = 3,
        TwoPairs = 2,
        OnePair = 1,
        HighCard = 0
    }
}
