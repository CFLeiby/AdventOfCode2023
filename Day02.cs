namespace AdventOfCode;

public class Day02 : DayChallengeBase
{
    private static readonly Dictionary<string,int> ColorMaxes = new() 
        { { "red", 12 }, { "green", 13}, { "blue", 14 } };
    
    protected override string InputFile => "Day02.txt";
    protected override string Part1Prefix => "Sum of possible games: ";
    protected override string Part2Prefix => "Sum of powers: ";

    protected override int ExecutePart1(string line)
    {
        var game = line.Split(':');
        var sets = game[1].Trim().Split(';');
        foreach (var set in sets)
        {
            var colorGroups = set.Trim().Split(",");
            foreach (var colorGroup in colorGroups)
            {
                var color = colorGroup.Trim().Split(' ');
                if (int.Parse(color[0]) > ColorMaxes[color[1]])
                {
                    return 0;
                }
            }
        }

        return int.Parse(game[0].Split(' ')[1]);
    }

    protected override int ExecutePart2(string line)
    {
        var red = 0;
        var blue = 0;
        var green = 0;
        var sets = line.Split(':')[1].Trim().Split(';');
        foreach (var set in sets)
        {
            var colorGroups = set.Trim().Split(",");
            foreach (var colorGroup in colorGroups)
            {
                var color = colorGroup.Trim().Split(' ');
                var colorCount = int.Parse(color[0]);
                switch (color[1])
                {
                    case "red":
                        red = Math.Max(red, colorCount);
                        break;
                    case "blue":
                        blue = Math.Max(blue, colorCount);
                        break;
                    case "green":
                        green = Math.Max(green, colorCount);
                        break;
                }
            }
        }
        return red * blue * green;
    }
}