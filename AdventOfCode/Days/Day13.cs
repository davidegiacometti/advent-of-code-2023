using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day13 : BaseDay
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

        private static int GetReflectedRows(char[][] pattern, int skip = -1)
        {
            for (var row = 1; row < pattern.Length; row++)
            {
                if (row == skip)
                {
                    continue;
                }

                var prev = row - 1;
                var current = row;
                var reflection = true;
                while (prev >= 0 && current < pattern.Length)
                {
                    if (!pattern[prev].SequenceEqual(pattern[current]))
                    {
                        reflection = false;
                        break;
                    }

                    prev--;
                    current++;
                }

                if (reflection)
                {
                    return row;
                }
            }

            return -1;
        }

        private static int GetReflectedColumns(char[][] pattern, int skip = -1)
        {
            for (var col = 1; col < pattern[0].Length; col++)
            {
                if (col == skip)
                {
                    continue;
                }

                var prev = col - 1;
                var current = col;
                var reflection = true;
                while (prev >= 0 && current < pattern[0].Length)
                {
                    if (!GetColumn(pattern, prev).SequenceEqual(GetColumn(pattern, current)))
                    {
                        reflection = false;
                        break;
                    }

                    prev--;
                    current++;
                }

                if (reflection)
                {
                    return col;
                }
            }

            return -1;
        }

        private static IEnumerable<char> GetColumn(char[][] pattern, int col)
        {
            for (var i = 0; i < pattern.Length; i++)
            {
                yield return pattern[i][col];
            }
        }

        private long Solve(Part part)
        {
            var lines = File.ReadAllLines(InputFilePath);
            var patterns = new List<char[][]>();

            var chunkIndexes = lines
                .Select((line, index) => (line, index))
                .Where(x => x.line.Length == 0).Select(x => x.index)
                .Prepend(0)
                .Append(lines.Length)
                .ToArray();

            for (var i = 0; i < chunkIndexes.Length - 1; i++)
            {
                var skipCount = chunkIndexes[i] > 0 ? chunkIndexes[i] + 1 : chunkIndexes[i];
                var takeCount = chunkIndexes[i + 1] - skipCount;
                var chunk = lines.Skip(skipCount).Take(takeCount).ToArray();
                patterns.Add(chunk.Select(c => c.ToCharArray()).ToArray());
            }

            var result = 0L;
            var rows = 0L;
            var cols = 0L;
            if (part == Part.Part1)
            {
                foreach (var p in patterns)
                {
                    var r = GetReflectedRows(p);
                    if (r != -1)
                    {
                        rows += r;
                    }

                    var c = GetReflectedColumns(p);
                    if (c != -1)
                    {
                        cols += c;
                    }
                }
            }
            else
            {
                foreach (var p in patterns)
                {
                    var smudgeFound = false;
                    var r = GetReflectedRows(p);
                    var c = GetReflectedColumns(p);

                    for (var i = 0; i < p.Length; i++)
                    {
                        if (smudgeFound)
                        {
                            break;
                        }

                        for (var j = 0; j < p[i].Length; j++)
                        {
                            if (smudgeFound)
                            {
                                break;
                            }

                            var prev = p[i][j];
                            p[i][j] = p[i][j] == '.' ? '#' : '.';

                            var newR = GetReflectedRows(p, r);
                            var newC = GetReflectedColumns(p, c);
                            var newRC = (newR, newC);
                            p[i][j] = prev;

                            if (newRC != (-1, -1) && newRC != (r, c))
                            {
                                smudgeFound = true;

                                if (newR != -1)
                                {
                                    rows += newR;
                                }
                                else if (newC != -1)
                                {
                                    cols += newC;
                                }
                            }
                        }
                    }
                }
            }

            result = cols + (100 * rows);
            return result;
        }
    }
}
