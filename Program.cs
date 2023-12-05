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
        
        DayChallengeBase challengeBase;
        switch(day)
        {
            case 1:
                challengeBase = new Day01();
                break;
            case 2:
                challengeBase = new Day02();
                break;
            
            default: 
                Console.WriteLine($"Day {day} does not exist yet!");
                return;
        }
        
        challengeBase.Execute().Wait();
    }
}