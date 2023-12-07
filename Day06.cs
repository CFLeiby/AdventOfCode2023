namespace AdventOfCode;

using System.Reflection;

public class Day06 : IDayChallenge
{
    public Task Execute()
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AdventOfCode.Inputs.Day06.txt"))
        {
            if (stream == null)
            {
                Console.Write("Input file not found");
                return Task.CompletedTask;
            }

            using (var reader = new StreamReader(stream))
            {
                var timeLine = reader.ReadLine()?[5..].Trim();
                if (timeLine == null)
                {
                    Console.Write("No times!");
                    return Task.CompletedTask;
                }
                var distanceLine = reader.ReadLine()?[9..].Trim();
                if (distanceLine == null)
                {
                    Console.Write("No distances!");
                    return Task.CompletedTask;
                }

                var combos = 1;
                var times = timeLine.Split(' ').Where(t => !string.IsNullOrEmpty(t)).ToList();
                var distances = distanceLine.Split(' ').Where(t => !string.IsNullOrEmpty(t)).ToList();
                for (var i = 0; i < times.Count; i++)
                {
                    combos *= GetCountOfWinningTimes(int.Parse(times[i]), int.Parse(distances[i]));
                }
                Console.WriteLine($"Ways to win (multiple races): {combos}");

                var time = long.Parse(timeLine.Replace(" ", ""));
                var distance = long.Parse(distanceLine.Replace(" ", ""));
                combos = GetCountOfWinningTimes(time, distance);
                Console.WriteLine($"Ways to win (one race): {combos}");
            }
        }

        return Task.CompletedTask;
    }

    private static int GetCountOfWinningTimes(long time, long goal)
    {
        var winningTimes = 0;
        //Times on the high and low ends aren't gonna work, so lets' start in the middle 
        //and work our way up until we hit a time that doesn't work
        var midpoint = (long)Math.Round(time / 2d);
        for (var j = midpoint; j < time; j++)
        {
            var distance = j * (time - j);
            if (distance <= goal)
            {
                break;
            }

            winningTimes++;
        }
                
        //Do the same starting in the middle and going down
        for (var j = midpoint - 1; j > -1; j--)
        {
            var distance = j * (time - j);
            if (distance <= goal)
            {
                break;
            }

            winningTimes++;
        }
            
        return winningTimes;
    }
}