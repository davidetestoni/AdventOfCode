namespace AdventOfCode2021.Days
{
    internal class Day13 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day13.txt");
            var points = new List<Point>();
            var folds = new List<Fold>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] split;

                if (line.StartsWith("fold"))
                {
                    var foldInstruction = line.Replace("fold along ", "");
                    split = foldInstruction.Split('=');
                    var axis = int.Parse(split[1]);
                    var direction = split[0] == "x"
                        ? FoldDirection.RightToLeft
                        : FoldDirection.DownToUp;

                    folds.Add(new Fold(axis, direction));

                    continue;
                }

                split = line.Split(',');
                var x = int.Parse(split[0]);
                var y = int.Parse(split[1]);

                points.Add(new Point(x, y));
            }

            var rows = points.Max(p => p.Y) + 1;
            var columns = points.Max(p => p.X) + 1;

            var m = Enumerable.Repeat(false, rows)
                .Select(_ => Enumerable.Repeat(false, columns).ToArray())
                .ToArray();

            foreach (var point in points)
            {
                m[point.Y][point.X] = true;
            }

            m = FoldPaper(m, folds[0]);

            var visibleDots = m.Sum(row => row.Count(e => e));

            Console.WriteLine($"Visible after 1st fold: {visibleDots}");

            foreach (var fold in folds.Skip(1))
            {
                m = FoldPaper(m, fold);
            }

            PrintMatrix(m);
        }

        record Fold(int Axis, FoldDirection Direction);

        record Point(int X, int Y);

        enum FoldDirection
        {
            DownToUp,
            RightToLeft
        }

        void PrintMatrix(bool[][] m)
        {
            foreach (var r in m)
            {
                Console.WriteLine(new string(r.Select(x => x ? '#' : '.').ToArray()));
            }
        }

        bool[][] FoldPaper(bool[][] m, Fold fold)
        {
            if (fold.Direction is FoldDirection.DownToUp)
            {
                return FoldUp(m, fold.Axis);
            }

            return FoldLeft(m, fold.Axis);
        }

        bool[][] FoldUp(bool[][] m, int y)
        {
            for (int i = y + 1; i < m.Length; i++)
            {
                for (int j = 0; j < m[i].Length; j++)
                {
                    if (m[i][j])
                    {
                        m[y * 2 - i][j] = true;
                    }
                }
            }

            return m.Take(y).ToArray();
        }

        bool[][] FoldLeft(bool[][] m, int x)
        {
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = x + 1; j < m[i].Length; j++)
                {
                    if (m[i][j])
                    {
                        m[i][x * 2 - j] = true;
                    }
                }
            }

            return m.Select(row => row.Take(x).ToArray()).ToArray();
        }
    }
}
