namespace AdventOfCode2024.Days;

public record Day0 : Day<Day0>, IDay<Day0>
{
    public static int DayNumber => 0;

    public static object SolvePart1(ImmutableArray<string> input) => input[0];

    public static object SolvePart2(ImmutableArray<string> input) => "Solved";
}
