namespace AdventOfCode2021.Days
{
    internal class Day12 : IDay
    {
        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day12.txt");

            // Build the graph
            var nodes = new Dictionary<string, Node>();

            foreach (var line in lines)
            {
                // Get the two labels
                var pair = line.Split('-');
                var leftLabel = pair[0];
                var rightLabel = pair[1];

                // If the left node doesn't exist, create one
                var left = nodes.GetValueOrDefault(leftLabel);

                if (left is null)
                {
                    left = new Node(leftLabel);
                    nodes[leftLabel] = left;
                }

                // If the right node doesn't exist, create one
                var right = nodes.GetValueOrDefault(rightLabel);

                if (right is null)
                {
                    right = new Node(rightLabel);
                    nodes[rightLabel] = right;
                }

                // Link them if not already linked
                if (!left.Links.Any(l => l.Label == rightLabel))
                {
                    left.Links.Add(right);
                }

                if (!right.Links.Any(l => l.Label == leftLabel))
                {
                    right.Links.Add(left);
                }
            }

            // Find all paths
            var paths = FindPathsToEnd(new(), nodes["start"], 1);

            Console.WriteLine($"Paths: {paths.Count}");

            paths = FindPathsToEnd(new(), nodes["start"], 2);

            Console.WriteLine($"Paths with 2 visits: {paths.Count}");
        }

        private List<List<Node>> FindPathsToEnd(
            List<Node> path, Node node, int maxSmallCaveVisits)
        {
            // If this node is the end, add it and return the path
            if (node.Type is NodeType.End)
            {
                path.Add(node);
                return new() { path };
            }

            // If it's the start and it was already in the path,
            // break here.
            if (node.Type is NodeType.Start && path.Contains(node))
            {
                return new();
            }

            // If it's a small cave and it was already in the path
            if (node.Type is NodeType.SmallCave && path.Contains(node))
            {
                // maxSmallCaveVisits times, break here
                if (path.Where(n => n == node).Count() >= maxSmallCaveVisits)
                {
                    return new();
                }
                else
                {
                    maxSmallCaveVisits--;
                }
            }

            // Add the node to the path
            path.Add(node);

            // Get the path for all links and return the flattened list,
            // but make a copy of the path, otherwise multiple branches
            // edit the same list.
            return node.Links
                .SelectMany(l => FindPathsToEnd(path.ToList(), l, maxSmallCaveVisits))
                .ToList();
        }

        class Node
        {
            public string Label { get; set; }
            public NodeType Type { get; set; }
            public List<Node> Links { get; set; } = new();

            public Node(string label)
            {
                Label = label;

                if (label == "start")
                {
                    Type = NodeType.Start;
                }
                else if (label == "end")
                {
                    Type = NodeType.End;
                }
                else if (label.ToCharArray().All(c => char.IsUpper(c)))
                {
                    Type = NodeType.BigCave;
                }
                else
                {
                    Type = NodeType.SmallCave;
                }
            }
        }

        enum NodeType
        {
            Start,
            End,
            BigCave,
            SmallCave
        }
    }
}
