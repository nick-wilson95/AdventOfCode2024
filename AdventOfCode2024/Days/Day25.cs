using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day25 : Day<Day25>, IDay<Day25>
{
    public static int DayNumber => 25;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var (a, b) = input.Split("").Partition(_ => _.First() is "#####");

        static int[] Parse(IEnumerable<string> strings) => strings
            .Transpose()
            .Select(_ => _.Count(_ => _ is '#') - 1)
            .ToArray();

        var locks = a.Select(Parse).ToArray();
        var keys = b.Select(Parse).ToArray();

        var result = 0;
        for (var i = 0; i < locks.Length; i++)
        {
            var @lock = locks[i];
            for (var j = 0; j < keys.Length; j++)
            {
                var key = keys[j];
                if (@lock.Zip(key).All(_ => _.First + _.Second <= 5)) result++;
            }
        }

        return result;
    }
}
