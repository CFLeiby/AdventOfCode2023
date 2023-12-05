namespace AdventOfCode;

public class Day01 : DayChallengeBase
{
    private static readonly string[] NumberNames = 
        { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    private static readonly string[] BackwardsNames = 
        NumberNames.Select(n => new string(n.Reverse().ToArray())).ToArray();
    
    protected override string InputFile => "Day01.txt";
    
    protected override string Part1Prefix => "Total with numbers only: ";

    protected override string Part2Prefix => "Total with number names: ";

    protected override int ExecutePart1(string line)
    {
        var reverseLine = line.Reverse().ToArray();
        var firstDigit = GetFirstNumber(line, null);
        var secondDigit = GetFirstNumber(new string(reverseLine), null);
        return int.Parse(new[] { firstDigit, secondDigit }); 
    }

    protected override int ExecutePart2(string line)
    {
        var reverseLine = line.Reverse().ToArray();
        var firstDigit = GetFirstNumber(line, NumberNames);
        var secondDigit = GetFirstNumber(new string(reverseLine), BackwardsNames);
        return int.Parse(new[] { firstDigit, secondDigit });
    }

    private static char GetFirstNumber(string line, IReadOnlyList<string> numberNames)
    {
        if (numberNames == null)
        {
            return line.First(char.IsNumber);
        }
        
        var firstDigitPos = line.TakeWhile(c => !char.IsDigit(c)).Count();
        if (firstDigitPos == line.Length)
        {
            firstDigitPos = -1;
        }

        var firstWordValue = 0;
        var firstWordPos = line.Length;
        for(var i = 0; i < NumberNames.Length; i++)
        {
            var index = line.IndexOf(numberNames[i], StringComparison.Ordinal);
            if (index <= -1 || index >= firstWordPos)
            {
                continue;
            }

            firstWordPos = index;
            firstWordValue = i + 1;

        }
        return firstWordPos < firstDigitPos ? firstWordValue.ToString()[0] : line[firstDigitPos];
    }
}