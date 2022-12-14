namespace AdventOfCode2021.Days
{
    internal class Day03 : IDay
    {
        public void Run()
        {
            // TODO: Check if a binary tree can help

            var lines = File.ReadAllLines("Days/Day03.txt");
            var lineLength = lines[0].Length;
            var bitSum = Enumerable.Repeat(0, lineLength).ToArray();

            foreach (var line in lines)
            {
                for (int i = 0; i < lineLength; i++)
                {
                    bitSum[i] += CharToInt(line[i]);
                }
            }

            var gammaBits = bitSum.Select(b => b > (lines.Length / 2) ? 1 : 0).ToArray();
            var epsilonBits = bitSum.Select(b => b > (lines.Length / 2) ? 0 : 1).ToArray();
            var gammaValue = BinaryStringToInt(string.Join(string.Empty, gammaBits));
            var epsilonValue = BinaryStringToInt(string.Join(string.Empty, epsilonBits));

            Console.WriteLine(gammaValue * epsilonValue);

            var mostCommonBits = lines.Select(l => l.ToCharArray()).ToList();
            var leastCommonBits = lines.Select(l => l.ToCharArray()).ToList();

            for (int i = 0; i < lines[0].Length; i++)
            {
                if (mostCommonBits.Count == 1)
                {
                    break;
                }

                var oneCount = mostCommonBits
                    .Where(s => s[i] == '1').Count();

                var zeroCount = mostCommonBits.Count - oneCount;
                var mostCommon = oneCount >= zeroCount ? '1' : '0';
                
                mostCommonBits.RemoveAll(s => s[i] != mostCommon);
            }

            for (int i = 0; i < lines[0].Length; i++)
            {
                if (leastCommonBits.Count == 1)
                {
                    break;
                }

                var oneCount = leastCommonBits
                    .Where(s => s[i] == '1').Count();

                var zeroCount = leastCommonBits.Count - oneCount;
                var leastCommon = oneCount >= zeroCount ? '0' : '1';

                leastCommonBits.RemoveAll(s => s[i] != leastCommon);
            }

            var oxygenGenRating = BinaryStringToInt(
                new string(mostCommonBits.Single()));
            
            var co2ScrubberRating = BinaryStringToInt(
                new string(leastCommonBits.Single()));

            Console.WriteLine(oxygenGenRating * co2ScrubberRating);
        }

        private static int BinaryStringToInt(string binaryString)
            => Convert.ToInt32(binaryString, 2);

        private static int CharToInt(char c) => c - '0';
    }
}
