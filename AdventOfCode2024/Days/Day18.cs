using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day18 : Day<Day18>, IDay<Day18>
{
    public static int DayNumber => 18;

    private const int Size = 71;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var seq = input.Select(_ => _.Split(',').Select(int.Parse).Fold((i, j) => (i, j)));

        var grid = new int[Size, Size];

        CanEscape(seq.Take(1024), out var result);

        return result;
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var seq = input.Select(_ => _.Split(',').Select(int.Parse).Fold((i, j) => (i, j))).ToArray();

        var bounds = (Low: 0, High: seq.Length - 1);
        while (bounds.Low < bounds.High)
        {
            var count = (bounds.Low + bounds.High) / 2 + 1;
            var canEscape = CanEscape(seq.Take(count), out _);

            bounds = canEscape
                ? (count, bounds.High)
                : (bounds.Low, count - 1);
        }

        return seq[bounds.Low];
    }

    public static bool CanEscape(IEnumerable<(int i, int j)> seq, out int minSteps)
    {
        var grid = new int[Size, Size];

        seq.ForEach(_ => grid[_.i, _.j] = -1);

        HashSet<(int i, int j)> Test(IEnumerable<(int j, int i)> positions)
        {
            HashSet<(int i, int j)> next = [];
            foreach (var (i, j) in positions)
            {
                (int, int)[] adj = [(i + 1, j), (i, j + 1), (i - 1, j), (i, j - 1)];
                foreach (var (x, y) in adj)
                {
                    if ((x, y) is (0, 0)) continue;
                    if (x < 0 || x >= Size || y < 0 || y >= Size) continue;
                    
                    var current = grid[x, y];
                    if (current is -1) continue;
                    if (current is 0 || current > grid[i, j])
                    {
                        grid[x, y] = grid[i, j] + 1;
                        next.Add((x, y));
                    }
                }
            }

            return next;
        }

        HashSet<(int i, int j)> toTest = [(0, 0)];
        while (toTest.Count > 0)
        {
            toTest = Test(toTest);
        }

        minSteps = grid[Size - 1, Size - 1];
        return grid[Size - 1, Size - 1] is not 0;
    }
}
