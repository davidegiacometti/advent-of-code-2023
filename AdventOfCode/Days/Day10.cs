using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day10 : BaseDay
    {
        public enum Direction
        {
            None = 0,
            Up = 1,
            Right = 2,
            Down = 3,
            Left = 4,
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
                if (startPos[i].Row < 0 || startPos[i].Col < 0 || startPos[i].Row > pipes.Length - 1 || startPos[i].Col == pipes[startPos[i].Row].Length - 1)
                {
                    continue;
                }

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
                if (startPos[i].Row < 0 || startPos[i].Col < 0 || startPos[i].Row > pipes.Length - 1 || startPos[i].Col == pipes[startPos[i].Row].Length - 1)
                {
                    continue;
                }

                var steps = 0;
                do
                {
                    traversed.Add(startPos[i]);
                }
                while (Traverse(pipes, ref startPos[i], ref steps));

                if (steps > 0)
                {
                    break;
                }
            }

            var notTraversed = new List<Position>();
            for (var i = 0; i < pipes.Length; i++)
            {
                for (var j = 0; j < pipes[i].Length; j++)
                {
                    if (!traversed.Any(p => p.Row == i && p.Col == j))
                    {
                        notTraversed.Add(new Position(i, j));
                    }
                }
            }

            var clusters = new List<HashSet<Position>>();
            while (notTraversed.Count > 0)
            {
                var currentCluster = new HashSet<Position>();
                var clusterQueue = new Queue<Position>();
                clusterQueue.Enqueue(notTraversed[0]);
                while (clusterQueue.Count > 0)
                {
                    var tile = clusterQueue.Dequeue();
                    var adjacentTiles = GetAdjacentPositions(tile).Where(notTraversed.Contains);
                    foreach (var t in adjacentTiles)
                    {
                        clusterQueue.Enqueue(t);
                        notTraversed.Remove(t);
                    }

                    currentCluster.Add(tile);
                }

                // Remove edge touching clusters
                if (!currentCluster.Any(c => c.Row == 0 || c.Col == 0 || c.Row == pipes.Length - 1 || c.Col == pipes[c.Row].Length - 1))
                {
                    clusters.Add(currentCluster);
                }
            }

            var insideLoop = clusters.SelectMany(c => c).ToList();

            var result = 0L;
            foreach (var pos in clusters.SelectMany(c => c).ToArray())
            {
                var inversions = CountInversions(pipes, traversed, pos.Row, pos.Col);
                if (inversions % 2 == 1)
                {
                    result++;
                }
                else
                {
                    insideLoop.Remove(pos);
                }
            }

            Print(pipes, traversed, insideLoop);
            return new ValueTask<string>(result.ToString());
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

        private static void Print(char[][] pipes, List<Position> traversed, List<Position> insideLoop)
        {
            for (var i = 0; i < pipes.Length; i++)
            {
                for (var j = 0; j < pipes[i].Length; j++)
                {
                    if (traversed.Any(p => p.Row == i && p.Col == j))
                    {
                        Console.Write(pipes[i][j]);
                    }
                    else if (insideLoop.Any(p => p.Row == i && p.Col == j))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('I');
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('O');
                        Console.ResetColor();
                    }
                }

                Console.WriteLine();
            }
        }

        private static IEnumerable<Position> GetAdjacentPositions(Position pos)
        {
            var cols = Enumerable.Range(pos.Col - 1, 3).ToArray();
            return new[] { new Position(pos.Row, cols[0]), new Position(pos.Row, cols[1]) } // Same row
                .Concat(cols.Select(col => new Position(pos.Row - 1, col))) // Previous row
                .Concat(cols.Select(col => new Position(pos.Row + 1, col))); // Next row
        }

        // Ray casting algorithm: https://en.wikipedia.org/wiki/Point_in_polygon
        private static int CountInversions(char[][] pipes, List<Position> traversed, int row, int col)
        {
            var line = pipes[row];
            var count = 0;
            for (var i = 0; i < col; i++)
            {
                if (!traversed.Any(p => p.Row == row && p.Col == i))
                {
                    continue;
                }

                count += line[i] == 'J' || line[i] == 'L' || line[i] == '|' ? 1 : 0;
            }

            return count;
        }

        public struct Position(int row, int col, Direction dir)
        {
            public Position(int row, int col)
                : this(row, col, Direction.None) // Don't care about direction
            {
            }

            public int Row { get; set; } = row;

            public int Col { get; set; } = col;

            public Direction Direction { get; set; } = dir;

            public readonly override string ToString() => $"({Row}, {Col})";
        }
    }
}
