using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day16 : BaseDay
    {
        public enum Direction
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3,
        }

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

        public long Solve(Part part)
        {
            var lines = File.ReadAllLines(InputFilePath);
            var tiles = new char[lines.Length][];
            for (var i = 0; i < lines.Length; i++)
            {
                tiles[i] = lines[i].ToCharArray();
            }

            var compareLock = new object();
            var maxEnergized = 0L;

            (Position Position, Direction Direction)[] starts;
            if (part == Part.Part1)
            {
                starts = new[] { (new Position(0, 0), Direction.Right) };
            }
            else
            {
                starts = Enumerable.Range(0, tiles[0].Length).Select(i => (new Position(0, i), Direction.Down))
                   .Concat(Enumerable.Range(0, tiles[0].Length).Select(i => (new Position(tiles.Length - 1, i), Direction.Up)))
                   .Concat(Enumerable.Range(0, tiles.Length).Select(i => (new Position(i, 0), Direction.Right)))
                   .Concat(Enumerable.Range(0, tiles.Length).Select(i => (new Position(i, tiles.Length - 1), Direction.Left)))
                   .ToArray();
            }

            starts.AsParallel().ForAll(s =>
            {
                var energized = new bool[tiles.Length][];
                for (var i = 0; i < tiles.Length; i++)
                {
                    energized[i] = new bool[tiles[i].Length];
                    for (var j = 0; j < tiles[i].Length; j++)
                    {
                        energized[i][j] = false;
                    }
                }

                var traverseQueue = new Queue<(Position Position, Direction Direction)>();
                var traversed = new HashSet<(Position Position, Direction Direction)>();
                traverseQueue.Enqueue(s);

                while (traverseQueue.Count > 0)
                {
                    var (current, direction) = traverseQueue.Dequeue();

                    if (current.Row < 0 || current.Row >= tiles.Length || current.Col < 0 || current.Col >= tiles[current.Row].Length)
                    {
                        continue;
                    }

                    if (traversed.Any(p => p.Position.Equals(current) && p.Direction == direction))
                    {
                        continue;
                    }

                    traversed.Add((current, direction));
                    energized[current.Row][current.Col] = true;
                    var tile = tiles[current.Row][current.Col];

                    switch (tile)
                    {
                        case '.':
                            switch (direction)
                            {
                                case Direction.Up:
                                    current.Row--;
                                    break;
                                case Direction.Right:
                                    current.Col++;
                                    break;
                                case Direction.Down:
                                    current.Row++;
                                    break;
                                case Direction.Left:
                                    current.Col--;
                                    break;
                            }

                            traverseQueue.Enqueue((current, direction));
                            break;
                        case '/':
                            switch (direction)
                            {
                                case Direction.Up:
                                    current.Col++;
                                    direction = Direction.Right;
                                    break;
                                case Direction.Right:
                                    current.Row--;
                                    direction = Direction.Up;
                                    break;
                                case Direction.Down:
                                    current.Col--;
                                    direction = Direction.Left;
                                    break;
                                case Direction.Left:
                                    current.Row++;
                                    direction = Direction.Down;
                                    break;
                            }

                            traverseQueue.Enqueue((current, direction));
                            break;
                        case '\\':
                            switch (direction)
                            {
                                case Direction.Up:
                                    current.Col--;
                                    direction = Direction.Left;
                                    break;
                                case Direction.Right:
                                    current.Row++;
                                    direction = Direction.Down;
                                    break;
                                case Direction.Down:
                                    current.Col++;
                                    direction = Direction.Right;
                                    break;
                                case Direction.Left:
                                    current.Row--;
                                    direction = Direction.Up;
                                    break;
                            }

                            traverseQueue.Enqueue((current, direction));
                            break;
                        case '|':
                            switch (direction)
                            {
                                case Direction.Up:
                                    current.Row--;
                                    traverseQueue.Enqueue((current, direction));
                                    break;
                                case Direction.Down:
                                    current.Row++;
                                    traverseQueue.Enqueue((current, direction));
                                    break;
                                case Direction.Right:
                                case Direction.Left:
                                    traverseQueue.Enqueue((new(current.Row - 1, current.Col), Direction.Up));
                                    traverseQueue.Enqueue((new(current.Row + 1, current.Col), Direction.Down));
                                    break;
                            }

                            break;

                        case '-':
                            switch (direction)
                            {
                                case Direction.Right:
                                    current.Col++;
                                    traverseQueue.Enqueue((current, direction));
                                    break;
                                case Direction.Left:
                                    current.Col--;
                                    traverseQueue.Enqueue((current, direction));
                                    break;
                                case Direction.Up:
                                case Direction.Down:
                                    traverseQueue.Enqueue((new(current.Row, current.Col - 1), Direction.Left));
                                    traverseQueue.Enqueue((new(current.Row, current.Col + 1), Direction.Right));
                                    break;
                            }

                            break;
                    }
                }

                var count = energized.SelectMany(e => e).Count(e => e == true);
                lock (compareLock)
                {
                    if (count > maxEnergized)
                    {
                        maxEnergized = count;
                    }
                }

                if (part == Part.Part1)
                {
                    Print(tiles, energized);
                }
            });

            return maxEnergized;
        }

        private static void Print(char[][] tiles, bool[][] energized)
        {
            for (var i = 0; i < tiles.Length; i++)
            {
                for (var j = 0; j < tiles[i].Length; j++)
                {
                    if (energized[i][j])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('#');
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(tiles[i][j]);
                    }
                }

                Console.WriteLine();
            }
        }

        public struct Position(int row, int col)
        {
            public int Row { get; set; } = row;

            public int Col { get; set; } = col;

            public override readonly string ToString() => $"({Row}, {Col})";
        }
    }
}
