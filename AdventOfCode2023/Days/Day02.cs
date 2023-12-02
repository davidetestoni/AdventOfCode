using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days;

internal class Day02 : IDay
{
    public void Run()
    {
        var lines = File.ReadAllLines("Days/Day02.txt");

        var games = lines.Select((l, i) =>
        {
            l = l.Split(':')[1];

            return new Game
            {
                Id = i + 1,
                Draws = l.Split(';').Select(d =>
                {
                    var draw = new Draw();

                    var redMatch = Regex.Match(d, @"(\d+) red");

                    if (redMatch.Success)
                    {
                        draw.Reds = int.Parse(redMatch.Groups[1].Value);
                    }

                    var greenMatch = Regex.Match(d, @"(\d+) green");

                    if (greenMatch.Success)
                    {
                        draw.Greens = int.Parse(greenMatch.Groups[1].Value);
                    }

                    var blueMatch = Regex.Match(d, @"(\d+) blue");

                    if (blueMatch.Success)
                    {
                        draw.Blues = int.Parse(blueMatch.Groups[1].Value);
                    }

                    return draw;
                }).ToList()
            };
        });

        //foreach (var game in games)
        //{
        //    Console.Write($"Game {game.Id}: ");

        //    foreach (var draw in game.Draws)
        //    {
        //        Console.Write($"{draw.Reds} reds, {draw.Greens} greens, {draw.Blues} blues | ");
        //    }

        //    Console.WriteLine();
        //}

        // Part 1
        var sum = 0;

        foreach (var game in games)
        {
            if (game.Draws.All(d => d.Reds <= 12 && d.Greens <= 13 && d.Blues <= 14))
            {
                sum += game.Id;
            }
        }

        Console.WriteLine($"Sum: {sum}");

        // Part 2
        var power = 0L;

        foreach (var game in games)
        {
            var minReds = game.Draws.Select(d => d.Reds).Max();
            var minGreens = game.Draws.Select(d => d.Greens).Max();
            var minBlues = game.Draws.Select(d => d.Blues).Max();

            power += minReds * minGreens * minBlues;
        }

        Console.WriteLine($"Power: {power}");
    }

    class Game
    {
        public int Id { get; set; }
        public List<Draw> Draws { get; set; } = new();
    }

    class Draw
    {
        public int Reds { get; set; }
        public int Greens { get; set; }
        public int Blues { get; set; }
    }
}
