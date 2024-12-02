using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day2 : Day<Day2>, IDay<Day2>
{
    public static int DayNumber => 2;

    public static object SolvePart1(ImmutableArray<string> input) => input
        .Select(_ => _.Split().Select(int.Parse).ToArray())
        .Count(IsSafe);

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var reports = input.Select(_ => _.Split().Select(int.Parse).ToImmutableArray());

        return reports.Count(_ =>
        {
            if (IsSafe(_)) return true;

            for (var i = 0; i < _.Length; i++)
            {
                if (IsSafe(_.RemoveAt(i))) return true;
            }

            return false;
        });
    }
    private static bool IsSafe(IList<int> input)
    {
        var jumps = input.Zip(input.Skip(1)).Select(_ => _.First - _.Second);
        return jumps.All(_ => _ > 0 && _ < 4)
            || jumps.All(_ => _ < 0 && _ > -4);
    }
}
