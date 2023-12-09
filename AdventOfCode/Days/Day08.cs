using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day08 : BaseDay
    {
        public enum Direction
        {
            Left = 0,
            Right = 1,
        }

        public override ValueTask<string> Solve_1()
        {
            var directions = new List<Direction>();
            var lines = File.ReadAllLines(InputFilePath);
            foreach (var d in lines.First())
            {
                directions.Add(d == 'L' ? Direction.Left : Direction.Right);
            }

            var bucket = new List<Node>();
            foreach (var line in lines.Skip(2).OrderBy(l => l))
            {
                var value = line.Substring(0, 3);
                var left = line.Substring(7, 3);
                var right = line.Substring(12, 3);
                bucket.Add(new Node(value, left, right));
            }

            var currentNode = bucket.First();
            var zzzFound = false;
            var steps = 0;
            while (!zzzFound)
            {
                foreach (var d in directions)
                {
                    steps++;
                    currentNode = FindNext(currentNode, d, bucket);

                    if (currentNode.Value.Equals("ZZZ", StringComparison.Ordinal))
                    {
                        zzzFound = true;
                        break;
                    }
                }
            }

            return new ValueTask<string>(steps.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var directions = new List<Direction>();
            var lines = File.ReadAllLines(InputFilePath);
            foreach (var d in lines.First())
            {
                directions.Add(d == 'L' ? Direction.Left : Direction.Right);
            }

            var bucket = new List<Node>();
            foreach (var line in lines.Skip(2).OrderBy(l => l))
            {
                var value = line.Substring(0, 3);
                var left = line.Substring(7, 3);
                var right = line.Substring(12, 3);
                bucket.Add(new Node(value, left, right));
            }

            var nodes = bucket.Where(n => n.Value[2] == 'A').ToList();
            var steps = new List<long>();

            foreach (var n in nodes)
            {
                var s = 0;
                var currentNode = n;
                var zFound = false;
                while (!zFound)
                {
                    foreach (var d in directions)
                    {
                        s++;
                        currentNode = FindNext(currentNode, d, bucket);

                        if (currentNode.Value[2] == 'Z')
                        {
                            zFound = true;
                            break;
                        }
                    }
                }

                steps.Add(s);
            }

            var result = FindLCM(steps);
            return new ValueTask<string>(result.ToString());
        }

        private static Node FindNext(Node node, Direction direction, List<Node> bucket)
        {
            var nextNode = direction == Direction.Left ? node.Left : node.Right;
            return bucket.Single(n => n.Value.Equals(nextNode, StringComparison.Ordinal));
        }

        private static long FindLCM(List<long> numbers)
        {
            long lcm = 1;

            foreach (long number in numbers)
            {
                lcm = CalculateLCM(lcm, number);
            }

            return lcm;
        }

        private static long CalculateLCM(long a, long b)
        {
            return (a * b) / FindGCD(a, b);
        }

        private static long FindGCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public struct Node(string value, string left, string right)
        {
            public string Value { get; set; } = value;

            public string Left { get; set; } = left;

            public string Right { get; set; } = right;

            public override readonly string ToString() => $"{Value} = ({Left}, {Right})";
        }
    }
}
