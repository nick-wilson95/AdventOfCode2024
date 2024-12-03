using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day3 : Day<Day3>, IDay<Day3>
{
    public static int DayNumber => 3;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var concat = string.Join(" ", input);
        return Solve(concat);
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var concat = string.Join(" ", input);
        var dos = concat.Split("do()");

        var result = 0;
        foreach (var @do in dos)
        {
            result += Solve(@do.Split("don't()")[0]);
        }

        return result;
    }

    public static int Solve(string input)
    {
        var result = 0;

        var parts = input.Split("mul(").Skip(1);
        foreach (var part in parts)
        {
            var nums = part.TakeUntil(_ => _ is ')');
            if (nums.Last() is not ')') continue;

            var numerics = nums.SkipLast(1).Split(',').ToArray();
            if (numerics.Length != 2) continue;

            if (!int.TryParse(new string(numerics[0].ToArray()), out var num1)) continue;
            if (!int.TryParse(new string(numerics[1].ToArray()), out var num2)) continue;

            result += num1 * num2;
        }

        return result;
    }
}
