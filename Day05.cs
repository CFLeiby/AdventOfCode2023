namespace AdventOfCode;

using System.Reflection;
using System.Text.RegularExpressions;

public class Day05 : IDayChallenge
{
    private string[] _lines;
    private List<MapRange> SeedToSoil = new();
    private List<MapRange> SoilToFertilizer = new();
    private List<MapRange> FertilizerToWater = new();
    private List<MapRange> WaterToLight = new();
    private List<MapRange> LightToTemp = new();
    private List<MapRange> TempToHumidity = new();
    private List<MapRange> HumidityToLoc = new();

    private record MapRange(long SourceRangeStart, long SourceRangeEnd, long DestinationRangeStart);

    public Task Execute()
    {
        try
        {
            LoadMaps();
            IEnumerable<long> seeds;
            
            using (var stream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream($"AdventOfCode.Inputs.Day05.Seeds.txt"))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Seeds.txt not found");
                }

                using (var reader = new StreamReader(stream))
                {
                    var line = reader.ReadLine();
                    seeds = line?.Split(' ').Select(long.Parse);
                }
            }

            if (seeds == null)
            {
                Console.WriteLine("Where are the seeds?");
                return Task.CompletedTask;
            }
            
            var closestLocation = (from seed in seeds 
                select MapValue(seed, SeedToSoil) into soil 
                select MapValue(soil, SoilToFertilizer) into fert 
                select MapValue(fert, FertilizerToWater) into water 
                select MapValue(water, WaterToLight) into light 
                select MapValue(light, LightToTemp) into temp 
                select MapValue(temp, TempToHumidity) into humidity 
                select MapValue(humidity, HumidityToLoc)).Min();
            
            Console.WriteLine($"Closest location: {closestLocation}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return Task.CompletedTask;
    }

    private void LoadMaps()
    {
        LoadMap("SeedToSoil.txt", SeedToSoil);
        LoadMap("SoilToFertilizer.txt", SoilToFertilizer);
        LoadMap("FertilizerToWater.txt", FertilizerToWater);
        LoadMap("WaterToLight.txt", WaterToLight);
        LoadMap("LightToTemp.txt", LightToTemp);
        LoadMap("TempToHumidity.txt", TempToHumidity);
        LoadMap("HumidityToLoc.txt", HumidityToLoc);
    }

    private static void LoadMap(string sourceFile, ICollection<MapRange> targetMap)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AdventOfCode.Inputs.Day05.{sourceFile}");
        if (stream == null)
        {
            throw new FileNotFoundException($"{sourceFile} not found");
        }

        using var reader = new StreamReader(stream);
        var line = reader.ReadLine();
        do
        {
            if (string.IsNullOrEmpty(line))
            {
                break;
            }
            var range = line.Split(' ');
            var sourceStart = long.Parse(range[1]);
            var mapEntry = new MapRange(sourceStart, sourceStart + long.Parse(range[2]), long.Parse(range[0]));
            targetMap.Add(mapEntry);
            line = reader.ReadLine();
        } while (line != null);
    }

    private static long MapValue(long sourceValue, IEnumerable<MapRange> map)
    {
        //Look for the first range that our source value falls into
        var range = map.FirstOrDefault(m => m.SourceRangeStart <= sourceValue && m.SourceRangeEnd >= sourceValue);
        //None defined? Then the destination value is the same
        if (range == null)
            return sourceValue;
        
        //destination value will be the same distance from the start of its range as the source is from its
        return range.DestinationRangeStart + (sourceValue - range.SourceRangeStart);
    }
}