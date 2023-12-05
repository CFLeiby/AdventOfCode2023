namespace AdventOfCode;

using System.Reflection;

public class Day02 : IDayChallenge
{
    private static readonly Dictionary<string,int> ColorMaxes = new() 
        { { "red", 12 }, { "green", 13}, { "blue", 14 } };
    
    public Task Execute()
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AdventOfCode.DayTwo.Input.txt"))
        {
            if (stream == null)
            {
                Console.Write("Input file not found");
                return Task.CompletedTask;
            }
            
            using (var reader = new StreamReader(stream))
            {
                var possibleTotal = 0;
                var powerTotal = 0;
                var line = reader.ReadLine();
                while (line != null)
                {
                    var game = line.Split(':');
                    if (GameIsPossible(game[1]))
                    {
                        possibleTotal += int.Parse(game[0].Split(' ')[1]);
                    }

                    powerTotal += GetGamePower(game[1]);

                    line = reader.ReadLine();
                }
                
                Console.WriteLine("Sum of possible games: " + possibleTotal);
                Console.WriteLine("Sum of powers: " + powerTotal);
            }
        }
        return Task.CompletedTask;
    }

    private static bool GameIsPossible(string game)
    {
        var sets = game.Trim().Split(';');
        foreach (var set in sets)
        {
            var colorGroups = set.Trim().Split(",");
            foreach (var colorGroup in colorGroups)
            {
                var color = colorGroup.Trim().Split(' ');
                if (int.Parse(color[0]) > ColorMaxes[color[1]])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static int GetGamePower(string game)
    {
        var red = 0;
        var blue = 0;
        var green = 0;
        var sets = game.Trim().Split(';');
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