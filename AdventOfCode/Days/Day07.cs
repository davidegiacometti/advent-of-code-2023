using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day07 : BaseDay
    {
        public static readonly Dictionary<char, int> _cardScore = new()
        {
            { 'A', 13 },
            { 'K', 12 },
            { 'Q', 11 },
            { 'J', 10 },
            { 'T', 9 },
            { '9', 8 },
            { '8', 7 },
            { '7', 6 },
            { '6', 5 },
            { '5', 4 },
            { '4', 3 },
            { '3', 2 },
            { '2', 1 },
        };

        public static readonly Dictionary<char, int> _cardScoreJockers = new()
        {
            { 'A', 13 },
            { 'K', 12 },
            { 'Q', 11 },
            { 'T', 10 },
            { '9', 9 },
            { '8', 8 },
            { '7', 7 },
            { '6', 6 },
            { '5', 5 },
            { '4', 4 },
            { '3', 3 },
            { '2', 2 },
            { 'J', 1 },
        };

        public override ValueTask<string> Solve_1()
        {
            var result = Solve(false);
            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var result = Solve(true);
            return new ValueTask<string>(result.ToString());
        }

        public int Solve(bool jokers)
        {
            using var sr = new StreamReader(InputFilePath);
            string? line;
            var hands = new List<Hand>();

            while ((line = sr.ReadLine()) != null)
            {
                var lineParts = line.Split(' ');
                if (lineParts.Length == 2)
                {
                    var cards = lineParts[0].ToCharArray();
                    var bid = int.Parse(lineParts[1]);
                    var hand = new Hand(cards, bid, jokers);
                    hands.Add(hand);
                }
            }

            hands.Sort();

            var result = 0;
            for (var i = 0; i < hands.Count; i++)
            {
                result += hands[i].Bid * (i + 1);
            }

            return result;
        }

        public struct Hand : IComparable<Hand>
        {
            public Hand(char[] cards, int bid, bool jokers)
            {
                Cards = cards;
                Bid = bid;
                Type = GetHandType(jokers);
                Jokers = jokers;
            }

            internal enum HandType
            {
                HighCard = 0,
                OnePair = 1,
                TwoPair = 2,
                ThreeOfAKind = 3,
                FullHouse = 4,
                FourOfAKind = 5,
                FiveOfAKind = 6,
            }

            public char[] Cards { get; set; }

            public int Bid { get; set; }

            public bool Jokers { get; set; }

            internal HandType Type { get; }

            public readonly int CompareTo(Hand other)
            {
                if (Type < other.Type)
                {
                    return -1;
                }
                else if (Type > other.Type)
                {
                    return 1;
                }
                else
                {
                    for (var i = 0; i < Cards.Length; i++)
                    {
                        if (Cards[i] == other.Cards[i])
                        {
                            continue;
                        }

                        var instanceScore = Jokers ? _cardScoreJockers[Cards[i]] : _cardScore[Cards[i]];
                        var otherScore = other.Jokers ? _cardScoreJockers[other.Cards[i]] : _cardScore[other.Cards[i]];

                        if (instanceScore < otherScore)
                        {
                            return -1;
                        }
                        else if (instanceScore > otherScore)
                        {
                            return 1;
                        }
                    }

                    return 0;
                }
            }

            internal readonly HandType GetHandType(bool jokers)
            {
                var cardGroups = Cards.GroupBy(g => g);
                var groupsCount = cardGroups.Count();

                if (jokers && Cards.Any(c => c == 'J'))
                {
                    var cardsCount = new List<(char Card, int Count, int Score)>();
                    foreach (var c in Cards.Where(c => c != 'J').Distinct())
                    {
                        cardsCount.Add((c, Cards.Count(cc => cc == c), _cardScoreJockers[c]));
                    }

                    var jockerValue = _cardScoreJockers.First().Key;
                    if (cardsCount.Count > 0)
                    {
                        jockerValue = cardsCount.OrderByDescending(c => c.Count).ThenByDescending(c => c.Score).First().Card;
                    }

                    var cardsCopy = Cards.ToArray();
                    for (var i = 0; i < cardsCopy.Length; i++)
                    {
                        if (cardsCopy[i] == 'J')
                        {
                            cardsCopy[i] = jockerValue;
                        }
                    }

                    cardGroups = cardsCopy.GroupBy(g => g);
                    groupsCount = cardGroups.Count();
                }

                switch (groupsCount)
                {
                    case 1:
                        return HandType.FiveOfAKind;
                    case 2:
                        if (cardGroups.Any(g => g.Count() == 4))
                        {
                            return HandType.FourOfAKind;
                        }
                        else
                        {
                            return HandType.FullHouse;
                        }

                    case 3:
                        if (cardGroups.Any(g => g.Count() == 3))
                        {
                            return HandType.ThreeOfAKind;
                        }
                        else
                        {
                            return HandType.TwoPair;
                        }

                    case 4:
                        return HandType.OnePair;
                    default:
                        return HandType.HighCard;
                }
            }
        }
    }
}
