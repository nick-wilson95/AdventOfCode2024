using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day21 : Day<Day21>, IDay<Day21>
{
    public static int DayNumber => 21;

    public static object SolvePart1(ImmutableArray<string> input) => Solve(input, 2);

    public static object SolvePart2(ImmutableArray<string> input) => Solve(input, 25);

    private static long Solve(ImmutableArray<string> input, int maxDepth) => input
        .Sum(_ => int.Parse(_[..^1]) * MinSeq([], _, maxDepth));

    private static long MinSeq(Dictionary<(int, int, uint), long> cache, string code, int maxDepth)
    {
        long Steps(int a, int b, uint depth)
        {
            var x = b % 3 - a % 3;
            var y = b / 3 - a / 3;

            if (depth == maxDepth) return Math.Abs(x) + Math.Abs(y) + 1;

            static IEnumerable<int> Repeat(int count, int neg, int pos) =>
                Enumerable.Repeat(count < 0 ? neg : pos, Math.Abs(count));

            var xSteps = Repeat(x, 0, 2);
            var ySteps = Repeat(y, 1, 4);

            long XFirst() => Pass([5, .. xSteps, .. ySteps, 5], depth + 1);
            long YFirst() => Pass([5, .. ySteps, .. xSteps, 5], depth + 1);

            return (depth, a, b) switch
            {
                (0, var k, < 3) when k % 3 is 0 => XFirst(),
                (0, < 3, var k) when k % 3 is 0 => YFirst(),
                (0, _, _) => Math.Min(XFirst(), YFirst()),

                (> 0, 0, > 3) => XFirst(),
                (> 0, > 3, 0) => YFirst(),
                (> 0, _, _) => Math.Min(XFirst(), YFirst()),
            };
        }

        long CachedSteps(int a, int b, uint depth)
        {
            var key = (a, b, depth);

            if (!cache.TryGetValue(key, out long value))
            {
                value = Steps(a, b, depth);
                cache[key] = value;
            }

            return value;
        }

        long Pass(IEnumerable<int> input, uint depth) => input
            .Pairwise((a, b) => CachedSteps(a, b, depth))
            .Sum();

        var initial = code.Select(_ => _ switch
        {
            '0' => 1,
            'A' => 2,
            _ => int.Parse(_.ToString()) + 2
        });

        return Pass([2, ..initial], 0);
    }
}
