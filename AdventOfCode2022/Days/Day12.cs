using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;

namespace AdventOfCode2022.Days
{
    internal class Day12 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day12.txt");
            var rows = lines.Length;
            var columns = lines[0].Length;
            var totalNodes = rows * columns;
            var graph = new Graph<int, string>();

            // Fill the graph with nodes
            for (int i = 1; i <= totalNodes; i++)
            {
                graph.AddNode(i);
            }

            var sourceIndex = 0;
            var destinationIndex = 0;

            // We need to build an adjacency matrix to use Dijkstra's algorithm
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    // Get the char representing the elevation
                    var c = lines[i][j];

                    // Find the index of the node in the target adjacency matrix
                    var nodeIndex = i * columns + j;

                    // Assign the source and destination indices
                    if (c == 'S')
                    {
                        sourceIndex = nodeIndex;
                    }

                    if (c == 'E')
                    {
                        destinationIndex = nodeIndex;
                    }

                    // Look at the node directly above, and store the link
                    if (i > 1)
                    {
                        RegisterLink(lines, graph, (i, j), (i - 1, j));
                    }

                    // Look at the node directly below, and store the link
                    if (i < rows - 1)
                    {
                        RegisterLink(lines, graph, (i, j), (i + 1, j));
                    }

                    // Look at the node directly to the left, and store the link
                    if (j > 1)
                    {
                        RegisterLink(lines, graph, (i, j), (i, j - 1));
                    }

                    // Look at the node directly to the right, and store the link
                    if (j < columns - 1)
                    {
                        RegisterLink(lines, graph, (i, j), (i, j + 1));
                    }
                }
            }

            var result = graph.Dijkstra(
                (uint)sourceIndex + 1, (uint)destinationIndex + 1);
            
            var path = result.GetPath();
            var steps = path.Count() - 1;

            Console.WriteLine(steps);

            // PART 2
            var lowestPoints = new List<int>();

            // Get the indices of all the 'a' points ('S' works too)
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var c = lines[i][j];
                    
                    if (c == 'a' || c == 'S')
                    {
                        lowestPoints.Add(i * columns + j);
                    }
                }
            }

            var minPathLength = int.MaxValue;

            foreach (var point in lowestPoints)
            {
                result = graph.Dijkstra(
                    (uint)point + 1, (uint)destinationIndex + 1);

                path = result.GetPath();
                var pathLength = path.Count() - 1;

                // If there is no possible path, the length will be 0 - 1
                if (pathLength == -1)
                {
                    continue;
                }

                minPathLength = Math.Min(minPathLength, pathLength);
            }

            Console.WriteLine(minPathLength);
        }

        private static void RegisterLink(string[] lines, Graph<int, string> graph,
            (int i, int j) n1, (int i, int j) n2)
        {
            // Get the two elevations
            var n1Elev = lines[n1.i][n1.j];
            var n2Elev = lines[n2.i][n2.j];

            // Find the node indices
            var n1Index = n1.i * lines[0].Length + n1.j;
            var n2Index = n2.i * lines[0].Length + n2.j;

            // Store the weight for the forward link
            var forwardWeight = GetLinkWeight(n1Elev, n2Elev);

            if (forwardWeight is not null)
            {
                graph.Connect((uint)n1Index + 1, (uint)n2Index + 1,
                    forwardWeight.Value, string.Empty);
            }


            // Store the weight of the reverse link
            var reverseWeight = GetLinkWeight(n2Elev, n1Elev);

            if (reverseWeight is not null)
            {
                graph.Connect((uint)n2Index + 1, (uint)n1Index + 1,
                    reverseWeight.Value, string.Empty);
            }
        }

        private static int? GetLinkWeight(char c, char neighbor)
        {
            // The start has elevation 'a', the end has elevation 'z'
            if (c == 'S') c = 'a';
            if (c == 'E') c = 'z';
            if (neighbor == 'S') neighbor = 'a';
            if (neighbor == 'E') neighbor = 'z';

            return (neighbor - c) switch
            {
                // Destination is higher by more than 1, no link
                > 1 => null,

                // Destination is higher by 1, it costs very little
                1 => 1,

                // If they are the same, it costs a bit more
                0 => 2,

                // If the neighbor is below, it costs way more
                < 0 => 2 + (c - neighbor)
            };
        }
    }
}
