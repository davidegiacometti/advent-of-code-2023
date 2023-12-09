using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day06 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            var result = Solve(false);
            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var result = Solve(true);
            return new ValueTask<string>(result.ToString());
        }

        private IEnumerable<(long Ms, long Mm)> GetRaces(bool parseSingleRace)
        {
            var lines = File.ReadAllLines(InputFilePath);

            if (parseSingleRace)
            {
                var times = lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1).ToArray();
                var distances = lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1).ToArray();
                var time = times[0].Replace(" ", string.Empty);
                var distance = distances[0].Replace(" ", string.Empty);

                if (long.TryParse(time, out var ms) && long.TryParse(distance, out var mm))
                {
                    yield return (ms, mm);
                }
            }
            else
            {
                var times = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1).ToArray();
                var distances = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1).ToArray();

                for (var i = 0; i < times.Length; i++)
                {
                    if (int.TryParse(times[i], out var ms) && int.TryParse(distances[i], out var mm))
                    {
                        yield return (ms, mm);
                    }
                }
            }
        }

        private int Solve(bool parseSingleRace)
        {
            var races = GetRaces(parseSingleRace);
            var result = 0;

            foreach (var (ms, mm) in races)
            {
                var win = 0;
                for (var i = 1; i < ms; i++)
                {
                    var travel = (ms - i) * i;
                    if (travel > mm)
                    {
                        win++;
                    }
                }

                result = result == 0 ? win : result *= win;
            }

            return result;
        }
    }
}
