using System.Drawing;
using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day18 : BaseDay
    {
        private static readonly char[] _colorTrimChars = new char[] { '(', ')' };

        public enum Direction
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3,
        }

        public override ValueTask<string> Solve_1()
        {
            var lines = File.ReadAllLines(InputFilePath);
            var digPlan = new Dig[lines.Length];

            for (var i = 0; i < lines.Length; i++)
            {
                var splittedLine = lines[i].Split(' ');
                Dig dig = default;

                switch (splittedLine[0][0])
                {
                    case 'U':
                        dig.Direction = Direction.Up;
                        break;
                    case 'R':
                        dig.Direction = Direction.Right;
                        break;
                    case 'D':
                        dig.Direction = Direction.Down;
                        break;
                    case 'L':
                        dig.Direction = Direction.Left;
                        break;
                }

                dig.Length = int.Parse(splittedLine[1]);
                dig.Color = ColorTranslator.FromHtml(splittedLine[2].Trim(_colorTrimChars));
                digPlan[i] = dig;
            }

            var positions = new List<Position> { new(0, 0) };

            for (var i = 0; i < digPlan.Length; i++)
            {
                MakeDig(digPlan[i], positions);
            }

            positions.RemoveAt(0); // (0, 0) is present twice

            // Positions can go negative, so we need to shift
            var rowMin = Math.Abs(positions.Min(d => d.Row));
            var colMin = Math.Abs(positions.Min(d => d.Col));

            for (var i = 0; i < positions.Count; i++)
            {
                var row = positions[i].Row + rowMin;
                var col = positions[i].Col + colMin;
                positions[i] = new Position(row, col);
            }

            var rowMax = positions.Max(d => d.Row);
            var colMax = positions.Max(d => d.Col);

            var notDigged = new List<Position>();

            for (var i = 0; i <= rowMax; i++)
            {
                for (var j = 0; j <= colMax; j++)
                {
                    if (!positions.Any(p => p.Row == i && p.Col == j))
                    {
                        notDigged.Add(new Position(i, j));
                    }
                }
            }

            var internalArea = new HashSet<Position>();
            while (notDigged.Count > 0)
            {
                var area = new HashSet<Position>();
                var floodQueue = new Queue<Position>();
                floodQueue.Enqueue(notDigged.First());

                while (floodQueue.Count > 0)
                {
                    var position = floodQueue.Dequeue();
                    var adjacentPositions = GetAdjacentPositions(position).Where(notDigged.Contains);

                    foreach (var p in adjacentPositions)
                    {
                        floodQueue.Enqueue(p);
                        notDigged.Remove(p);
                    }

                    area.Add(position);
                }

                if (area.Any(a => a.Row == 0 || a.Col == 0 || a.Row == rowMax || a.Col == colMax))
                {
                    continue;
                }

                internalArea = new HashSet<Position>(area);
            }

            // Print(positions, internalArea);
            return new ValueTask<string>((positions.Count + internalArea.Count).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new ValueTask<string>(string.Empty);
        }

        private static void MakeDig(Dig dig, List<Position> positions)
        {
            for (var i = 0; i < dig.Length; i++)
            {
                var lastPosition = positions.Last();
                switch (dig.Direction)
                {
                    case Direction.Up:
                        positions.Add(new Position(lastPosition.Row - 1, lastPosition.Col));
                        break;
                    case Direction.Right:
                        positions.Add(new Position(lastPosition.Row, lastPosition.Col + 1));
                        break;
                    case Direction.Down:
                        positions.Add(new Position(lastPosition.Row + 1, lastPosition.Col));
                        break;
                    case Direction.Left:
                        positions.Add(new Position(lastPosition.Row, lastPosition.Col - 1));
                        break;
                }
            }
        }

        private static void Print(List<Position> positions, HashSet<Position> area)
        {
            var rowMax = positions.Max(d => d.Row);
            var colMax = positions.Max(d => d.Col);
            var excavation = new char[rowMax + 1][];

            for (var i = 0; i <= rowMax; i++)
            {
                excavation[i] = new char[colMax + 1];
                for (var j = 0; j <= colMax; j++)
                {
                    if (positions.Any(p => p.Row == i && p.Col == j))
                    {
                        excavation[i][j] = '#';
                    }
                    else
                    {
                        excavation[i][j] = '.';
                    }
                }
            }

            for (var i = 0; i < excavation.Length; i++)
            {
                for (var j = 0; j < excavation[i].Length; j++)
                {
                    if (area.Any(p => p.Row == i && p.Col == j))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('.');
                        Console.ResetColor();
                    }
                    else if (positions.Any(p => p.Row == i && p.Col == j))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(excavation[i][j]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(excavation[i][j]);
                    }
                }

                Console.WriteLine();
            }
        }

        private static IEnumerable<Position> GetAdjacentPositions(Position pos)
        {
            var cols = Enumerable.Range(pos.Col - 1, 3).ToArray();
            return new[] { new Position(pos.Row, cols[0]), new Position(pos.Row, cols[^1]) } // Same row
                .Concat(cols.Select(col => new Position(pos.Row - 1, col))) // Previous row
                .Concat(cols.Select(col => new Position(pos.Row + 1, col))); // Next row
        }

        public struct Dig
        {
            public Direction Direction { get; set; }

            public int Length { get; set; }

            public Color Color { get; set; }

            public readonly override string ToString() => $"{Direction} {Length} ({Color})";
        }

        public struct Position(int row, int col)
        {
            public int Row { get; set; } = row;

            public int Col { get; set; } = col;

            public override readonly string ToString() => $"({Row}, {Col})";
        }
    }
}
