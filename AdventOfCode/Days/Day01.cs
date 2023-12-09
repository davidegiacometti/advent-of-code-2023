using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day01 : BaseDay
    {
        private readonly Dictionary<string, int> _digits;

        public Day01()
        {
            var digits = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            _digits = Enumerable.Range(1, digits.Length).ToDictionary(d => digits[d - 1]);
        }

        public override ValueTask<string> Solve_1()
        {
            return Solve(false);
        }

        public override ValueTask<string> Solve_2()
        {
            return Solve(true);
        }

        private ValueTask<string> Solve(bool searchLettersDigits)
        {
            using var sr = new StreamReader(InputFilePath);
            var result = 0;
            string? line;

            while ((line = sr.ReadLine()) != null)
            {
                int? firstDigitIndex = null;
                int? firstDigit = null;
                int? lastDigitIndex = null;
                int? lastDigit = null;

                for (var i = 0; i < line.Length; i++)
                {
                    var firstChar = line[i] - '0';
                    if (firstChar >= 0 && firstChar <= 9)
                    {
                        firstDigit = firstChar;
                        firstDigitIndex = i;
                        break;
                    }
                }

                if (firstDigitIndex != null)
                {
                    for (var i = line.Length - 1; i >= firstDigitIndex; i--)
                    {
                        var lastChar = line[i] - '0';
                        if (lastChar >= 0 && lastChar <= 9)
                        {
                            lastDigit = lastChar;
                            lastDigitIndex = i;
                            break;
                        }
                    }
                }

                if (searchLettersDigits)
                {
                    foreach (var digit in _digits.Keys)
                    {
                        var firstIndex = line.IndexOf(digit, StringComparison.OrdinalIgnoreCase);
                        if (firstIndex != -1 && (firstDigitIndex == null || firstIndex < firstDigitIndex))
                        {
                            firstDigitIndex = firstIndex;
                            firstDigit = _digits[digit];
                        }

                        var lastIndex = line.LastIndexOf(digit, StringComparison.OrdinalIgnoreCase);
                        if (lastIndex != -1 && (lastDigitIndex == null || lastIndex > lastDigitIndex))
                        {
                            lastDigitIndex = lastIndex;
                            lastDigit = _digits[digit];
                        }
                    }
                }

                if (firstDigit != null && lastDigit != null)
                {
                    var lineValueStr = $"{firstDigit}{lastDigit}";
                    if (int.TryParse(lineValueStr, out int lineValue))
                    {
                        result += lineValue;
                    }
                }
            }

            return ValueTask.FromResult(result.ToString());
        }
    }
}
