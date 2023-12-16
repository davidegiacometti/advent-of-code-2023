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
            var lines = File.ReadAllLines(InputFilePath);
            var tiles = new char[lines.Length][];
            var energized = new bool[lines.Length][];
            for (var i = 0; i < lines.Length; i++)
            {
                tiles[i] = lines[i].ToCharArray();
                energized[i] = new bool[tiles[i].Length];
            }

            var traverseQueue = new Queue<(Position Position, Direction Direction)>();
            var traversed = new HashSet<(Position Position, Direction Direction)>();
            traverseQueue.Enqueue((new(0, 0), Direction.Right));

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

            Print(tiles, energized);
            var result = energized.SelectMany(e => e).Count(e => e == true);
            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new ValueTask<string>(string.Empty);
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
