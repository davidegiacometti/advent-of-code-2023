using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day15 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            var result = Solve(Part.Part1);
            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var result = Solve(Part.Part2);
            return new ValueTask<string>(result.ToString());
        }

        private static int ComputeHash(string step)
        {
            var lastComputed = 0;
            for (var i = 0; i < step.Length; i++)
            {
                lastComputed = ((lastComputed + step[i]) * 17) % 256;
            }

            return lastComputed;
        }

        private long Solve(Part part)
        {
            var steps = File.ReadAllLines(InputFilePath)[0].Split(',');
            var result = 0L;

            if (part == Part.Part1)
            {
                foreach (var step in steps)
                {
                    result += ComputeHash(step);
                }
            }
            else
            {
                var boxes = Enumerable.Range(0, 256).ToDictionary(k => k, k => new List<Lens>());

                foreach (var step in steps)
                {
                    var operation = step.Contains('-') ? '-' : '=';
                    var stepSplit = step.Split(operation, StringSplitOptions.RemoveEmptyEntries);

                    var boxKey = ComputeHash(stepSplit[0]);

                    if (!boxes.TryGetValue(boxKey, out var boxContent))
                    {
                        continue;
                    }

                    var name = stepSplit[0];
                    var focalLength = stepSplit.Length > 1 ? int.Parse(stepSplit[1]) : 0;
                    var lens = new Lens(name, focalLength);

                    if (operation == '-')
                    {
                        boxContent.Remove(new Lens(stepSplit[0], 0));
                    }
                    else
                    {
                        var index = boxContent.IndexOf(lens);
                        if (index != -1)
                        {
                            boxContent.RemoveAt(index);
                        }
                        else
                        {
                            index = boxContent.Count;
                        }

                        boxContent.Insert(index, lens);
                    }
                }

                for (var i = 0; i < boxes.Count; i++)
                {
                    if (!boxes.TryGetValue(i, out var content))
                    {
                        continue;
                    }

                    result += content.Select((lens, index) => (i + 1) * (index + 1) * lens.FocalLength).Sum();
                }
            }

            return result;
        }

        public struct Lens(string name, int focalLength)
        {
            public string Name { get; set; } = name;

            public int FocalLength { get; set; } = focalLength;

            public static bool operator ==(Lens left, Lens right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Lens left, Lens right)
            {
                return !(left == right);
            }

            public override readonly bool Equals(object? obj)
            {
                return obj is Lens lens &&
                       Name == lens.Name;
            }

            public override readonly int GetHashCode()
            {
                return HashCode.Combine(Name);
            }

            public override readonly string ToString()
            {
                return $"{Name} {FocalLength}";
            }
        }
    }
}
