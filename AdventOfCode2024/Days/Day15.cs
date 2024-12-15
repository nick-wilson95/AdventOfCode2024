using MoreLinq;
using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode2024.Days;

public record Day15 : Day<Day15>, IDay<Day15>
{
    public static int DayNumber => 15;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var parts = input.Split("").ToArray();        
        var map = parts[0].Select(_ => _.ToArray()).ToArray();
        var moves = parts[1].SelectMany(_ => _);

        var (y, x) = GetStart(map);

        foreach (var move in moves)
        {
            var (dj, di) = move switch {
                '>' => (0, 1),
                'v' => (1, 0),
                '<' => (0, -1),
                '^' => (-1, 0),
                _ => throw new UnreachableException()
            };

            for (var k = 1; true; k++)
            {
                var item = map[y + k * dj][x + k * di];
                if (item is 'O') continue;
                if (item is '#') break;

                for (var m = k; m >= 1; m--)
                {
                    map[y + m * dj][x + m * di] = map[y + (m - 1) * dj][x + (m - 1) * di];
                }

                map[y][x] = '.';
                (y, x) = (y + dj, x + di);
                break;
            }
        }

        return GetResult(map, 'O');
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var parts = input.Split("").ToArray();
        var map = parts[0].Select(_ => _.SelectMany(_ => _ switch
        {
            '#' => "##",
            'O' => "[]",
            '.' => "..",
            '@' => "@.",
            _ => throw new UnreachableException()
        }).ToArray()).ToArray();

        var moves = parts[1].SelectMany(_ => _);

        var (y, x) = GetStart(map);

        foreach (var move in moves)
        {
            if (move is '>' or '<')
            {
                var di = move is '>' ? 1 : -1;

                for (var k = 1; true; k++)
                {
                    var item = map[y][x + k * di];
                    if (item is '[' or ']') continue;
                    if (item is '#') break;

                    for (var m = k; m >= 1; m--)
                    {
                        map[y][x + m * di] = map[y][x + (m - 1) * di];
                    }

                    map[y][x] = '.';
                    x += di;
                    break;
                }

                continue;
            }

            var dj = move is 'v' ? 1 : -1;
            var movers = GetMovers(map, (y, x), dj);

            if (movers.Count is 0) continue;

            movers.Reverse();

            foreach (var row in movers)
            {
                foreach (var (j, i) in row)
                {
                    map[j + dj][i] = map[j][i];
                    map[j][i] = '.';
                }
            }

            y += dj;
        }

        return GetResult(map, '[');
    }

    private static List<HashSet<(int j, int i)>> GetMovers(char[][] map, (int y, int x) origin, int dj)
    {
        List<HashSet<(int j, int i)>> toMove = [[origin]];
        while (toMove[^1].Count > 0)
        {
            var prev = toMove.Last();

            toMove.Add([]);
            var row = toMove.Last();
            foreach (var (j, i) in prev)
            {
                var next = map[j + dj][i];

                if (next is '#') return [];
                if (next is '.') continue;

                row.Add((j + dj, i));
                row.Add((j + dj, i + (next is ']' ? -1 : 1)));
            }
        }

        return toMove;
    }

    private static (int y, int x) GetStart(char[][] map)
    {
        for (var j = 0; j < map.Length; j++)
        {
            for (var i = 0; i < map[0].Length; i++)
            {
                if (map[j][i] == '@')
                {
                    return (j, i);
                }
            }
        }

        throw new UnreachableException();
    }

    private static int GetResult(char[][] map, char box)
    {
        var result = 0;
        for (var j = 0; j < map.Length; j++)
        {
            for (var i = 0; i < map[0].Length; i++)
            {
                if (map[j][i] == box)
                {
                    result += j * 100 + i;
                }
            }
        }

        return result;
    }
}
