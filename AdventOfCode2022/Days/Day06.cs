namespace AdventOfCode2022.Days
{
    internal class Day06 : IDay
    {
        public void Run()
        {
            var text = File.ReadAllText("Days/Day06.txt");
            
            Console.WriteLine(IndexOfMarker(text, 4));
            Console.WriteLine(IndexOfMarker(text, 14));
        }

        private static int IndexOfMarker(string text, int markerSize)
        {
            var capacity = markerSize;
            var queue = new Queue<char>(capacity);
            var index = 0;

            for (int i = 0; i < text.Length; i++)
            {
                // If the queue is full, dequeue
                if (queue.Count == capacity)
                {
                    queue.Dequeue();
                }

                queue.Enqueue(text[i]);

                // If we saw enough characters and they are all different
                if (queue.Count == capacity && queue.Distinct().Count() == capacity)
                {
                    index = i + 1;
                    break;
                }
            }

            return index;
        }
    }
}
