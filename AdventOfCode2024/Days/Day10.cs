using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day10 : Day<Day10>, IDay<Day10>
{
    public static int DayNumber => 10;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var map = input
            .Select(_ => _.Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray();

        var length = map.Length;
        var width = map[0].Length;

        var sets = Enumerable.Range(0, length)
            .Select(_ => Enumerable.Range(0, width)
                .Select(_ => new HashSet<(int j, int i)>())
                .ToArray())
            .ToArray();

        var result = 0;
        for (var height = 9; height >= 0; height--)
        {
            for (var j = 0; j < length; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    if (map[j][i] != height) continue;

                    if (height is 9)
                    {
                        sets[j][i].Add((j, i));
                        continue;
                    }

                    (int j, int i)[] adj = [(j + 1, i), (j, i + 1), (j - 1, i), (j, i - 1)];

                    foreach (var item in adj.Where(_ => _.i >= 0 && _.j >= 0 && _.i < width && _.j < length))
                    {
                        if (map[item.j][item.i] == height + 1)
                        {
                            sets[item.j][item.i].ForEach(_ => sets[j][i].Add(_));
                        }
                    }

                    if (height is 0)
                    {
                        result += sets[j][i].Count;
                    }
                }
            }
        }

        return result;
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var map = input
            .Select(_ => _.Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray();

        var length = map.Length;
        var width = map[0].Length;

        var sets = Enumerable.Range(0, length)
            .Select(_ => Enumerable.Repeat(0, width).ToArray())
            .ToArray();

        var result = 0;
        for (var height = 9; height >= 0; height--)
        {
            for (var j = 0; j < length; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    if (map[j][i] != height) continue;

                    if (height is 9)
                    {
                        sets[j][i] ++;
                        continue;
                    }

                    (int j, int i)[] adj = [(j + 1, i), (j, i + 1), (j - 1, i), (j, i - 1)];

                    foreach (var item in adj.Where(_ => _.i >= 0 && _.j >= 0 && _.i < width && _.j < length))
                    {
                        if (map[item.j][item.i] == height + 1)
                        {
                            sets[j][i] += sets[item.j][item.i];
                        }
                    }

                    if (height is 0)
                    {
                        result += sets[j][i];
                    }
                }
            }
        }

        return result;
    }
}
