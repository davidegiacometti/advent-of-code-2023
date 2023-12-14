using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day14 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            var lines = File.ReadAllLines(InputFilePath);
            var dish = new char[lines.Length][];
            for (var i = 0; i < lines.Length; i++)
            {
                dish[i] = lines[i].ToCharArray();
            }

            SlideNorth(dish);

            var result = 0L;
            for (var i = 0; i < dish.Length; i++)
            {
                var rocks = dish[i].Count(d => d == 'O');
                result += rocks * (dish.Length - i);
            }

            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var lines = File.ReadAllLines(InputFilePath);
            var dish = new char[lines.Length][];
            for (var i = 0; i < lines.Length; i++)
            {
                dish[i] = lines[i].ToCharArray();
            }

            for (var i = 0; i < 1000; i++)
            {
                SlideNorth(dish);
                SlideWest(dish);
                SlideSouth(dish);
                SlideEast(dish);
            }

            var result = 0L;
            for (var i = 0; i < dish.Length; i++)
            {
                var rocks = dish[i].Count(d => d == 'O');
                result += rocks * (dish.Length - i);
            }

            return new ValueTask<string>(result.ToString());
        }

        private static void SlideWest(char[][] dish)
        {
            foreach (var d in dish)
            {
                SlideWest(d, 0);
            }
        }

        private static void SlideEast(char[][] dish)
        {
            foreach (var d in dish)
            {
                SlideEast(d, d.Length - 1);
            }
        }

        private static void SlideWest(char[] dish, int index)
        {
            if (index == dish.Length - 1)
            {
                return;
            }

            switch (dish[index])
            {
                case '#':
                    SlideWest(dish, index + 1);
                    break;
                case '.':
                    if (dish[index + 1] == 'O')
                    {
                        (dish[index + 1], dish[index]) = (dish[index], dish[index + 1]);
                        SlideWest(dish, index > 0 ? index - 1 : 0);
                    }
                    else
                    {
                        SlideWest(dish, index + 1);
                    }

                    break;
                case 'O':
                    SlideWest(dish, index + 1);
                    break;
            }
        }

        private static void SlideEast(char[] dish, int index)
        {
            if (index < 1)
            {
                return;
            }

            switch (dish[index])
            {
                case '#':
                    SlideEast(dish, index - 1);
                    break;
                case '.':
                    if (dish[index - 1] == 'O')
                    {
                        (dish[index - 1], dish[index]) = (dish[index], dish[index - 1]);
                        SlideEast(dish, index == dish.Length - 1 ? dish.Length - 1 : index + 1);
                    }
                    else
                    {
                        SlideEast(dish, index - 1);
                    }

                    break;
                case 'O':
                    SlideEast(dish, index - 1);
                    break;
            }
        }

        private static void SlideNorth(char[][] dish)
        {
            for (var i = 0; i < dish.Length; i++)
            {
                SlideNorth(dish, 0, i);
            }
        }

        private static void SlideNorth(char[][] dish, int row, int col)
        {
            if (row == dish.Length - 1)
            {
                return;
            }

            switch (dish[row][col])
            {
                case '#':
                    SlideNorth(dish, row + 1, col);
                    break;
                case '.':
                    if (dish[row + 1][col] == 'O')
                    {
                        (dish[row + 1][col], dish[row][col]) = (dish[row][col], dish[row + 1][col]);
                        SlideNorth(dish, row > 0 ? row - 1 : 0, col);
                    }
                    else
                    {
                        SlideNorth(dish, row + 1, col);
                    }

                    break;
                case 'O':
                    SlideNorth(dish, row + 1, col);
                    break;
            }
        }

        private static void SlideSouth(char[][] dish)
        {
            for (var i = 0; i < dish.Length; i++)
            {
                SlideSouth(dish, dish.Length - 1, i);
            }
        }

        private static void SlideSouth(char[][] dish, int row, int col)
        {
            if (row < 1)
            {
                return;
            }

            switch (dish[row][col])
            {
                case '#':
                    SlideSouth(dish, row - 1, col);
                    break;
                case '.':
                    if (dish[row - 1][col] == 'O')
                    {
                        (dish[row - 1][col], dish[row][col]) = (dish[row][col], dish[row - 1][col]);
                        SlideSouth(dish, row == dish.Length - 1 ? dish.Length - 1 : row + 1, col);
                    }
                    else
                    {
                        SlideSouth(dish, row - 1, col);
                    }

                    break;
                case 'O':
                    SlideSouth(dish, row - 1, col);
                    break;
            }
        }

        private static void Print(char[][] arr)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                for (var j = 0; j < arr[i].Length; j++)
                {
                    Console.Write(arr[i][j]);
                }

                Console.WriteLine();
            }
        }
    }
}
