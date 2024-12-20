using System.Diagnostics;

namespace AdventOfCode2024.Days;

public record Day20 : Day<Day20>, IDay<Day20>
{
    public static int DayNumber => 20;

    private static readonly (int j, int i)[] adj = [(1, 0), (0, 1), (-1, 0), (0, -1)];

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var (map, seq) = GetDistances(input);

        var result = 0;
        foreach (var (j, i) in seq)
        {
            foreach (var (oj, oi) in adj)
            {
                var (tj, ti) = (j + 2 * oj, i + 2 * oi);
                if (tj < 0 || tj >= map.Length || ti < 0 || ti >= map[0].Length) continue;
                if (map[tj][ti] is -1) continue;
                var saving = map[j][i] - map[tj][ti];
                if (saving >= 102) result++;
            }
        }

        return result;
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        const int Cheat = 20;

        var (map, _) = GetDistances(input);

        var result = 0;
        for (var j = 0; j < input.Length; j++)
        {
            for (var i = 0; i < input[0].Length; i++)
            {
                if (map[j][i] is -1) continue;
                for (var J = j; J < Math.Min(input.Length, j + Cheat + 1); J++)
                {
                    var jDist = J - j;
                    for (var I = Math.Max(0, i - Cheat + jDist); I < Math.Min(input[0].Length, i + Cheat + 1 - jDist); I++)
                    {
                        if (J == j && I < i) continue;
                        if (map[J][I] is -1) continue;

                        var diff = Math.Abs(map[j][i] - map[J][I]);
                        var dist = jDist + Math.Abs(I - i);
                        if (diff >= 100 + dist) result++;
                    }
                }
            }
        }

        return result;
    }

    private static (int[][] Map, List<(int j, int i)> Seq) GetDistances(ImmutableArray<string> input)
    {
        var map = input.Select(_ => _.Select(_ => _ switch { '#' => -1, _ => int.MaxValue }).ToArray()).ToArray();

        var (y, x) = GetStart(input);

        List<(int j, int i)> seq = [];
        for (var i = 0; true; i++)
        {
            map[y][x] = i;

            seq.Add((y, x));

            var end = true;
            foreach (var (oy, ox) in adj)
            {
                if (map[y + oy][x + ox] is int.MaxValue)
                {
                    (y, x) = (y + oy, x + ox);
                    end = false;
                    break;
                }
            }

            if (end) break;
        }

        return(map, seq);
    }

    private static (int j, int i) GetStart(ImmutableArray<string> input)
    {
        for (var j = 0; j < input.Length; j++)
        {
            for (var i = 0; i < input[0].Length; i++)
            {
                if (input[j][i] is 'S')
                {
                    return (j, i);
                }
            }
        }

        throw new UnreachableException();
    }
}
