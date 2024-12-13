using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day13 : Day<Day13>, IDay<Day13>
{
    public static int DayNumber => 13;

    private record Machine((double x, double y) A, (double x, double y) B, (double x, double y) P);

    public static object SolvePart1(ImmutableArray<string> input) => Solve(input, 0);

    public static object SolvePart2(ImmutableArray<string> input) => Solve(input, 10000000000000);

    public static object Solve(ImmutableArray<string> input, long prizeOffset)
    {
        var machines = input.Split("").Select(_ => _.Fold((a, b, c) =>
        {
            static (long x, long y) Values(string input, long offset = 0) => input
                .Split(": ")[1]
                .Split(", ")
                .Select(_ => long.Parse(_[2..]) + offset)
                .Fold((a, b) => (a, b));

            return new Machine(Values(a), Values(b), Values(c, prizeOffset));
        }));

        static double RequiredTokens(Machine machine)
        {
            var (A, B, P) = machine;

            var t = (P.y - P.x * A.y / A.x) / (B.y - B.x * A.y / A.x);
            var s = (P.x - B.x * t) / A.x;

            var sRound = Math.Round(s);
            var tRound = Math.Round(t);

            if (sRound * tRound < 0) return 0;
            if (Math.Abs(s - sRound) > 10e-4) return 0;
            if (Math.Abs(t - tRound) > 10e-4) return 0;

            return sRound * 3 + tRound;
        }

        return machines.Sum(RequiredTokens);
    }
}
