using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day19 : BaseDay
    {
        public enum Comparison
        {
            Greater = 0,
            Less = 1,
        }

        public override ValueTask<string> Solve_1()
        {
            var lines = File.ReadAllLines(InputFilePath);
            var workflow = new List<Rule>();
            var ratings = new List<Rating>();
            var parsingWorkflow = true;

            foreach (var line in lines)
            {
                if (line.Length == 0)
                {
                    parsingWorkflow = false;
                    continue;
                }

                if (parsingWorkflow)
                {
                    var endNameIndex = line.IndexOf('{');
                    var rule = new Rule(line[..endNameIndex]);
                    var splitRules = line[(endNameIndex + 1)..^1].Split(',');

                    foreach (var part in splitRules)
                    {
                        if (part.Contains('<') || part.Contains('>'))
                        {
                            var rating = part[0];
                            var comparison = part[1];
                            var indexOfSemicolon = part.IndexOf(':');
                            var compareValue = int.Parse(part[2..indexOfSemicolon]);
                            var compareResult = part[(indexOfSemicolon + 1)..];
                            rule.Conditions.Add(new Condition(rating, (compareValue, comparison == '<' ? Comparison.Less : Comparison.Greater), compareResult));
                        }
                        else
                        {
                            rule.Else = part;
                        }
                    }

                    workflow.Add(rule);
                }
                else
                {
                    var splittedLine = line.Trim('{', '}').Split(',');
                    var x = 0;
                    var m = 0;
                    var a = 0;
                    var s = 0;

                    foreach (var part in splittedLine)
                    {
                        switch (part[0])
                        {
                            case 'x':
                                x = int.Parse(part[2..]);
                                break;
                            case 'm':
                                m = int.Parse(part[2..]);
                                break;
                            case 'a':
                                a = int.Parse(part[2..]);
                                break;
                            case 's':
                                s = int.Parse(part[2..]);
                                break;
                        }
                    }

                    ratings.Add(new Rating(x, m, a, s));
                }
            }

            var result = 0L;

            foreach (var rating in ratings)
            {
                var ruleResult = "in";
                do
                {
                    var rule = workflow.Single(r => r.Name.Equals(ruleResult, StringComparison.Ordinal));
                    ruleResult = ApplyRule(rating, rule);
                }
                while (ruleResult.Length > 1); // A or R

                if (ruleResult.Equals("A", StringComparison.Ordinal))
                {
                    result += rating.X + rating.M + rating.A + rating.S;
                }
            }

            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new ValueTask<string>(string.Empty);
        }

        private static string ApplyRule(Rating rating, Rule rule)
        {
            foreach (var condition in rule.Conditions)
            {
                var r = 0;
                switch (condition.Category)
                {
                    case 'x':
                        r = rating.X;
                        break;
                    case 'm':
                        r = rating.M;
                        break;
                    case 'a':
                        r = rating.A;
                        break;
                    case 's':
                        r = rating.S;
                        break;
                }

                if ((condition.Expression.Comparison == Comparison.Less && r < condition.Expression.Value) ||
                    (condition.Expression.Comparison == Comparison.Greater && r > condition.Expression.Value))
                {
                    return condition.Result;
                }
            }

            return rule.Else;
        }

        public struct Rule(string name)
        {
            public string Name { get; set; } = name;

            public List<Condition> Conditions { get; set; } = new List<Condition>();

            public string Else { get; set; }

            public readonly override string ToString() => Name;
        }

        public struct Condition(char category, (int, Comparison) expression, string result)
        {
            public char Category { get; set; } = category;

            public (int Value, Comparison Comparison) Expression { get; set; } = expression;

            public string Result { get; set; } = result;

            public readonly override string ToString() => $"{Category} {Expression.Comparison} {Expression.Value} : {Result}";
        }

        public struct Rating(int x, int m, int a, int s)
        {
            public int X { get; set; } = x;

            public int M { get; set; } = m;

            public int A { get; set; } = a;

            public int S { get; set; } = s;

            public readonly override string ToString() => $"x={X}, m={M}, a={A}, s={S}";
        }
    }
}
