namespace AdventOfCode;

using System.Reflection;
using System.Xml;

public class Day01 : IDayChallenge
{
    private static readonly string[] NumberNames = 
        { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    
    public Task Execute()
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AdventOfCode.DayOne.Input.txt"))
        {
            using (var reader = new StreamReader(stream))
            {
                var backwardNames = NumberNames.Select(n => new string(n.Reverse().ToArray())).ToArray();
                var total = 0;
                var input = reader.ReadToEnd().Split(Environment.NewLine);
                foreach (var line in input)
                {
                    var firstDigit = GetFirstNumber(line, NumberNames);
                    var secondDigit = GetFirstNumber(new string(line.Reverse().ToArray()), backwardNames);
                    total += int.Parse(new[] { firstDigit, secondDigit });
                }
                
                Console.WriteLine(total);
            }
        }
        return Task.CompletedTask;
    }

    private static char GetFirstNumber(string line, IReadOnlyList<string> numberNames)
    {
        var firstDigitPos = line.TakeWhile(c => !char.IsDigit(c)).Count();
        if (firstDigitPos == line.Length)
        {
            firstDigitPos = -1;
        }

        var firstWordValue = 0;
        var firstWordPos = line.Length;
        for(var i = 0; i < NumberNames.Length; i++)
        {
            var index = line.IndexOf(numberNames[i], StringComparison.Ordinal);
            if (index <= -1 || index >= firstWordPos)
            {
                continue;
            }

            firstWordPos = index;
            firstWordValue = i + 1;

        }
        return firstWordPos < firstDigitPos ? firstWordValue.ToString()[0] : line[firstDigitPos];

        return '0';
    }
}