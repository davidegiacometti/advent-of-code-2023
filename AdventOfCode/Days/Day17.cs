using System.ComponentModel;
using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day17 : BaseDay
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

        public int Solve(Part part)
        {
            var lines = File.ReadAllLines(InputFilePath);
            var map = new int[lines.Length][];
            for (var i = 0; i < lines.Length; i++)
            {
                map[i] = new int[lines[i].Length];
                for (var j = 0; j < lines[i].Length; j++)
                {
                    map[i][j] = int.Parse(lines[i].Substring(j, 1));
                }
            }

            var start = new Position(0, 0);
            var end = new Position(map.Length - 1, map[0].Length - 1);

            var minSteps = part == Part.Part1 ? 1 : 4;
            var maxSteps = part == Part.Part1 ? 3 : 10;
            return Traverse(map, start, end, minSteps, maxSteps);
        }

        private static int Traverse(int[][] map, Position start, Position end, int minSteps, int maxSteps)
        {
            var queue = new PriorityQueue<(Position Position, Direction Direction, int Steps), int>();
            var visited = new Dictionary<(Direction, int), int>[map.Length][];
            for (var i = 0; i < map.Length; i++)
            {
                visited[i] = new Dictionary<(Direction, int), int>[map[i].Length];
                for (var j = 0; j < map[i].Length; j++)
                {
                    visited[i][j] = new Dictionary<(Direction, int), int>();
                }
            }

            queue.Enqueue((start, Direction.Right, 0), 0);
            queue.Enqueue((start, Direction.Down, 0), 0);

            while (queue.Count > 0)
            {
                var (position, direction, steps) = queue.Dequeue();
                var heat = visited[position.Row][position.Col].GetValueOrDefault((direction, steps));

                if (steps < maxSteps)
                {
                    Move(position, direction, heat, steps, minSteps, maxSteps, map, queue, visited);
                }

                if (steps >= minSteps)
                {
                    Move(position, TurnLeft(direction), heat, 0, minSteps, maxSteps, map, queue, visited);
                    Move(position, TurnRight(direction), heat, 0, minSteps, maxSteps, map, queue, visited);
                }
            }

            return visited[end.Row][end.Col].Min(h => h.Value);
        }

        private static Direction TurnLeft(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Right => Direction.Up,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                _ => throw new InvalidEnumArgumentException(),
            };
        }

        private static Direction TurnRight(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new InvalidEnumArgumentException(),
            };
        }

        private static void Move(
            Position position,
            Direction direction,
            int heat,
            int steps,
            int minSteps,
            int maxSteps,
            int[][] map,
            PriorityQueue<(Position Position, Direction Direction, int Steps), int> queue,
            Dictionary<(Direction, int), int>[][] visited)
        {
            for (var i = 0; i < maxSteps; i++)
            {
                switch (direction)
                {
                    case Direction.Up:
                        position.Row--;
                        break;
                    case Direction.Right:
                        position.Col++;
                        break;
                    case Direction.Down:
                        position.Row++;
                        break;
                    case Direction.Left:
                        position.Col--;
                        break;
                }

                var newSteps = steps + i + 1;

                if (newSteps > maxSteps || position.Row < 0 || position.Col < 0 || position.Row >= map.Length || position.Col >= map[position.Row].Length)
                {
                    return;
                }

                heat += map[position.Row][position.Col];

                if (i + 1 < minSteps)
                {
                    continue;
                }

                var visitedHeats = visited[position.Row][position.Col];

                if (visitedHeats.TryGetValue((direction, newSteps), out var visitedHeat) && visitedHeat <= heat)
                {
                    return;
                }

                queue.Enqueue((position, direction, newSteps), heat);
                visitedHeats[(direction, newSteps)] = heat;
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
