namespace AdventOfCode;

public class Day09 : DayChallengeBase
{
    protected override string InputFile => "Day09.txt";
    protected override string Part1Prefix => "Sum of next values: ";
    protected override string Part2Prefix => "Sum of powers: ";

    protected override int ExecutePart1(string line)
    {
        var lasts = new List<int>();

        var values = line.Split(' ').Select(x => Convert.ToInt32(x)).ToList();
        do
        {
            lasts.Add(values.Last());
            values = GetDiffs(values);
        } while (values.Any(v => v != 0));

        return lasts.Sum();
    }

    protected override int ExecutePart2(string line)
    {
        var firsts = new List<int>();

        var values = line.Split(' ').Select(x => Convert.ToInt32(x)).ToList();
        do
        {
            firsts.Add(values[0]);
            values = GetDiffs(values);
        } while (values.Any(v => v != 0));

        //Extrapolate the previous value for each row, working our way from the end of the list of first
        //values to the beginning, subtracting as we go 
        firsts.Reverse();
        return firsts.Aggregate(0, (current, first) => first - current);
    }

    private static List<int> GetDiffs(List<int> values)
    {
        var diffs = new List<int>();
        for (var i = 1; i < values.Count; i++)
        {
            diffs.Add(values[i] - values[i - 1]);
        }

        return diffs;
    }
}