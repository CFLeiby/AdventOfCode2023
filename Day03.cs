namespace AdventOfCode;

using System.Reflection;
using System.Text.RegularExpressions;

public class Day03 : IDayChallenge 
{
    public Task Execute()
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AdventOfCode.Inputs.Day03.txt"))
        {
            if (stream == null)
            {
                Console.Write("Input file not found");
                return Task.CompletedTask;
            }

            using (var reader = new StreamReader(stream))
            {
                var lines = reader.ReadToEnd().Split(Environment.NewLine);
                var partNumberTotal = 0;

                //part numbers from the first line
                partNumberTotal += GetPartNumbers(lines[0], null, lines[1]).Sum();
                
                //part numbers from the last line
                var lastLine = lines.Length - 1;
                partNumberTotal += GetPartNumbers(lines[lastLine],lines[lastLine -1], null).Sum();

                //part numbers for each line in between
                for(var i = 1; i < lastLine; i++)
                {
                    partNumberTotal += GetPartNumbers(lines[i], lines[i-1], lines[i+1]).Sum();
                }
                
                Console.WriteLine($"Sum of all part numbers: {partNumberTotal}");
            }
        }

        return Task.CompletedTask;
    }

    private static IEnumerable<int> GetPartNumbers(string line, string lineAbove, string lineBelow)
    {
        var partNumbers = new List<int>();

        var potentialNumbers = Regex.Matches(line, @"\d+");
        foreach (Match potentialNumber in potentialNumbers)
        {
            var numberRange =
                new Tuple<int, int>(potentialNumber.Index, potentialNumber.Index + potentialNumber.Length-1);
            if (IsPartNumber(numberRange, line, lineAbove, lineBelow))
            {
                partNumbers.Add(int.Parse(potentialNumber.Value));
            }
        }
        return partNumbers;
    }
    
    private static bool IsPartNumber(Tuple<int, int> characterRange, string line, string lineAbvoe, string lineBelow)
    {
        int lowerBound;
        if (characterRange.Item1 > 0)
        {
            lowerBound = characterRange.Item1 - 1;
            if (line[lowerBound] != '.')
            {
                return true;
            }
        }
        else
        {
            lowerBound = 0;
        }

        int upperBound;
        if (characterRange.Item2 < line.Length - 1)
        {
            upperBound = characterRange.Item2 + 1;
            if (line[upperBound] != '.')
            {
                return true;
            }
        }
        else
        {
            upperBound = line.Length - 1;
        }

        return ContainsSymbol(lineAbvoe, lowerBound, upperBound) || ContainsSymbol(lineBelow, lowerBound, upperBound);
    }

    private static bool ContainsSymbol(string line, int from, int to)
    {
        if (string.IsNullOrEmpty(line))
        {
            return false;
        }
        
        for(var i = from; i <= to; i++)
        {
            if(line[i] != '.')
            {
                return true;
            }
        }

        return false;
    }
}