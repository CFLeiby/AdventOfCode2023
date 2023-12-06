namespace AdventOfCode;

public class Day04 : DayChallengeBase
{
    private readonly Dictionary<int, int> _bonusCards = new();
    private readonly Dictionary<int, int> _matchCounts = new();
    
    protected override string InputFile => "Day04.txt";
    protected override string Part1Prefix => "Total points: ";
    protected override string Part2Prefix => "Total cards: ";

    protected override int ExecutePart1(string line)
    {
        var game = line.Split(':');
        var gameNumber = int.Parse(game[0][4..].Trim());
        var matches = GetMatches(gameNumber, game[1]);
        return matches == 0 ? 0 : (int)Math.Pow(2,matches - 1); 
    }

    protected override int ExecutePart2(string line)
    {
        var game = line.Split(':');
        var gameNumber = int.Parse(game[0][4..].Trim());
        var matches = GetMatches(gameNumber, game[1]);
        
        //We win one of each bonus card for the card sent in...
        var currentGameCards = 1;
        //Plus another one for any bonus copies we have won of the current game 
        if (_bonusCards.TryGetValue(gameNumber, out var bonusCopies))
        {
            currentGameCards += bonusCopies;
        }
        
        for (var i = 1; i < matches + 1; i++)
        {
            var bonusGameNumber = gameNumber + i;
            if (_bonusCards.ContainsKey(bonusGameNumber))
            {
                _bonusCards[bonusGameNumber] += currentGameCards;
            }
            else
            {
                _bonusCards.Add(bonusGameNumber, currentGameCards);
            }
        }
        
        //Return the number of copies of cards for the current game that we just played
        return currentGameCards;
    }

    private int GetMatches(int gameNumber, string game)
    {
        if (_matchCounts.TryGetValue(gameNumber, out var matches))
        {
            return matches;
        }

        var numbers = game.Split('|');
        var winners = numbers[0].Trim().Split(' ').Where(s => !string.IsNullOrWhiteSpace(s));
        var ours = numbers[1].Trim().Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct();
        matches = ours.Count(n => winners.Contains(n));
        
        //cache a copy for part 2 to use before returning
        _matchCounts.Add(gameNumber, matches);
        return matches;
    }
}