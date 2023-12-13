using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day13 : BaseDay
    {
        public override ValueTask<string> Solve_1()
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

            var rowsCount = 0L;
            var colsCount = 0L;
            foreach (var p in patterns)
            {
                var rows = GetReflectedRows(p);
                if (rows != -1)
                {
                    rowsCount += rows;
                }

                var cols = GetReflectedColumns(p);
                if (cols != -1)
                {
                    colsCount += cols;
                }
            }

            var result = colsCount + (100 * rowsCount);
            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new ValueTask<string>(string.Empty);
        }

        private static int GetReflectedRows(char[][] pattern)
        {
            for (var row = 1; row < pattern.Length; row++)
            {
                var prev = row - 1;
                var current = row;
                var isReflected = true;
                while (prev >= 0 && current < pattern.Length)
                {
                    if (!pattern[prev].SequenceEqual(pattern[current]))
                    {
                        isReflected = false;
                        break;
                    }

                    prev--;
                    current++;
                }

                if (isReflected)
                {
                    return row;
                }
            }

            return -1;
        }

        private static int GetReflectedColumns(char[][] pattern)
        {
            for (var cols = 1; cols < pattern[0].Length; cols++)
            {
                var prev = cols - 1;
                var current = cols;
                var isReflected = true;
                while (prev >= 0 && current < pattern[0].Length)
                {
                    if (!GetColumn(pattern, prev).SequenceEqual(GetColumn(pattern, current)))
                    {
                        isReflected = false;
                        break;
                    }

                    prev--;
                    current++;
                }

                if (isReflected)
                {
                    return cols;
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
    }
}
