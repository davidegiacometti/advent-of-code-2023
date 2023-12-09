using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode.Days
{
    public partial class Day02 : BaseDay
    {
        private const int RedCubes = 12;
        private const int GreenCubes = 13;
        private const int BlueCubes = 14;

        public override ValueTask<string> Solve_1()
        {
            using var sr = new StreamReader(InputFilePath);
            var result = 0;
            string? line;

            while ((line = sr.ReadLine()) != null)
            {
                var gameIndex = GetPossibleGameIndex(line);
                if (gameIndex != null)
                {
                    result += gameIndex.Value;
                }
            }

            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            using var sr = new StreamReader(InputFilePath);
            var result = 0;
            string? line;

            while ((line = sr.ReadLine()) != null)
            {
                result += GetPowerOfMinimum(line);
            }

            return new ValueTask<string>(result.ToString());
        }

        [GeneratedRegex(@"(\d{1,2}) (red|green|blue)")]
        private static partial Regex SubsetRegex();

        private int? GetPossibleGameIndex(string line)
        {
            var colonIndex = line.IndexOf(':');

            // 4 is the length of "Game "
            var gameIndexStr = line[4..colonIndex];
            if (int.TryParse(gameIndexStr, out int gameIndex))
            {
                var sets = line[(colonIndex + 1)..].Split(';');
                foreach (var set in sets)
                {
                    var redCount = 0;
                    var greenCount = 0;
                    var blueCount = 0;

                    var setColors = set.Split(',');
                    foreach (var setColor in setColors)
                    {
                        var match = SubsetRegex().Match(setColor);
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int count))
                        {
                            switch (match.Groups[2].Value)
                            {
                                case "red":
                                    redCount += count;
                                    break;
                                case "green":
                                    greenCount += count;
                                    break;
                                case "blue":
                                    blueCount += count;
                                    break;
                            }
                        }
                    }

                    if (redCount > RedCubes || greenCount > GreenCubes || blueCount > BlueCubes)
                    {
                        return null;
                    }
                }

                return gameIndex;
            }

            return null;
        }

        private int GetPowerOfMinimum(string line)
        {
            var colonIndex = line.IndexOf(':');

            var redMax = 0;
            var greenMax = 0;
            var blueMax = 0;

            var sets = line[(colonIndex + 1)..].Split(';');
            foreach (var set in sets)
            {
                var setColors = set.Split(',');
                foreach (var setColor in setColors)
                {
                    var match = SubsetRegex().Match(setColor);
                    if (match.Success && int.TryParse(match.Groups[1].Value, out int count))
                    {
                        switch (match.Groups[2].Value)
                        {
                            case "red":
                                if (count > redMax)
                                {
                                    redMax = count;
                                }

                                break;
                            case "green":
                                if (count > greenMax)
                                {
                                    greenMax = count;
                                }

                                break;
                            case "blue":
                                if (count > blueMax)
                                {
                                    blueMax = count;
                                }

                                break;
                        }
                    }
                }
            }

            return redMax * greenMax * blueMax;
        }
    }
}
