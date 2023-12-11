using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day11 : BaseDay
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

        private static bool IsColumEmpty(char[][] universe, int col)
        {
            for (var i = 0; i < universe.Length; i++)
            {
                if (universe[i][col] == '#')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsRowEmpty(char[][] universe, int row)
        {
            for (var i = 0; i < universe.Length; i++)
            {
                if (universe[row][i] == '#')
                {
                    return false;
                }
            }

            return true;
        }

        private static IEnumerable<((int Row, int Col) Galaxy1, (int Row, int Col) Galaxy2)> GetPairs(List<(int Row, int Col)> galaxies, int index)
        {
            var galaxy = galaxies[index];
            for (int i = 0; i < galaxies.Count; i++)
            {
                if (i != index)
                {
                    yield return (galaxy, galaxies[i]);
                }
            }
        }

        private static int CalculateManhattanDistance(int row1, int col1, int row2, int col2)
        {
            return Math.Abs(row2 - row1) + Math.Abs(col2 - col1);
        }

        private long Solve(Part part)
        {
            var lines = File.ReadAllLines(InputFilePath);
            var universe = new char[lines.Length][];
            for (var i = 0; i < lines.Length; i++)
            {
                universe[i] = new char[lines[i].Length];
                universe[i] = lines[i].ToCharArray();
            }

            var emptyRows = new List<int>();
            var emptyColumns = new List<int>();

            for (var i = 0; i < universe.Length; i++)
            {
                var galaxy = new List<char>();

                for (var j = 0; j < universe[i].Length; j++)
                {
                    if (i == 0)
                    {
                        if (IsColumEmpty(universe, j))
                        {
                            emptyColumns.Add(j);
                        }
                    }

                    galaxy.Add(universe[i][j]);
                }

                if (IsRowEmpty(universe, i))
                {
                    emptyRows.Add(i);
                }
            }

            var galaxies = new List<(int Row, int Col)>();
            for (var i = 0; i < universe.Length; i++)
            {
                for (var j = 0; j < universe[i].Length; j++)
                {
                    if (universe[i][j] == '#')
                    {
                        galaxies.Add((i, j));
                    }
                }
            }

            long result = 0;
            var scale = part == Part.Part1 ? 1 : 999999;
            for (var i = 0; i < galaxies.Count; i++)
            {
                var pairs = GetPairs(galaxies, i);
                foreach (var (galaxy1, galaxy2) in pairs)
                {
                    var distance = CalculateManhattanDistance(galaxy1.Row, galaxy1.Col, galaxy2.Row, galaxy2.Col);
                    if (galaxy1.Row > galaxy2.Row)
                    {
                        distance += emptyRows.Count(r => r > galaxy2.Row && r < galaxy1.Row) * scale;
                    }
                    else
                    {
                        distance += emptyRows.Count(r => r > galaxy1.Row && r < galaxy2.Row) * scale;
                    }

                    if (galaxy1.Col > galaxy2.Col)
                    {
                        distance += emptyColumns.Count(c => c > galaxy2.Col && c < galaxy1.Col) * scale;
                    }
                    else
                    {
                        distance += emptyColumns.Count(c => c > galaxy1.Col && c < galaxy2.Col) * scale;
                    }

                    result += distance;
                }
            }

            result /= 2;
            return result;
        }
    }
}
