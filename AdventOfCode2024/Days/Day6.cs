using System.Diagnostics;

namespace AdventOfCode2024.Days;

public record Day6 : Day<Day6>, IDay<Day6>
{
    public static int DayNumber => 6;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var (map, start) = ParseInput(input);
        return GetVisited(map, start).Count;
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var (map, start) = ParseInput(input);
        map[start.j][start.i] = '.';

        bool Test((int j, int i) objectPos)
        {
            var (j, i, direction) = (start.j, start.i, 0);

            HashSet<(int, int, int)> visited = [];
            while (true)
            {
                if (visited.Contains((j, i, direction))) return true;

                visited.Add((j, i, direction));

                var (jNext, iNext) = direction switch
                {
                    0 => (j - 1, i),
                    1 => (j, i + 1),
                    2 => (j + 1, i),
                    3 => (j, i - 1),
                    _ => throw new UnreachableException()
                };

                if (jNext < 0 || jNext >= map.Length || iNext < 0 || iNext >= map[0].Length) return false;

                var nextChar = map[jNext][iNext];

                if (nextChar is '#' || (jNext, iNext) == objectPos)
                {
                    direction = (direction + 1) % 4;
                }
                else
                {
                    (j, i) = (jNext, iNext);
                }
            }
        }

        return GetVisited(map, start)
            .Where(_ => _ != start)
            .Count(Test);
    }

    public static HashSet<(int j, int i)> GetVisited(char[][] map, (int, int) start)
    {
        var (j, i) = start;
        var direction = 0;

        HashSet<(int, int)> visited = [];
        while (true)
        {
            visited.Add((j, i));

            var (jNext, iNext) = direction switch
            {
                0 => (j - 1, i),
                1 => (j, i + 1),
                2 => (j + 1, i),
                3 => (j, i - 1),
                _ => throw new UnreachableException()
            };

            if (jNext < 0 || jNext >= map.Length || iNext < 0 || iNext >= map[0].Length) break;

            if (map[jNext][iNext] is '#')
            {
                direction = (direction + 1) % 4;
            }
            else
            {
                (j, i) = (jNext, iNext);
            }
        }

        return visited;
    }

    private static (char[][] Map, (int j, int i) Start) ParseInput(ImmutableArray<string> input)
    {
        var map = input.Select(_ => _.ToArray()).ToArray();

        for (var j = 0; j < map.Length; j++)
        {
            for (var i = 0; i < map.Length; i++)
            {
                if (map[j][i] is '^')
                {
                    return (map, (j, i));
                }
            }
        }

        throw new UnreachableException();
    }
}
