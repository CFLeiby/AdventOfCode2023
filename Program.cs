namespace AdventOfCode;

/// <summary>
/// The program.
/// </summary>
internal static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0 || !int.TryParse(args[0], out int day))
        {
            return;
        }
        
        IDayChallenge challengeBase;
        switch(day)
        {
            case 1:
                challengeBase = new Day01();
                break;
            case 2:
                challengeBase = new Day02();
                break;
            case 3:
                challengeBase = new Day03();
                break;
            case 4:
                challengeBase = new Day04();
                break;
            case 5:
                challengeBase = new Day05();
                break;
            case 6:
                challengeBase = new Day06();
                break;
            case 7:
                challengeBase = new Day07();
                break;
            default: 
                Console.WriteLine($"Day {day} does not exist yet!");
                return;
        }
        
        challengeBase.Execute().Wait();
    }
}