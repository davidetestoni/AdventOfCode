using AdventOfCode.Utils;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day8(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day8.txt");

    [Fact]
    public void Part1()
    {
        var matrix = _lines.Select(l => l.ToCharArray()).ToArray();

        var frequencies = matrix
            .Cells()
            .Where(c => c.Value != '.')
            .GroupBy(c => c.Value)
            .Select(g => g.ToArray());
        
        var antiNodes = new HashSet<(int y, int x)>();

        foreach (var antennas in frequencies)
        {
            foreach (var pair in antennas.Combinations(2))
            {
                var pairList = pair.ToList();
                
                var diffX = Math.Abs(pairList[0].X - pairList[1].X);
                var diffY = Math.Abs(pairList[0].Y - pairList[1].Y);
             
                // We assume antennas are never in the same row/column, but
                // always form a diagonal line
                
                // Find the top and bottom antennas
                var top = pairList[0].Y < pairList[1].Y ? pairList[0] : pairList[1];
                var bottom = pairList[0].Y < pairList[1].Y ? pairList[1] : pairList[0];
                
                // If the antenna at the top is more shifted to the right
                if (top.X > bottom.X)
                {
                    var antiNode1 = (top.Y - diffY, top.X + diffX);
                    var antiNode2 = (bottom.Y + diffY, bottom.X - diffX);
                    
                    if (matrix.IsWithinBounds(antiNode1.Item1, antiNode1.Item2))
                    {
                        antiNodes.Add(antiNode1);
                    }
                    
                    if (matrix.IsWithinBounds(antiNode2.Item1, antiNode2.Item2))
                    {
                        antiNodes.Add(antiNode2);
                    }
                }
                // If the antenna at the bottom is more shifted to the right
                else
                {
                    var antiNode1 = (top.Y - diffY, top.X - diffX);
                    var antiNode2 = (bottom.Y + diffY, bottom.X + diffX);
                    
                    if (matrix.IsWithinBounds(antiNode1.Item1, antiNode1.Item2))
                    {
                        antiNodes.Add(antiNode1);
                    }
                    
                    if (matrix.IsWithinBounds(antiNode2.Item1, antiNode2.Item2))
                    {
                        antiNodes.Add(antiNode2);
                    }
                }
            }
        }

        var total = antiNodes.Count;
        
        output.WriteLine(total.ToString());
        
        Assert.Equal(291, total);
    }

    [Fact]
    public void Part2()
    {
        var matrix = _lines.Select(l => l.ToCharArray()).ToArray();

        var frequencies = matrix
            .Cells()
            .Where(c => c.Value != '.')
            .GroupBy(c => c.Value)
            .Select(g => g.ToArray());
        
        var antiNodes = new HashSet<(int y, int x)>();

        foreach (var antennas in frequencies)
        {
            // If there are at least 2 antennas of this frequency, add one anti-node
            // at the location of each antenna
            if (antennas.Length >= 2)
            {
                foreach (var antenna in antennas)
                {
                    antiNodes.Add((antenna.Y, antenna.X));
                }
            }
            
            foreach (var pair in antennas.Combinations(2))
            {
                var pairList = pair.ToList();
                
                var diffX = Math.Abs(pairList[0].X - pairList[1].X);
                var diffY = Math.Abs(pairList[0].Y - pairList[1].Y);
             
                // We assume antennas are never in the same row/column, but
                // always form a diagonal line
                
                // Find the top and bottom antennas
                var top = pairList[0].Y < pairList[1].Y ? pairList[0] : pairList[1];
                var bottom = pairList[0].Y < pairList[1].Y ? pairList[1] : pairList[0];
                
                // If the antenna at the top is more shifted to the right
                if (top.X > bottom.X)
                {
                    var multiplier = 1;

                    // As long as we can add anti-nodes, keep adding them
                    while (true)
                    {
                        var antiNode1 = (top.Y - diffY * multiplier, top.X + diffX * multiplier);
                        var antiNode2 = (bottom.Y + diffY * multiplier, bottom.X - diffX * multiplier);
                        
                        // If we can't add any anti-nodes, break
                        if (!matrix.IsWithinBounds(antiNode1.Item1, antiNode1.Item2) &&
                            !matrix.IsWithinBounds(antiNode2.Item1, antiNode2.Item2))
                        {
                            break;
                        }
                        
                        if (matrix.IsWithinBounds(antiNode1.Item1, antiNode1.Item2))
                        {
                            antiNodes.Add(antiNode1);
                        }
                        
                        if (matrix.IsWithinBounds(antiNode2.Item1, antiNode2.Item2))
                        {
                            antiNodes.Add(antiNode2);
                        }
                        
                        multiplier++;
                    }
                }
                // If the antenna at the bottom is more shifted to the right
                else
                {
                    var multiplier = 1;
                    
                    while (true)
                    {
                        var antiNode1 = (top.Y - diffY * multiplier, top.X - diffX * multiplier);
                        var antiNode2 = (bottom.Y + diffY * multiplier, bottom.X + diffX * multiplier);
                        
                        if (!matrix.IsWithinBounds(antiNode1.Item1, antiNode1.Item2) &&
                            !matrix.IsWithinBounds(antiNode2.Item1, antiNode2.Item2))
                        {
                            break;
                        }
                        
                        if (matrix.IsWithinBounds(antiNode1.Item1, antiNode1.Item2))
                        {
                            antiNodes.Add(antiNode1);
                        }
                        
                        if (matrix.IsWithinBounds(antiNode2.Item1, antiNode2.Item2))
                        {
                            antiNodes.Add(antiNode2);
                        }
                        
                        multiplier++;
                    }
                }
            }
        }

        var total = antiNodes.Count;
        
        output.WriteLine(total.ToString());
    }
}
