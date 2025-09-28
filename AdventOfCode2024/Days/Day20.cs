using AdventOfCode.Utils;
using Dijkstra.NET.Graph.Simple;
using Dijkstra.NET.ShortestPath;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day20(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day20.txt");

    // These algorithms are quite slow (around 1 minute) since they have
    // an O(N^2) time complexity, but they work.
    
    // ASSUMPTION
    // By looking at the input data, there is no such situation:
    // ###.# <- coming from here
    // ##.## <- cutting through here (turning left) would give an advantage
    // ...## <- shorter path never possible before
    // A.k.a., there are no closed-off sections of the maze that would
    // only be accessible by cutting through a wall and turning.
    
    [Fact]
    public void Part1()
    {
        // For the test input, it's 1. Otherwise, it's 100.
        const int minGain = 100;
        
        var matrix = _lines
            .Select(l => l.ToArray())
            .ToArray();

        var start = matrix.Cells().First(c => c.Value == 'S');
        var end = matrix.Cells().First(c => c.Value == 'E');
        
        // Baseline
        var graph = new Graph();
        var nodes = PopulateGraph(matrix, graph);
        var reverseNodes = nodes.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        var shortestPathResult = graph.Dijkstra(nodes[start], nodes[end]);
        
        HashSet<(Cell<char>, Cell<char>)> checkedPairs = [];
        var cheats = 0;
        
        var pathCells = shortestPathResult
            .GetPath()
            .Select(c => reverseNodes[c])
            .ToList();
        
        // O(N^2) time complexity, meh
        for (var i = 0; i < pathCells.Count; i++)
        {
            for (var j = 0; j < pathCells.Count; j++)
            {
                var cellA = pathCells[i];
                var cellB = pathCells[j];
                
                // If it's the same cell, skip it
                if (cellA == cellB)
                {
                    continue;
                }
                
                // If we've already checked this pair (in any order), skip it
                if (checkedPairs.Contains((cellA, cellB)) || checkedPairs.Contains((cellB, cellA)))
                {
                    continue;
                }
                
                checkedPairs.Add((cellA, cellB));
                
                var alignedHorizontally = cellA.Y == cellB.Y;
                var alignedVertically = cellA.X == cellB.X;
                
                // If the cells are not aligned, skip it
                if (!alignedHorizontally && !alignedVertically)
                {
                    continue;
                }
                
                var distance = alignedHorizontally
                    ? Math.Abs(cellA.X - cellB.X)
                    : Math.Abs(cellA.Y - cellB.Y);
                
                // If they aren't 2 cells apart, skip them
                if (distance != 2)
                {
                    continue;
                }
                
                // If they don't have a wall between them, skip it
                if (alignedHorizontally)
                {
                    var leftCell = cellA.X < cellB.X ? cellA : cellB;
                    
                    if (matrix.At(leftCell.Y, leftCell.X + 1) != '#')
                    {
                        continue;
                    }
                }
                else
                {
                    var topCell = cellA.Y < cellB.Y ? cellA : cellB;
                    
                    if (matrix.At(topCell.Y + 1, topCell.X) != '#')
                    {
                        continue;
                    }
                }
                
                // Now we know that we can connect these two cells by
                // cheating through the wall between them, so we can
                // calculate how many steps we save by cheating
                var gain = Math.Abs(i - j) - 2; // Exclude the start and end cells
                
                if (gain >= minGain)
                {
                    cheats++;
                }
            }
        }
        
        output.WriteLine(cheats.ToString());
        
        Assert.Equal(1387, cheats);
    }

    [Fact]
    public void Part2()
    {
        // For the test input, it's 50. Otherwise, it's 100.
        const int minGain = 100;
        
        var matrix = _lines
            .Select(l => l.ToArray())
            .ToArray();

        var start = matrix.Cells().First(c => c.Value == 'S');
        var end = matrix.Cells().First(c => c.Value == 'E');
        
        // Baseline
        var graph = new Graph();
        var nodes = PopulateGraph(matrix, graph);
        var reverseNodes = nodes.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        var shortestPathResult = graph.Dijkstra(nodes[start], nodes[end]);
        
        HashSet<(Cell<char>, Cell<char>)> checkedPairs = [];
        Dictionary<int, int> cheated = [];
        var cheats = 0;
        
        var pathCells = shortestPathResult
            .GetPath()
            .Select(c => reverseNodes[c])
            .ToList();
        
        // O(N^2) time complexity, meh
        for (var i = 0; i < pathCells.Count; i++)
        {
            for (var j = 0; j < pathCells.Count; j++)
            {
                var cellA = pathCells[i];
                var cellB = pathCells[j];
                
                // If it's the same cell, skip it
                if (cellA == cellB)
                {
                    continue;
                }
                
                // If we've already checked this pair (in any order), skip it
                if (checkedPairs.Contains((cellA, cellB)) || checkedPairs.Contains((cellB, cellA)))
                {
                    continue;
                }
                
                checkedPairs.Add((cellA, cellB));
                
                // Count the least number of steps to walk from one cell
                // of the matrix to the other (without considering the walls)
                // We need the Manhattan distance
                var distance = Math.Abs(cellA.X - cellB.X) + Math.Abs(cellA.Y - cellB.Y);
                
                // If it's more than 20 steps, skip it
                if (distance > 20)
                {
                    continue;
                }
                
                // Now we know that we can connect these two cells by
                // cheating through any walls between them, so we can
                // calculate how many steps we save by cheating
                var gain = Math.Abs(i - j) - distance;
                
                if (gain >= minGain)
                {
                    cheats++;
                    cheated[gain] = cheated.GetValueOrDefault(gain) + 1;
                }
            }
        }
        
        output.WriteLine(cheats.ToString());
    }
    
    private static Dictionary<Cell<char>, uint> PopulateGraph(
        char[][] matrix, Graph graph)
    {
        var walkableCells = matrix.Cells()
            .Where(c => c.Value != '#')
            .ToList();
        
        Dictionary<Cell<char>, uint> nodes = [];
        
        // Fill the graph with nodes
        foreach (var cell in walkableCells)
        {
            nodes[cell] = graph.AddNode();
        }

        foreach (var cell in walkableCells)
        {
            var validNeighbors = matrix.Neighbors(cell, diagonal: false)
                .Where(c => c.Value != '#')
                .ToList();
            
            // Otherwise, connect this cell to its neighbors with a weight of 1
            foreach (var neighbor in validNeighbors)
            {
                graph.Connect(
                    nodes[cell],
                    nodes[neighbor],
                    1);
            }
        }

        return nodes;
    }
}
