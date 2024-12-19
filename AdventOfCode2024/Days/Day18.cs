using AdventOfCode.Utils;
using Dijkstra.NET.Graph.Simple;
using Dijkstra.NET.ShortestPath;
using Xunit.Abstractions;

namespace AdventOfCode2024.Days;

public class Day18(ITestOutputHelper output)
{
    private readonly string[] _lines = File.ReadAllLines("Data/Day18.txt");

    [Fact]
    public void Fact1()
    {
        const int size = 71;
        const int bytes = 1024;
        
        var matrix = Enumerable
            .Range(0, size)
            .Select(_ => Enumerable.Repeat('.', size).ToArray())
            .ToArray();

        foreach (var line in _lines.Take(bytes))
        {
            var coordinates = line.Split(",");
            matrix[int.Parse(coordinates[1])][int.Parse(coordinates[0])] = '#';
        }

        var start = matrix.At(0, 0);
        var end = matrix.At(size - 1, size - 1);
        
        var graph = new Graph();
        var nodes = PopulateGraph(matrix, graph);
        
        var shortest = graph.Dijkstra(nodes[start], nodes[end]).Distance;

        output.WriteLine(shortest.ToString());
        
        Assert.Equal(506, shortest);
    }

    [Fact]
    public void Part2()
    {
        const int size = 71;
        const int bytes = 1024; // We know these bytes are safe
        
        var matrix = Enumerable
            .Range(0, size)
            .Select(_ => Enumerable.Repeat('.', size).ToArray())
            .ToArray();

        foreach (var line in _lines.Take(bytes))
        {
            var coordinates = line.Split(",");
            matrix[int.Parse(coordinates[1])][int.Parse(coordinates[0])] = '#';
        }

        var start = matrix.At(0, 0);
        var end = matrix.At(size - 1, size - 1);

        string? blockingLine = null;
        
        // Take the next bytes, one by one, update the matrix and recreate the graph
        foreach (var line in _lines.Skip(bytes))
        {
            var coordinates = line.Split(",");
            matrix[int.Parse(coordinates[1])][int.Parse(coordinates[0])] = '#';
            
            // The library doesn't allow us to remove links, so we need to
            // recreate the graph every time
            var graph = new Graph();
            var nodes = PopulateGraph(matrix, graph);
            
            var result = graph.Dijkstra(nodes[start], nodes[end]);

            if (result.IsFounded)
            {
                continue;
            }
            
            blockingLine = line;
            break;
        }
        
        output.WriteLine(blockingLine);
    }

    private static Dictionary<Cell<char>, uint> PopulateGraph(char[][] matrix, Graph graph)
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
