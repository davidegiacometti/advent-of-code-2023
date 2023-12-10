using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day10 : BaseDay
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
            var pipes = new char[lines.Length][];
            var row = 0;
            var col = 0;

            for (var i = 0; i < lines.Length; i++)
            {
                var lineChars = lines[i].ToCharArray();
                pipes[i] = lineChars;
                var indexOfS = Array.IndexOf(lineChars, 'S');
                if (indexOfS != -1)
                {
                    row = i;
                    col = indexOfS;
                }
            }

            var startPos = new Position[] { new(row - 1, col, Direction.Up), new(row, col + 1, Direction.Right), new(row + 1, col, Direction.Down), new(row, col - 1, Direction.Left) };
            for (var i = 0; i < startPos.Length; i++)
            {
                var steps = 0;
                while (Traverse(pipes, ref startPos[i], ref steps))
                {
                }

                if (steps > 0)
                {
                    steps = steps % 2 == 0 ? steps / 2 : (steps / 2) + 1;

                    return new ValueTask<string>(steps.ToString());
                }
            }

            return new ValueTask<string>(string.Empty);
        }

        public override ValueTask<string> Solve_2()
        {
            var lines = File.ReadAllLines(InputFilePath);
            var pipes = new char[lines.Length][];
            var row = 0;
            var col = 0;

            for (var i = 0; i < lines.Length; i++)
            {
                var lineChars = lines[i].ToCharArray();
                pipes[i] = lineChars;
                var indexOfS = Array.IndexOf(lineChars, 'S');
                if (indexOfS != -1)
                {
                    row = i;
                    col = indexOfS;
                }
            }

            var startPos = new Position[] { new(row - 1, col, Direction.Up), new(row, col + 1, Direction.Right), new(row + 1, col, Direction.Down), new(row, col - 1, Direction.Left) };
            var traversed = new List<Position>();
            for (var i = 0; i < startPos.Length; i++)
            {
                var steps = 0;
                while (Traverse(pipes, ref startPos[i], ref steps))
                {
                    traversed.Add(startPos[i]);
                }

                if (steps > 0)
                {
                    break;
                }
            }

            Print(pipes, traversed);

            return new ValueTask<string>(string.Empty);
        }

        private static bool Traverse(char[][] pipes, ref Position position, ref int steps)
        {
            var tile = pipes[position.Row][position.Col];
            switch (tile)
            {
                case '|':
                    {
                        if (position.Direction == Direction.Up)
                        {
                            position.Row--;
                        }
                        else if (position.Direction == Direction.Down)
                        {
                            position.Row++;
                        }
                        else
                        {
                            break;
                        }

                        steps++;
                        return true;
                    }

                case '-':
                    {
                        if (position.Direction == Direction.Right)
                        {
                            position.Col++;
                        }
                        else if (position.Direction == Direction.Left)
                        {
                            position.Col--;
                        }
                        else
                        {
                            break;
                        }

                        steps++;
                        return true;
                    }

                case 'L':
                    {
                        if (position.Direction == Direction.Down)
                        {
                            position.Col++;
                            position.Direction = Direction.Right;
                        }
                        else if (position.Direction == Direction.Left)
                        {
                            position.Row--;
                            position.Direction = Direction.Up;
                        }
                        else
                        {
                            break;
                        }

                        steps++;
                        return true;
                    }

                case 'J':
                    {
                        if (position.Direction == Direction.Right)
                        {
                            position.Row--;
                            position.Direction = Direction.Up;
                        }
                        else if (position.Direction == Direction.Down)
                        {
                            position.Col--;
                            position.Direction = Direction.Left;
                        }
                        else
                        {
                            break;
                        }

                        steps++;
                        return true;
                    }

                case '7':
                    {
                        if (position.Direction == Direction.Up)
                        {
                            position.Col--;
                            position.Direction = Direction.Left;
                        }
                        else if (position.Direction == Direction.Right)
                        {
                            position.Row++;
                            position.Direction = Direction.Down;
                        }
                        else
                        {
                            break;
                        }

                        steps++;
                        return true;
                    }

                case 'F':
                    {
                        if (position.Direction == Direction.Up)
                        {
                            position.Col++;
                            position.Direction = Direction.Right;
                        }
                        else if (position.Direction == Direction.Left)
                        {
                            position.Row++;
                            position.Direction = Direction.Down;
                        }
                        else
                        {
                            break;
                        }

                        steps++;
                        return true;
                    }
            }

            return false;
        }

        private static void Print(char[][] pipes, List<Position> traversed)
        {
            for (var i = 0; i < pipes.Length; i++)
            {
                for (var j = 0; j < pipes[i].Length; j++)
                {
                    if (traversed.Any(p => p.Row == i && p.Col == j))
                    {
                        Console.Write('o');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }

                Console.WriteLine();
            }
        }

        public struct Position(int row, int col, Direction dir)
        {
            public int Row { get; set; } = row;

            public int Col { get; set; } = col;

            public Direction Direction { get; set; } = dir;
        }
    }
}
