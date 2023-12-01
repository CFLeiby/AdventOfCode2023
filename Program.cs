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
        
        IDayChallenge challenge;
        switch(day)
        {
            case 1:
                challenge = new Day01();
                break;  
            default: 
                return;
        }
        
        challenge.Execute().Wait();
    }
}