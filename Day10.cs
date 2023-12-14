namespace AdventOfCode;

using System.Reflection;

public class Day10 : IDayChallenge
{
    private char[][] _map;
    private int _maxColumn;
    private int _maxRow;
    
    public Task Execute()
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AdventOfCode.Inputs.Day10.txt"))
        {
            if (stream == null)
            {
                Console.Write("Input file not found");
                return Task.CompletedTask;
            }

            using (var reader = new StreamReader(stream))
            {
                var lines = reader.ReadToEnd().Split(Environment.NewLine);
                _maxColumn = lines[0].Length - 1;
                _maxRow = lines.Length - 1;

                _map = lines.Select(l => l.ToCharArray()).ToArray();
                
                var maxDistance = ExecutePart1();
                 Console.WriteLine($"Max distance: {maxDistance}");

                 var tilesInLoop = ExecutePart2();
                 Console.WriteLine($"Total tiles: {tilesInLoop}");
            }
        }

        return Task.CompletedTask;
    }

    private int ExecutePart1()
    {
        //First step: find the entrance
        int startRow;
        var startColumn = 0;
        var found = false;
        for (startRow = 0; startRow <= _maxColumn; startRow++)
        {
            for (startColumn = 0; startColumn <= _maxRow; startColumn++)
            {
                if (_map[startRow][startColumn] != 'S')
                {
                    continue;
                }
                
                found = true;
                break;
            }

            if (found)
            {
                break;
            }
        }
        
        //Now that we found the start, let's find the loop.
        //Try each direction until we find it
        var loop = ((GoNorth(startRow - 1, startColumn) ?? GoEast(startRow, startColumn + 1)) ?? GoSouth(startRow + 1, startColumn)) ?? GoWest(startRow, startColumn - 1);

        //Farthest point is in the middle of the loop
        return (int)Math.Ceiling(loop.Count/2m);
    }

    private int ExecutePart2()
    {
        //TODO
        return 0;
    }
    
    private List<char> GoNorth(int row, int column)
    {
        if (row < 0)
        {
            //Can't go north from here
            return null;
        }

        var nextStep = _map[row][column];
        if (nextStep == 'S')
        {
            //We've completed the loop!
            return new List<char>();
        }

        var remainingSteps = nextStep switch
        {
            '|' => GoNorth(row - 1, column),
            '7' => GoWest(row, column - 1),
            'F' => GoEast(row, column + 1),
            _ => null
        };

        if (remainingSteps == null)
        {
            return null;
        }
        
        //We reached the start again; add this step to the list and return it
        remainingSteps.Add(nextStep);
        return remainingSteps;
    }
    
    private List<char> GoEast(int row, int column)
    {
        if (column > _maxColumn)
        {
            //Can't go east from here
            return null;
        }

        var nextStep = _map[row][column];
        if (nextStep == 'S')
        {
            //We've completed the loop!
            return new List<char>();
        }

        var remainingSteps = nextStep switch
        {
            '-' => GoEast(row, column + 1),
            'J' => GoNorth(row - 1, column),
            '7' => GoSouth(row + 1, column),
            _ => null
        };

        if (remainingSteps == null)
        {
            return null;
        }
        
        //We reached the start again; add this step to the list and return it
        remainingSteps.Add(nextStep);
        return remainingSteps;
    }
    
    private List<char> GoSouth(int row, int column)
    {
        if (row > _maxRow)
        {
            //Can't go south from here
            return null;
        }

        var nextStep = _map[row][column];
        if (nextStep == 'S')
        {
            return new List<char>();
        }

        var remainingSteps = nextStep switch
        {
            '|' => GoSouth(row + 1, column),
            'J' => GoWest(row, column - 1),
            'L' => GoEast(row, column + 1),
            _ => null
        };

        if (remainingSteps == null)
        {
            return null;
        }
        
        //We reached the start again; add this step to the list and return it
        remainingSteps.Add(nextStep);
        return remainingSteps;
    }

    private List<char> GoWest(int row, int column)
    {
        if (column < 0)
        {
            //Can't go west from here
            return null;
        }

        var nextStep = _map[row][column];
        if (nextStep == 'S')
        {
            //We've completed the loop!
            return new List<char>();
        }

        var remainingSteps = nextStep switch
        {
            '-' => GoWest(row, column - 1),
            'L' => GoNorth(row - 1, column),
            'F' => GoSouth(row + 1, column),
            _ => null
        };

        if (remainingSteps == null)
        {
            return null;
        }
        
        //We reached the start again; add this step to the list and return it
        remainingSteps.Add(nextStep);
        return remainingSteps;
    }
}