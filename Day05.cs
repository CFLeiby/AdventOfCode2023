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

    private record MapRange(long SourceRangeStart, long SourceRangeEnd, long DestinationRangeStart)
    {
        public long MapValue(long sourceValue)
        {
            return DestinationRangeStart + sourceValue - SourceRangeStart;
        }
    }

    public async Task Execute()
    {
        try
        {
            LoadMaps();
            List<long> seeds;
            
            using (var stream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream($"AdventOfCode.Inputs.Day05.Seeds.txt"))
            {
                if (stream == null)
                {
                    Console.WriteLine($"Seeds.txt not found");
                    return;
                }

                using (var reader = new StreamReader(stream))
                {
                    var line = await reader.ReadLineAsync();
                    seeds = line?.Split(' ').Select(long.Parse).ToList();
                }
            }

            if (seeds == null)
            {
                Console.WriteLine("Where are the seeds?");
                return;
            }
            
            var closestLocation = (from seed in seeds 
                select MapValue(seed, SeedToSoil) into soil 
                select MapValue(soil, SoilToFertilizer) into fertilize 
                select MapValue(fertilize, FertilizerToWater) into water 
                select MapValue(water, WaterToLight) into light 
                select MapValue(light, LightToTemp) into temp 
                select MapValue(temp, TempToHumidity) into humidity 
                select MapValue(humidity, HumidityToLoc)).Min();
            Console.WriteLine($"Closest location (discrete numbers): {closestLocation}");

            var tasks = new List<Task<long>>();
            for (var i = 0; i < seeds.Count; i += 2)
            {
                var rangeStart = seeds[i];
                var rangeSize = seeds[i + 1];
                tasks.Add(Task.Run(() =>
                {
                    closestLocation = long.MaxValue;
                    for (var j = 0; j < rangeSize; j++)
                    {
                        var seed =  rangeStart + j;
                        var soil = MapValue(seed, SeedToSoil);
                        var fertilize = MapValue(soil, SoilToFertilizer);
                        var water = MapValue(fertilize, FertilizerToWater);
                        var light = MapValue(water, WaterToLight);
                        var temp = MapValue(light, LightToTemp);
                        var humidity = MapValue(temp, TempToHumidity);
                        var loc = MapValue(humidity, HumidityToLoc);
                        closestLocation = Math.Min(closestLocation, loc);
                    }

                    return closestLocation;
                }));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Closest location (ranges): {tasks.Min(t => t.Result)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
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
        return range?.MapValue(sourceValue)??sourceValue;
    }
}