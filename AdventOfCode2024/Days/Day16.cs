using System.Diagnostics;

namespace AdventOfCode2024.Days;

public record Day16 : Day<Day16>, IDay<Day16>
{
    public static int DayNumber => 16;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var junctions = GetJunctions(input, out var start, out var end);

        Dictionary<(int j, int i, int dir), int> shortestDistances = [];
        shortestDistances[(start.j, start.i, 0)] = 0;

        void AddIfBetter(int j, int i, int dir, int value)
        {
            var key = (j, i, dir);
            if (!shortestDistances.TryGetValue(key, out var current) || current > value)
            {
                shortestDistances[key] = value;
            }
        }

        var best = int.MaxValue;
        HashSet<(int j, int i, int dir)> processed = [];
        while (true)
        {
            var min = shortestDistances
                .Where(_ => !processed.Contains(_.Key))
                .MinBy(_ => _.Value);

            processed.Add(min.Key);

            var ((j, i, dir), value) = min;

            if (min.Value >= best) return best;

            AddIfBetter(j, i, (dir + 3) % 4, value + 1000);
            AddIfBetter(j, i, (dir + 1) % 4, value + 1000);

            (j, i) = MoveDir(j, i, dir);
            value++;

            if (input[j][i] is '#') continue;

            while (true)
            {
                if ((j, i) == end)
                {
                    best = Math.Min(best, value);
                    break;
                }

                if (junctions.Contains((j, i)))
                {
                    AddIfBetter(j, i, dir, value);
                    break;
                }

                var deadend = true;
                for (var k = 3; k < 6; k++)
                {
                    var test = MoveDir(j, i, (dir + k) % 4);

                    if (input[test.j][test.i] is '#') continue;

                    deadend = false;
                    value += k is 4 ? 1 : 1001;
                    dir = (dir + k) % 4;
                    (j, i) = test;
                    break;
                }

                if (deadend) break;
            }
        }
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var junctions = GetJunctions(input, out var start, out var end);

        Dictionary<(int j, int i, int dir), (int best, ImmutableHashSet<(int j, int i)> visited)> shortestDistances = [];
        shortestDistances[(start.j, start.i, 0)] = (0, []);

        void AddIfBetter(int j, int i, int dir, int value, ImmutableHashSet<(int j, int i)> visited)
        {
            var key = (j, i, dir);
            if (!shortestDistances.TryGetValue(key, out var current) || current.best > value)
            {
                shortestDistances[key] = (value, visited);
            }
            else if (current.best == value)
            {
                shortestDistances[key] = (value, current.visited.Union(visited));
            }
        }

        var bestValue = int.MaxValue;
        ImmutableHashSet<(int j, int i)> bestVisited = [];
        HashSet<(int j, int i, int dir)> processed = [];
        while (true)
        {
            var min = shortestDistances
                .Where(_ => !processed.Contains(_.Key))
                .MinBy(_ => _.Value.best);

            processed.Add(min.Key);

            var ((j, i, dir), (value, visited)) = min;

            if (value > bestValue)
            {
                return bestVisited.Count;
            }

            AddIfBetter(j, i, (dir + 3) % 4, value + 1000, visited);
            AddIfBetter(j, i, (dir + 1) % 4, value + 1000, visited);

            HashSet<(int j, int i)> extraVisited = [(j, i)];
            (j, i) = MoveDir(j, i, dir);
            value++;

            if (input[j][i] is '#') continue;
            while (true)
            {
                extraVisited.Add((j, i));

                if ((j, i) == end)
                {
                    if (value < bestValue)
                    {
                        bestValue = value;
                        bestVisited = visited.Union(extraVisited);
                    }
                    if (value == bestValue)
                    {
                        bestVisited = bestVisited.Union(visited).Union(extraVisited);
                    }
                    break;
                }

                if (junctions.Contains((j, i)))
                {
                    AddIfBetter(j, i, dir, value, visited.Union(extraVisited));
                    break;
                }

                var deadend = true;
                for (var k = 3; k < 6; k++)
                {
                    var test = MoveDir(j, i, (dir + k) % 4);

                    if (input[test.j][test.i] is '#') continue;

                    deadend = false;
                    value += k is 4 ? 1 : 1001;
                    dir = (dir + k) % 4;
                    (j, i) = test;
                    break;
                }

                if (deadend) break;
            }
        }
    }

    private static HashSet<(int j, int i)> GetJunctions(ImmutableArray<string> input, out (int j, int i) start, out (int j, int i) end)
    {
        (start, end) = (default, default);

        HashSet<(int, int)> junctions = [];
        for (var j = 0; j < input.Length; j++)
        {
            for (var i = 0; i < input[0].Length; i++)
            {
                if (input[j][i] is 'S') start = (j, i);
                if (input[j][i] is 'E') end = (j, i);

                if (input[j][i] is not '.') continue;

                char[] adj = [input[j + 1][i], input[j - 1][i], input[j][i + 1], input[j][i - 1]];
                if (adj.Count(_ => _ is '.') > 2) junctions.Add((j, i));
            }
        }

        return junctions;
    }

    private static (int j, int i) MoveDir(int j, int i, int dir) => dir switch
    {
        0 => (j, i + 1),
        1 => (j + 1, i),
        2 => (j, i - 1),
        3 => (j - 1, i),
        _ => throw new UnreachableException()
    };
}
