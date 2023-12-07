namespace AdventOfCode;

using System.Net.Http.Headers;
using System.Reflection;

public class Day07 : IDayChallenge
{
    public Task Execute()
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AdventOfCode.Inputs.Day07.txt"))
        {
            if (stream == null)
            {
                Console.Write("Input file not found");
                return Task.CompletedTask;
            }

            IEnumerable<string> handInfos;
            using (var reader = new StreamReader(stream))
            {
                handInfos = reader.ReadToEnd().Split(Environment.NewLine);
            }

            var totalWinnings = 0;
            var hands = handInfos.Select(x => new Hand(x, false)).OrderBy(x => x).ToList();
            totalWinnings += hands.Select((t, i) => (i + 1) * t.Bid).Sum();
            Console.WriteLine($"Total winnings (J is for Jack): {totalWinnings}");
            
            totalWinnings = 0;
            hands = handInfos.Select(x => new Hand(x, true)).OrderBy(x => x).ToList();
            totalWinnings += hands.Select((t, i) => (i + 1) * t.Bid).Sum();
            Console.WriteLine($"Total winnings (J is for Joker): {totalWinnings}");
        }

        return Task.CompletedTask;
    }

    private class Hand : IComparable
    {
        public Hand(string handInfo, bool useJokers)
        {
            var handParts = handInfo.Split(' ');
            Cards = handParts[0].Select(c => CardValue(c, useJokers)).ToArray();
            Bid = int.Parse(handParts[1]);
            Rank = HandRank(Cards, useJokers);
        }
        
        public int Bid { get; }
        private int[] Cards { get; }
        private int Rank { get; }
        
        private static int CardValue(char card, bool jIsForJoker)
        {
            return card switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => jIsForJoker ? 0 : 11,
                'T' => 10,
                _ => int.Parse(card.ToString())
            };
        }

        private static int HandRank(IEnumerable<int> cards, bool useJokers)
        {
            var grouped = cards.GroupBy(x => x).ToList();
            var jokers = useJokers ? grouped.FirstOrDefault(g => g.Key == 0) : null;
            switch(grouped.Count)
            {
                case 5:
                    //All different cards.  If one is a joker, we have a pair
                    return jokers == null ? 0 : 1;

                case 4: 
                    //one pair.  If we have any jokers, it's actually 3 of a kind
                    //(either the pair is jokers so we combine with one of the other 
                    //single cards, or we have a single joker to combine with the pair)
                    return jokers == null ? 1 : 3;
                    
                case 3:
                    if (grouped.Any(g => g.Count() == 3))
                    {
                        //three of a kind.  If we have any jokers, it's 4 of a kind
                        return jokers == null ? 3 : 5;
                    }
                    
                    // two pair.  If we have one joker, it's a full house.  If we have
                    //two, it's 4 of a kind
                    return (jokers?.Count() ?? 0) switch
                    {
                        1 => 4,
                        2 => 5,
                        _ => 2
                    };

                case 2:
                    //If either group is jokers, it's 5 of a kind
                    if (jokers != null) return 6;

                    return grouped.Any(g => g.Count() == 4)
                        ? 5 //four of a kind
                        : 4; //full house

                default: 
                    // only thing left is 5 of a kind
                    return 6; 
            };
        }
        
        public int CompareTo(object obj)
        {
            var otherHand = (Hand)obj;
            if (otherHand == null)
                return 0;
            
            if (otherHand.Rank > Rank)
                return -1;
            if (otherHand.Rank < Rank)
                return 1;

            for (var i = 0; i < Cards.Length; i++)
            {
                if (Cards[i] < otherHand.Cards[i])
                    return -1;
                if (Cards[i] > otherHand.Cards[i])
                    return 1;
            }

            //We should never get here
            return 0;
        }
    }

    private long HandStrength(string hand)
    {
        var cards = hand.Split(' ')[0];
        var grouped = cards.GroupBy(x => x);
        //How many groups
        return 0;
    }
}