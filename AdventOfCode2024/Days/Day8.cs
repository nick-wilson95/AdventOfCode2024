using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day8 : Day<Day8>, IDay<Day8>
{
    public static int DayNumber => 8;

    public static object SolvePart1(ImmutableArray<string> input) => Solve(input,
        (a, b, width, height) =>
        {
            var (j, i) = (b.j + b.j - a.j, b.i + b.i - a.i);

            return (j >= height || j < 0 || i >= width || i < 0)
            ? []
            : [(j, i)];
        });

    public static object SolvePart2(ImmutableArray<string> input) => Solve(input,
        (a, b, width, height) =>
        {
            var result = new List<(int, int)>();
            for (var n = 0; true; n++)
            {
                var (j, i) = (b.j + n * (b.j - a.j), b.i + n * (b.i - a.i));

                if (j >= height || j < 0 || i >= width || i < 0) return result;

                result.Add((j, i));
            }
        });

    private static int Solve(ImmutableArray<string> input, Func<(int j, int i), (int j, int i), int, int, List<(int j, int i)>> getAntinodes)
    {
        var height = input.Length;
        var width = input[0].Length;

        Dictionary<char, HashSet<(int j, int i)>> locationsByFreq = [];
        for (var j = 0; j < input.Length; j++)
        {
            for (var i = 0; i < input[0].Length; i++)
            {
                var freq = input[j][i];
                if (freq is '.') continue;

                var set = locationsByFreq.GetValueOrDefault(freq, []);
                set.Add((j, i));
                locationsByFreq[freq] = set;
            }
        }

        HashSet<(int j, int i)> antinodes = [];
        foreach (var locations in locationsByFreq.Values)
        {
            foreach (var (a, b) in locations.Cartesian(locations, (a, b) => (a, b)))
            {
                if (a == b) continue;

                foreach (var antinode in getAntinodes(a, b, width, height)) antinodes.Add(antinode);
            }
        }

        return antinodes.Count;
    }
}
