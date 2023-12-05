namespace AdventOfCode;

using System.Reflection;
using System.Xml;

public class Day01 : IDayChallenge
{
    private static readonly string[] NumberNames = 
        { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    
    public Task Execute()
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AdventOfCode.Inputs.Day01.txt"))
        {
            if (stream == null)
            {
                Console.Write("Input file not found");
                return Task.CompletedTask;
            }
            
            using (var reader = new StreamReader(stream))
            {
                var backwardNames = NumberNames.Select(n => new string(n.Reverse().ToArray())).ToArray();
                var totalNumbersOnly = 0;
                var totalNames = 0;
                var line = reader.ReadLine();
                while (line != null)
                {
                    var reverseLine = line.Reverse().ToArray();
                    var firstDigit = GetFirstNumber(line, null);
                    var secondDigit = GetFirstNumber(new string(reverseLine), null);
                    totalNumbersOnly += int.Parse(new[] { firstDigit, secondDigit }); 
                    
                    firstDigit = GetFirstNumber(line, NumberNames);
                    secondDigit = GetFirstNumber(new string(reverseLine), backwardNames);
                    totalNames += int.Parse(new[] { firstDigit, secondDigit });
                    
                    line = reader.ReadLine();
                }
                
                Console.WriteLine("Total with numbers only: " + totalNumbersOnly);
                Console.WriteLine("Total with number names: " + totalNames);
            }
        }
        return Task.CompletedTask;
    }

    private static char GetFirstNumber(string line, IReadOnlyList<string> numberNames)
    {
        if (numberNames == null)
        {
            return line.First(char.IsNumber);
        }
        
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
    }
}