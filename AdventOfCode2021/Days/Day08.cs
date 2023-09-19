namespace AdventOfCode2021.Days
{
    internal class Day08 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day08.txt");

            var digitsList = lines.Select(
                l => l.Replace("| ", "").Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToArray();

            var counter = 0;

            foreach (var digits in digitsList)
            {
                // Output values are the last 4 after the first 10
                foreach (var outputDigit in digits.Skip(10))
                {
                    if (outputDigit.Length is 2 or 4 or 3 or 7)
                    {
                        counter++;
                    }
                }
            }

            Console.WriteLine($"Digits 1, 4, 7 or 8: {counter}");

            // Every line has 1 occurrence of every digit as values on the left
            var sum = 0;

            foreach (var digits in digitsList)
            {
                var inputDigits = digits.Take(10).ToArray();
                var outputDigits = digits.Skip(10).ToArray();

                var n = new Dictionary<int, char[]>(); // Numbers
                var s = new Dictionary<char, char>(); // Segments

                n[1] = inputDigits.Single(v => v.Length is 2).ToCharArray();
                n[4] = inputDigits.Single(v => v.Length is 4).ToCharArray();
                n[7] = inputDigits.Single(v => v.Length is 3).ToCharArray();
                n[8] = inputDigits.Single(v => v.Length is 7).ToCharArray();

                // 7 - 1 gives us 'a'
                s['a'] = n[7].Except(n[1]).Single();

                // 4 + 7 gives us something that is almost 9, off by 'g'
                var fourAndSeven = n[4].Union(n[7]).ToArray();
                n[9] = inputDigits.Single(v => v.Except(fourAndSeven).Count() == 1 && v.Intersect(fourAndSeven).Count() == 5).ToCharArray();
                s['g'] = n[9].Except(fourAndSeven).Single();

                // 8 - 9 is 'e'
                s['e'] = n[8].Except(n[9]).Single();

                // 9 - (4 - 1) gives us something that is almost 3, off by 1
                n[3] = inputDigits.Single(v => v.Except(n[9].Except(n[4].Except(n[1]))).Count() == 1).ToCharArray();
                s['b'] = n[9].Except(n[3]).Single();

                // 'a', 'g', 'e', 'b' + 1 gives us 0 and 'd'
                n[0] = new char[] { s['a'], s['g'], s['e'], s['b'] }.Concat(n[1]).ToArray();
                s['d'] = n[8].Except(n[0]).Single();

                // The number that is off by 1 from abdeg is 6 and we can get 'f' and 'c'
                var abdeg = new char[] { s['a'], s['b'], s['d'], s['e'], s['g'] };
                n[6] = inputDigits.Single(v => v.Except(abdeg).Count() == 1 && v.Intersect(abdeg).Count() == 5).ToCharArray();
                s['f'] = n[6].Intersect(n[1]).Single();
                s['c'] = n[8].Except(n[6]).Single();

                // Now we have all segments and we can get the last numbers
                n[2] = new char[] { s['a'], s['c'], s['d'], s['e'], s['g'] };
                n[5] = new char[] { s['a'], s['b'], s['d'], s['f'], s['g'] };

                var digit1 = DecodeDigit(n, outputDigits[0]);
                var digit2 = DecodeDigit(n, outputDigits[1]);
                var digit3 = DecodeDigit(n, outputDigits[2]);
                var digit4 = DecodeDigit(n, outputDigits[3]);

                sum += digit1 * 1000 + digit2 * 100 + digit3 * 10 + digit4;
            }

            Console.WriteLine($"Sum: {sum}");
        }

        private int DecodeDigit(Dictionary<int, char[]> n, string outputValue)
        {
            var chars = outputValue.ToCharArray();
            return n.First(x => chars.Length == x.Value.Length && chars.Except(x.Value).Count() == 0).Key;
        }
    }
}
