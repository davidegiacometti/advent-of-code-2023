using System.Text;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day03 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            using var sr = new StreamReader(InputFilePath);

            var numbers = new List<Number>();
            var symbols = new List<Position>();

            string? line;
            var numberBuilder = new StringBuilder();
            var y = 0;
            while ((line = sr.ReadLine()) != null)
            {
                var pattern = @"\d+";
                var matches = Regex.Matches(line, pattern);
                foreach (var match in matches.Cast<Match>())
                {
                    var number = new Number
                    {
                        Value = match.Value,
                        Start = new Position { X = match.Index, Y = y },
                        End = new Position { X = match.Index + match.Length - 1, Y = y },
                    };

                    numbers.Add(number);
                }

                for (var x = 0; x < line.Length; x++)
                {
                    var charValue = line[x];
                    var intValue = charValue - '0';
                    if (charValue != '.' && (intValue < 0 || intValue > 9))
                    {
                        var symbol = new Position
                        {
                            X = x,
                            Y = y,
                        };

                        symbols.Add(symbol);
                    }
                }

                y++;
            }

            var result = 0;
            foreach (var number in numbers)
            {
                foreach (var c in GetAdjacentCoordinates(number.Value, number.Start.X, number.Start.Y))
                {
                    if (symbols.Contains(c) && int.TryParse(number.Value, out var value))
                    {
                        result += value;
                        break;
                    }
                }
            }

            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            using var sr = new StreamReader(InputFilePath);

            var numbers = new List<Number>();
            var gears = new List<Position>();

            string? line;
            var numberBuilder = new StringBuilder();
            var y = 0;
            while ((line = sr.ReadLine()) != null)
            {
                var pattern = @"\d+";
                var matches = Regex.Matches(line, pattern);
                foreach (var match in matches.Cast<Match>())
                {
                    var number = new Number
                    {
                        Value = match.Value,
                        Start = new Position { X = match.Index, Y = y },
                        End = new Position { X = match.Index + match.Length - 1, Y = y },
                    };

                    numbers.Add(number);
                }

                var gearPattern = @"\*";
                var gearMatches = Regex.Matches(line, gearPattern);
                foreach (var match in gearMatches.Cast<Match>())
                {
                    var gear = new Position
                    {
                        X = match.Index,
                        Y = y,
                    };

                    gears.Add(gear);
                }

                y++;
            }

            var result = 0;
            foreach (var g in gears)
            {
                Number? adjacentNumber = null;
                var gearRatio = 0;

                foreach (var c in GetAdjacentCoordinates("*", g.X, g.Y))
                {
                    var number = numbers.Where(n => n.Start.Y == c.Y
                        && n.Start.X <= c.X
                        && n.End.X >= c.X
                        && (adjacentNumber == null || !n.Equals(adjacentNumber.Value))).ToArray(); // This is bad!

                    if (number.Length == 1)
                    {
                        if (gearRatio == 0)
                        {
                            adjacentNumber = number[0];
                            gearRatio = int.Parse(number[0].Value);
                        }
                        else
                        {
                            gearRatio *= int.Parse(number[0].Value);
                            result += gearRatio;
                            break;
                        }
                    }
                }
            }

            return new ValueTask<string>(result.ToString());
        }

        private static IEnumerable<Position> GetAdjacentCoordinates(string value, int x, int y)
        {
            var xCoordinates = Enumerable.Range(x - 1, value.Length + 2).ToArray();
            var adjacentCoordinates = new[] { new Position(xCoordinates[0], y), new Position(xCoordinates[^1], y) } // Same line
                .Concat(xCoordinates.Select(x => new Position(x, y - 1))) // Previous line
                .Concat(xCoordinates.Select(x => new Position(x, y + 1))); // Next line

            return adjacentCoordinates;
        }

        public struct Number
        {
            public string Value { get; set; }

            public Position Start { get; set; }

            public Position End { get; set; }
        }

        public struct Position(int x, int y)
        {
            public int X { get; set; } = x;

            public int Y { get; set; } = y;
        }
    }
}
