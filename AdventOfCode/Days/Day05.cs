using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode.Days
{
    public class Day05 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            using var sr = new StreamReader(InputFilePath);
            string? line;
            string? section = null;
            long? minLocation = null;

            var seeds = new List<long>();
            var seedSoilMap = new List<Mapping>();
            var soilFertilizerMap = new List<Mapping>();
            var fertilizerWaterMap = new List<Mapping>();
            var waterLightMap = new List<Mapping>();
            var lightTemperatureMap = new List<Mapping>();
            var temperatureHumidityMap = new List<Mapping>();
            var humidityLocationMap = new List<Mapping>();

            while ((line = sr.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    continue;
                }

                if (line.StartsWith("seeds: ", StringComparison.OrdinalIgnoreCase))
                {
                    section = "seeds";
                    seeds = line.Split(' ').Skip(1).Select(s => long.Parse(s)).ToList();
                    continue;
                }
                else if (line.Contains(':'))
                {
                    section = line[..line.IndexOf(':')];
                }

                if (section != null)
                {
                    var pattern = @"\d+";
                    var matches = Regex.Matches(line, pattern);

                    if (matches.Count == 3
                        && long.TryParse(matches[0].Value, out var dst)
                        && long.TryParse(matches[1].Value, out var src)
                        && long.TryParse(matches[2].Value, out var range))
                    {
                        if (section.Equals("seed-to-soil map", StringComparison.OrdinalIgnoreCase))
                        {
                            seedSoilMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("soil-to-fertilizer map", StringComparison.OrdinalIgnoreCase))
                        {
                            soilFertilizerMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("fertilizer-to-water map", StringComparison.OrdinalIgnoreCase))
                        {
                            fertilizerWaterMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("water-to-light map", StringComparison.OrdinalIgnoreCase))
                        {
                            waterLightMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("light-to-temperature map", StringComparison.OrdinalIgnoreCase))
                        {
                            lightTemperatureMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("temperature-to-humidity map", StringComparison.OrdinalIgnoreCase))
                        {
                            temperatureHumidityMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("humidity-to-location map", StringComparison.OrdinalIgnoreCase))
                        {
                            humidityLocationMap.Add(new Mapping(src, dst, range));
                        }
                    }
                }
            }

            foreach (var seed in seeds)
            {
                var soil = MapLookup(seed, seedSoilMap);
                var fertilizer = MapLookup(soil, soilFertilizerMap);
                var water = MapLookup(fertilizer, fertilizerWaterMap);
                var light = MapLookup(water, waterLightMap);
                var temperature = MapLookup(light, lightTemperatureMap);
                var humidity = MapLookup(temperature, temperatureHumidityMap);
                var location = MapLookup(humidity, humidityLocationMap);

                if (minLocation == null || location < minLocation)
                {
                    minLocation = location;
                }
            }

            return new ValueTask<string>(minLocation?.ToString() ?? "0");
        }

        public override ValueTask<string> Solve_2()
        {
            using var sr = new StreamReader(InputFilePath);
            string? line;
            string? section = null;
            long? minLocation = null;

            var seeds = new List<long>();
            var seedSoilMap = new List<Mapping>();
            var soilFertilizerMap = new List<Mapping>();
            var fertilizerWaterMap = new List<Mapping>();
            var waterLightMap = new List<Mapping>();
            var lightTemperatureMap = new List<Mapping>();
            var temperatureHumidityMap = new List<Mapping>();
            var humidityLocationMap = new List<Mapping>();

            while ((line = sr.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    continue;
                }

                if (line.StartsWith("seeds: ", StringComparison.OrdinalIgnoreCase))
                {
                    section = "seeds";
                    seeds = line.Split(' ').Skip(1).Select(s => long.Parse(s)).ToList();
                    continue;
                }
                else if (line.Contains(':'))
                {
                    section = line[..line.IndexOf(':')];
                }

                if (section != null)
                {
                    var pattern = @"\d+";
                    var matches = Regex.Matches(line, pattern);

                    if (matches.Count == 3
                        && long.TryParse(matches[0].Value, out var dst)
                        && long.TryParse(matches[1].Value, out var src)
                        && long.TryParse(matches[2].Value, out var range))
                    {
                        if (section.Equals("seed-to-soil map", StringComparison.OrdinalIgnoreCase))
                        {
                            seedSoilMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("soil-to-fertilizer map", StringComparison.OrdinalIgnoreCase))
                        {
                            soilFertilizerMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("fertilizer-to-water map", StringComparison.OrdinalIgnoreCase))
                        {
                            fertilizerWaterMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("water-to-light map", StringComparison.OrdinalIgnoreCase))
                        {
                            waterLightMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("light-to-temperature map", StringComparison.OrdinalIgnoreCase))
                        {
                            lightTemperatureMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("temperature-to-humidity map", StringComparison.OrdinalIgnoreCase))
                        {
                            temperatureHumidityMap.Add(new Mapping(src, dst, range));
                        }
                        else if (section.Equals("humidity-to-location map", StringComparison.OrdinalIgnoreCase))
                        {
                            humidityLocationMap.Add(new Mapping(src, dst, range));
                        }
                    }
                }
            }

            // This assumes seeds are even
            // Let's brute force :/
            for (var i = 0; i < seeds.Count - 1; i += 2)
            {
                GetSeeds(seeds[i], seeds[i + 1]).AsParallel().ForAll(s =>
                {
                    var soil = MapLookup(s, seedSoilMap);
                    var fertilizer = MapLookup(soil, soilFertilizerMap);
                    var water = MapLookup(fertilizer, fertilizerWaterMap);
                    var light = MapLookup(water, waterLightMap);
                    var temperature = MapLookup(light, lightTemperatureMap);
                    var himidity = MapLookup(temperature, temperatureHumidityMap);
                    var location = MapLookup(himidity, humidityLocationMap);

                    if (minLocation == null || location < minLocation)
                    {
                        minLocation = location;
                    }
                });
            }

            return new ValueTask<string>(minLocation?.ToString() ?? "0");
        }

        private static long MapLookup(long item, List<Mapping> map)
        {
            long? result = null;

            foreach (var m in map)
            {
                if (item >= m.Source && item <= m.Source + m.Range)
                {
                    var destination = m.Destination + (item - m.Source);
                    if (result == null || destination < result)
                    {
                        result = destination;
                    }
                }
            }

            return result ?? item;
        }

        private static IEnumerable<long> GetSeeds(long start, long count)
        {
            for (var i = start; i < start + count; i++)
            {
                yield return i;
            }
        }

        public struct Mapping(long source, long destination, long range)
        {
            public long Source { get; set; } = source;

            public long Destination { get; set; } = destination;

            public long Range { get; set; } = range;
        }
    }
}
