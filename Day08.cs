namespace AdventOfCode;

using System.Reflection;

public class Day08 : IDayChallenge
{
    private string _turns;
    private readonly Dictionary<string, Tuple<string, string>> _directions = new();
    
    public Task Execute()
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AdventOfCode.Inputs.Day08.txt"))
        {
            if (stream == null)
            {
                Console.Write("Input file not found");
                return Task.CompletedTask;
            }
            
            IEnumerable<string> directionLines;
            using (var reader = new StreamReader(stream))
            {
                _turns = reader.ReadLine()??string.Empty;
                reader.ReadLine();
                directionLines = reader.ReadToEnd().Split(Environment.NewLine);
            }

            //Lets turn our strings into a dictionary of left/right turns
            foreach (var line in directionLines)
            {
                AddToDictionary(line, _directions);
            }

            var turnCount = CountTurns(false);
            Console.WriteLine($"Turns to get to ZZZ: {turnCount}");

            turnCount = CountTurns(true);
            Console.WriteLine($"Turns to get to **Z: {turnCount}");
        }

        return Task.CompletedTask;
    }
    
    private static void AddToDictionary(string directionInfo, IDictionary<string, Tuple<string, string>> directions)
    {
        var lineParts = directionInfo.Split(" = ");
        var leftRightParts = lineParts[1].Split(", ");
        var leftRight = new Tuple<string, string>(leftRightParts[0][1..],
            leftRightParts[1][..^1]);

        directions.Add(lineParts[0], leftRight);
    }

    private long CountTurns(bool multiplePaths)
    {
        long turnCount = 0;
        if (multiplePaths)
        {
            var currentLocations = _directions.Keys.Where(k => k.EndsWith('A')).ToList();
            
            // //Looking for repeating cycles to see if we can do an LCM
            // for (var i = 0; i < currentLocations.Count; i++)
            // {
            //     var zTrips = new List<int>();
            //     var currentLocation = currentLocations[i];
            //     for (var j = 0; j < 10; j++)
            //     {
            //         var stepsToZ = 0;
            //         do
            //         {
            //             var goLeft = _turns[Convert.ToInt32(turnCount % _turns.Length)] == 'L';
            //             currentLocation = GetNextLocation(currentLocation, goLeft);
            //             turnCount++;
            //             stepsToZ++;
            //         } while (!currentLocation.EndsWith('Z'));
            //         zTrips.Add(stepsToZ);
            //     }
            //     Console.WriteLine($"Ten trips to Z on path {i}: {string.Join(", ", zTrips)}");
            // }

            //Above confirmed each path repeats regularly, so just get the length of the cycle for each and get LCM
            var zTrips = new List<long>();
            foreach (var t in currentLocations)
            {
                turnCount = 0;
                var currentPosition = t;
                while (!currentPosition.EndsWith('Z'))
                {
                    var goLeft = _turns[Convert.ToInt32(turnCount % _turns.Length)] == 'L';
                    currentPosition = GetNextLocation(currentPosition, goLeft);
                    turnCount++;
                }

                zTrips.Add(turnCount);
            }

            return GetLcm(zTrips);
        }

        var currentLocation = "AAA";
        while (currentLocation != "ZZZ")
        {
            currentLocation = GetNextLocation(currentLocation, _turns[Convert.ToInt32(turnCount % _turns.Length)] == 'L');
            turnCount++;
        }

        return turnCount;
    }

    private static long GetLcm(IEnumerable<long> values)
    {
        //Sort the distinct values biggset to smallest
        var sorted = values.Distinct().OrderByDescending(v => v).ToList();
        
        //Take the biggest number in the set to use as our base
        var baseValue = sorted[0];
        
        //Remove it from the set so we don't bother checking each time
        sorted.RemoveAt(0);
        
        //Check increasing multiples of the base to see if the other values divide in evenly.  If so, we've hit the LCM
        var multiplier = 1;
        while (sorted.Any(v => (baseValue * multiplier) % v > 0))
        {
            multiplier++;
        }

        return baseValue * multiplier;
    }
    
    private string GetNextLocation(string currentLocation, bool goLeft)
    {
        return goLeft
            ? _directions[currentLocation].Item1
            : _directions[currentLocation].Item2;

    }
}