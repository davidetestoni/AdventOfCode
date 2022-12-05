namespace AdventOfCode2022.Days
{
    internal class Day02 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day02.txt");
            var score = 0;

            foreach (var line in lines)
            {
                var inputs = line.Split(' ');
                var enemyMove = ParseMove(inputs[0]);
                var myMove = ParseMove(inputs[1]);

                var outcome = ComputeOutcome(myMove, enemyMove);
                score += (int)myMove + (int)outcome;
            }

            Console.WriteLine(score);

            // PART 2
            score = 0;

            foreach (var line in lines)
            {
                var inputs = line.Split(' ');
                var enemyMove = ParseMove(inputs[0]);
                var outcome = ParseOutcome(inputs[1]);

                var myMove = ComputeMove(enemyMove, outcome);
                score += (int)myMove + (int)outcome;
            }

            Console.WriteLine(score);
        }

        private enum Move
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        private enum Outcome
        {
            Loss = 0,
            Draw = 3,
            Win = 6
        }

        private static Move ParseMove(string input)
            => input switch
            {
                "A" or "X" => Move.Rock,
                "B" or "Y" => Move.Paper,
                "C" or "Z" => Move.Scissors,
                _ => throw new NotImplementedException()
            };

        private static Outcome ParseOutcome(string input)
            => input switch
            {
                "X" => Outcome.Loss,
                "Y" => Outcome.Draw,
                "Z" => Outcome.Win,
                _ => throw new NotImplementedException()
            };

        private static Outcome ComputeOutcome(Move myMove, Move enemyMove)
            => (myMove, enemyMove) switch
            {
                (Move.Rock, Move.Rock) => Outcome.Draw,
                (Move.Paper, Move.Paper) => Outcome.Draw,
                (Move.Scissors, Move.Scissors) => Outcome.Draw,

                (Move.Rock, Move.Paper) => Outcome.Loss,
                (Move.Paper, Move.Scissors) => Outcome.Loss,
                (Move.Scissors, Move.Rock) => Outcome.Loss,

                (Move.Rock, Move.Scissors) => Outcome.Win,
                (Move.Paper, Move.Rock) => Outcome.Win,
                (Move.Scissors, Move.Paper) => Outcome.Win,

                _ => throw new NotImplementedException()
            };

        private static Move ComputeMove(Move enemyMove, Outcome outcome)
            => (enemyMove, outcome) switch
            {
                (Move.Rock, Outcome.Loss) => Move.Scissors,
                (Move.Paper, Outcome.Loss) => Move.Rock,
                (Move.Scissors, Outcome.Loss) => Move.Paper,

                (Move.Rock, Outcome.Draw) => Move.Rock,
                (Move.Paper, Outcome.Draw) => Move.Paper,
                (Move.Scissors, Outcome.Draw) => Move.Scissors,

                (Move.Rock, Outcome.Win) => Move.Paper,
                (Move.Paper, Outcome.Win) => Move.Scissors,
                (Move.Scissors, Outcome.Win) => Move.Rock,

                _ => throw new NotImplementedException()
            };
    }
}
