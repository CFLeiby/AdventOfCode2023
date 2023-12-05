namespace AdventOfCode;

using System.Reflection;
using System.Text.RegularExpressions;

public class Day03 : IDayChallenge
{
    private string[] _lines;
    private IEnumerable<Tuple<int, int>>[] _partNosByLine;
    
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
                _lines = reader.ReadToEnd().Split(Environment.NewLine);
                _partNosByLine = new IEnumerable<Tuple<int, int>>[_lines.Length];
                
                //First line
                _partNosByLine[0] = GetPartNumbers(_lines[0], null, _lines[1]);
                
                //Last line
                var lastLine = _lines.Length - 1;
                _partNosByLine[lastLine] = GetPartNumbers(_lines[lastLine],_lines[lastLine -1], null);

                //Each line in between
                for(var i = 1; i < lastLine; i++)
                {
                    _partNosByLine[i] = GetPartNumbers(_lines[i], _lines[i-1], _lines[i+1]);
                }

                var partNoTotal = 0;
                var gearRatioTotal = 0;
                for (var i = 0; i < _lines.Length; i++)
                {
                    partNoTotal += _partNosByLine[i].Sum(pn =>
                    {
                        var partNo = _lines[i].Substring(pn.Item1, pn.Item2 - pn.Item1 + 1);
                        return int.Parse(partNo);
                    });

                    gearRatioTotal += GetGearRatios(i).Sum();
                }

                Console.WriteLine($"Sum of all part numbers: {partNoTotal}");
                Console.WriteLine($"Sum of all gear ratios: {gearRatioTotal}");
            }
        }

        return Task.CompletedTask;
    }

    private IEnumerable<int> GetGearRatios(int lineNumber)
    {
        //Find all the gear symbols in the line
        var potentialGears = Regex.Matches(_lines[lineNumber], @"\*");
        var gearRatios = new List<int>();
        foreach (Match potentialGear in potentialGears)
        {
            var touchingPartNos = new List<int>();

            //is there a part number to the left?
            var range = new Tuple<int, int>(potentialGear.Index - 1, potentialGear.Index + 1);
            var part = _partNosByLine[lineNumber].FirstOrDefault(pn => pn.Item2 == range.Item1);
            if (part != null)
            {
                var partNo = int.Parse(_lines[lineNumber].Substring(part.Item1, part.Item2 - part.Item1 + 1));
                touchingPartNos.Add(partNo);
            }

            //is there a part number to the right?
            part = _partNosByLine[lineNumber].FirstOrDefault(pn => pn.Item1 == range.Item2);
            if (part != null)
            {
                var partNo = int.Parse(_lines[lineNumber].Substring(part.Item1, part.Item2 - part.Item1 + 1));
                touchingPartNos.Add(partNo);
            }

            //How many are above?
            if (lineNumber > 0)
            {
                foreach (var partNo in _partNosByLine[lineNumber - 1].Where(pn =>
                             (pn.Item1 >= range.Item1 && pn.Item1 <= range.Item2) ||
                             (pn.Item2 >= range.Item1 && pn.Item2 <= range.Item2)))
                {
                    var number = int.Parse(_lines[lineNumber - 1].Substring(partNo.Item1, partNo.Item2 - partNo.Item1 + 1));
                    touchingPartNos.Add(number);                
                }
            }
            
            //got too many yet?
            if (touchingPartNos.Count > 2)
            {
                continue;
            }
            
            //How many below?
            if (lineNumber < _lines.Length - 1)
            {
                foreach (var partNo in _partNosByLine[lineNumber + 1].Where(pn =>
                             (pn.Item1 >= range.Item1 && pn.Item1 <= range.Item2) ||
                             (pn.Item2 >= range.Item1 && pn.Item2 <= range.Item2)))
                {
                    var number = int.Parse(_lines[lineNumber + 1].Substring(partNo.Item1, partNo.Item2 - partNo.Item1 + 1));
                    touchingPartNos.Add(number);                
                }
            }

            if (touchingPartNos.Count == 2)
            {
                gearRatios.Add(touchingPartNos[0] * touchingPartNos[1]);
            }
        }

        return gearRatios;
    }
    
    private static IEnumerable<Tuple<int, int>> GetPartNumbers(string line, string lineAbove, string lineBelow)
    {
        var partNumbers = new List<Tuple<int, int>>();

        var potentialNumbers = Regex.Matches(line, @"\d+");
        foreach (Match potentialNumber in potentialNumbers)
        {
            var numberRange =
                new Tuple<int, int>(potentialNumber.Index, potentialNumber.Index + potentialNumber.Length-1);
            if (IsPartNumber(numberRange, line, lineAbove, lineBelow))
            {
                partNumbers.Add(numberRange);
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