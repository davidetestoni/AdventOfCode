namespace AdventOfCode2021.Days
{
    internal class Day02 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day02.txt");
            var x = 0;
            var y = 0;

            foreach (var line in lines)
            {
                var split = line.Split(' ');
                var command = Enum.Parse<Command>(split[0], true);
                var value = int.Parse(split[1]);

                switch (command)
                {
                    case Command.Forward:
                        x += value;
                        break;

                    case Command.Up:
                        y -= value;
                        break;

                    case Command.Down:
                        y += value;
                        break;
                }
            }

            Console.WriteLine(x * y);

            x = 0;
            y = 0;
            var aim = 0;

            foreach (var line in lines)
            {
                var split = line.Split(' ');
                var command = Enum.Parse<Command>(split[0], true);
                var value = int.Parse(split[1]);

                switch (command)
                {
                    case Command.Down:
                        aim += value;
                        break;

                    case Command.Up:
                        aim -= value;
                        break;

                    case Command.Forward:
                        x += value;
                        y += aim * value;
                        break;
                }
            }

            Console.WriteLine(x * y);
        }

        private enum Command
        {
            Forward,
            Down,
            Up
        }
    }
}
