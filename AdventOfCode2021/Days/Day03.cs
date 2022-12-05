namespace AdventOfCode2021.Days
{
    internal class Day03 : IDay
    {
        public void Run()
        {
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
        }

        private static int BinaryStringToInt(string binaryString)
            => Convert.ToInt32(binaryString, 2);

        private static int CharToInt(char c) => c - '0';
    }
}
