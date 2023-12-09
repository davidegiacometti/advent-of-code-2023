using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day09 : BaseDay
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

        private long Solve(Part part)
        {
            var lines = File.ReadAllLines(InputFilePath);
            var sequences = lines.Select(l => l.Split(' ').Select(l => long.Parse(l)).ToArray()).ToArray();
            var result = 0L;

            foreach (var sequence in sequences)
            {
                var expandedSequence = new List<long[]>
                {
                    sequence,
                };

                var currentSubSequence = expandedSequence.Last();

                while (!currentSubSequence.All(s => s == 0))
                {
                    var nextSequence = new List<long>(currentSubSequence.Length - 1);

                    for (var i = currentSubSequence.Length - 1; i > 0; i--)
                    {
                        nextSequence.Insert(0, currentSubSequence[i] - currentSubSequence[i - 1]);
                    }

                    expandedSequence.Add(nextSequence.ToArray());
                    currentSubSequence = expandedSequence.Last();
                }

                var predicted = 0L;

                // Skip the the sequence filled with zeros
                for (var i = expandedSequence.Count - 2; i >= 0; i--)
                {
                    predicted = part == Part.Part1
                        ? expandedSequence[i].Last() + predicted
                        : expandedSequence[i].First() - predicted;
                }

                result += predicted;
            }

            return result;
        }
    }
}
