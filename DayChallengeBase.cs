namespace AdventOfCode;

using System.Reflection;

public abstract class DayChallengeBase
{
    public Task Execute()
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AdventOfCode.Inputs.{InputFile}"))
        {
            if (stream == null)
            {
                Console.Write("Input file not found");
                return Task.CompletedTask;
            }
            
            using (var reader = new StreamReader(stream))
            {
                var part1Total = 0;
                var part2Total = 0;
                var line = reader.ReadLine();
                while (line != null)
                {

                    part1Total += ExecutePart1(line);
                    part2Total += ExecutePart2(line);
                    
                    line = reader.ReadLine();
                }
                
                Console.WriteLine(Part1Prefix + part1Total);
                Console.WriteLine(Part2Prefix + part2Total);
            }
        }
        return Task.CompletedTask;
    }

    protected abstract string InputFile { get; }
    protected abstract string Part1Prefix { get; }
    protected abstract string Part2Prefix { get; }
    protected abstract int ExecutePart1(string line);
    protected abstract int ExecutePart2(string line);
}