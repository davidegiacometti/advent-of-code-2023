using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day12 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            using var sr = new StreamReader(InputFilePath);
            string? line;
            var springs = new List<char[]>();
            var damaged = new List<int[]>();

            while ((line = sr.ReadLine()) != null)
            {
                var splittedLine = line.Split(' ');
                springs.Add(splittedLine[0].ToCharArray());
                damaged.Add(splittedLine[1].Split(',').Select(int.Parse).ToArray());
            }

            var arrangements = 0L;

            Parallel.For(0, springs.Count, i =>
            {
                Interlocked.Add(ref arrangements, GetArrangements(springs[i], damaged[i]));
            });

            return new ValueTask<string>(arrangements.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            using var sr = new StreamReader(InputFilePath);
            string? line;
            var springs = new List<char[]>();
            var damaged = new List<int[]>();

            while ((line = sr.ReadLine()) != null)
            {
                var splittedLine = line.Split(' ');
                springs.Add(string.Join(',', Enumerable.Repeat(splittedLine[0], 5).ToArray()).ToCharArray());
                var d = splittedLine[1].Split(',').Select(int.Parse);
                damaged.Add(Enumerable.Repeat(d, 5).SelectMany(d => d).ToArray());
            }

            /*Parallel.For(0, springs.Count, i =>
            {
                GetArrangements(springs[i], damaged[i]);
            });*/

            return new ValueTask<string>(string.Empty);
        }

        private static long GetArrangements(char[] springs, int[] damaged)
        {
            var valid = 0L;
            var arrangements = new List<string>();
            GenerateCombinations(springs, 0, arrangements);

            foreach (var arrangement in arrangements)
            {
                var arrangementsGroups = arrangement.Split('.', StringSplitOptions.RemoveEmptyEntries);
                if (arrangementsGroups.Length != damaged.Length)
                {
                    continue;
                }

                var groupsMatch = true;
                for (var i = 0; i < damaged.Length; i++)
                {
                    if (arrangementsGroups[i].Count(c => c == '#') != damaged[i])
                    {
                        groupsMatch = false;
                        break;
                    }
                }

                if (groupsMatch)
                {
                    valid++;
                }
            }

            return valid;
        }

        private static void GenerateCombinations(char[] current, int index, List<string> arrangements)
        {
            if (index == current.Length)
            {
                arrangements.Add(new string(current));
                return;
            }

            if (current[index] == '?')
            {
                current[index] = '.';
                GenerateCombinations(current, index + 1, arrangements);
                current[index] = '#';
                GenerateCombinations(current, index + 1, arrangements);
                current[index] = '?'; // Set back the original value
            }
            else
            {
                GenerateCombinations(current, index + 1, arrangements);
            }
        }
    }
}
