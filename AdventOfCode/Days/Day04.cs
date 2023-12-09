using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day04 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            using var sr = new StreamReader(InputFilePath);
            var result = 0;
            string? line;

            while ((line = sr.ReadLine()) != null)
            {
                var gameScore = 0;
                var colonIndex = line.IndexOf(':');
                var numbers = line[(colonIndex + 2)..].Split('|');
                if (numbers.Length != 2)
                {
                    continue;
                }

                var winningNumbers = numbers[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var gameNumbers = numbers[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                foreach (var winningNumber in winningNumbers.Intersect(gameNumbers))
                {
                    gameScore = gameScore == 0 ? 1 : gameScore * 2;
                }

                result += gameScore;
            }

            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            using var sr = new StreamReader(InputFilePath);
            string? line;
            int gameIndex = 1;
            var cards = new List<int>();

            while ((line = sr.ReadLine()) != null)
            {
                var colonIndex = line.IndexOf(':');
                var numbers = line[(colonIndex + 2)..].Split('|');
                if (numbers.Length != 2)
                {
                    continue;
                }

                if (cards.Count < gameIndex)
                {
                    cards.Add(1);
                }
                else
                {
                    cards[gameIndex - 1]++;
                }

                var winningNumbers = numbers[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var gameNumbers = numbers[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                var winsCount = winningNumbers.Intersect(gameNumbers).Count();
                var currentCards = cards[gameIndex - 1];

                for (var i = gameIndex + 1; i <= gameIndex + winsCount; i++)
                {
                    if (i > cards.Count)
                    {
                        cards.Add(currentCards);
                    }
                    else
                    {
                        cards[i - 1] += currentCards;
                    }
                }

                gameIndex++;
            }

            return new ValueTask<string>(cards.Sum().ToString());
        }
    }
}
