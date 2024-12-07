using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day7 : Day<Day7>, IDay<Day7>
{
    public static int DayNumber => 7;

    public static object SolvePart1(ImmutableArray<string> input) => Solve(input,
        _ => _.Aggregate((a, b) => (a is 1 || b is 1) ? a * b : a + b),
        _ => _.Aggregate((a, b) => (a is 1 || b is 1) ? a + b : a * b),
        (a, b) => [a + b, a * b]);

    public static object SolvePart2(ImmutableArray<string> input) => Solve(input,
        _ => _.Aggregate((a, b) => (a is 1 || b is 1) ? a * b : a + b),
        _ => long.Parse(string.Concat(_.Select(_ => _.ToString()))),
        (a, b) => [a + b, a * b, long.Parse($"{a}{b}")]);

    public static long Solve(
        ImmutableArray<string> input,
        Func<long[], long> lowerBound,
        Func<long[], long> upperBound,
        Func<long, long, long[]> operators
    )
    {
        var equations = input.Select(_ => _
            .Split(':')
            .Fold((a, b) => (
                Target: long.Parse(a),
                Numbers: b[1..].Split(' ').Select(long.Parse).ToArray())
            ));

        bool CanSolveLine((long Target, long[] Numbers) equation)
        {
            var (target, numbers) = equation;

            bool Test(int index, long current)
            {
                if (index == numbers.Length) return current == target;

                if (target < lowerBound([current, .. numbers[index..]])) return false;
                if (target > upperBound([current, .. numbers[index..]])) return false;

                return operators(current, numbers[index])
                    .Any(_ => Test(index + 1, _));
            }

            return Test(1, numbers[0]);
        }

        return equations.Where(CanSolveLine).Sum(_ => _.Target);
    }
}
