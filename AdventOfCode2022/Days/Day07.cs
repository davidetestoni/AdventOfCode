using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days
{
    internal class Day07 : IDay
    {
        private static readonly long MAX_DIR_SIZE = 100000;
        private static readonly long TOTAL_DISK_SPACE = 70000000;
        private static readonly long MIN_DISK_SPACE = 30000000;

        public void Run()
        {
            var lines = File.ReadAllLines("Days/Day07.txt");
            var root = new Folder(parent: null);
            var current = root;

            foreach (var line in lines)
            {
                if (line.StartsWith("$ cd "))
                {
                    var destFolder = Regex.Match(line, @"^\$ cd (.*)$")
                        .Groups[1].Value;
                    
                    if (destFolder == "/")
                    {
                        current = root;
                    }
                    else if (destFolder == "..")
                    {
                        current = current.Parent ?? current;
                    }
                    else
                    {
                        current = current.GetSubFolder(destFolder);
                    }
                }
                else if (line == "$ ls")
                {
                    // We don't care since the only commands are cd and ls,
                    // so we handle cd and we disregard ls while considering
                    // all other lines as outputs of the ls in the current folder
                    continue;
                }
                else if (line.StartsWith("dir "))
                {
                    var subFolder = Regex.Match(line, @"^dir (.*)$")
                        .Groups[1].Value;

                    current.GetSubFolder(subFolder);
                }
                else
                {
                    var match = Regex.Match(line, @"^(\d+) (.*)$");
                    var fileSize = long.Parse(match.Groups[1].Value);
                    var fileName = match.Groups[2].Value;
                    current.FileSizes[fileName] = fileSize;
                }
            }

            // Init the list with the capacity we need in order to
            // avoid expensive array expansion operations
            var folderSizes = new List<long>(root.SubFolderCount);
            GatherFolderSizes(root, folderSizes);

            var sumOfSizes = folderSizes.Where(x => x <= MAX_DIR_SIZE).Sum();

            Console.WriteLine(sumOfSizes);

            var usedSpace = root.Size;
            var freeSpace = TOTAL_DISK_SPACE - usedSpace;
            var requiredSpace = MIN_DISK_SPACE - freeSpace;

            folderSizes.Sort();
            var minSufficientSize = folderSizes.First(x => x > requiredSpace);

            Console.WriteLine(minSufficientSize);
        }

        private static void GatherFolderSizes(Folder current,
            List<long> folderSizes)
        {
            folderSizes.Add(current.Size);

            foreach (var subFolder in current.SubFolders.Values)
            {
                GatherFolderSizes(subFolder, folderSizes);
            }
        }

        private class Folder
        {
            public Folder? Parent { get; }

            public Dictionary<string, Folder> SubFolders { get; } = new();

            public Dictionary<string, long> FileSizes { get; } = new();

            // Cached size
            private long? _size = null;

            // Lazily computes the size
            public long Size
            {
                get
                {
                    if (!_size.HasValue)
                    {
                        _size = FileSizes.Values.Sum() + SubFolders
                            .Values.Select(x => x.Size).Sum();
                    }

                    return _size.Value;
                }
            }

            public int SubFolderCount
                => SubFolders.Count + SubFolders
                .Values.Select(x => x.SubFolderCount).Sum();

            public Folder GetSubFolder(string name)
            {
                if (!SubFolders.ContainsKey(name))
                {
                    SubFolders[name] = new Folder(this);
                }

                return SubFolders[name];
            }

            public Folder(Folder? parent)
            {
                Parent = parent;
            }
        }
    }
}
