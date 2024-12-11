namespace AdventOfCode2024.Days;

public record Day11 : Day<Day11>, IDay<Day11>
{
    public static int DayNumber => 11;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var current = input.Single().Split(' ').Select(long.Parse).ToList();
        List<long> next = [];

        for (var i = 0; i < 25; i++)
        {
            foreach (var stone in current)
            {
                if (stone is 0)
                {
                    next.Add(1);
                    continue;
                }

                var log = (long)Math.Log(stone, 10);

                if (log % 2 is 1)
                {
                    var mod = (long)Math.Pow(10, (log + 1) / 2);
                    next.Add(stone / mod);
                    next.Add(stone % mod);
                }
                else
                {
                    next.Add(stone * 2024);
                }
            }

            current = next;
            next = [];
        }

        return current.Count;
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var initial = input.Single().Split(' ').Select(long.Parse).ToList();

        var cache = new Dictionary<(int, long), long>();

        long GetNumStones(int blinks, long stone)
        {
            if (blinks is 0) return 1;

            if (cache.ContainsKey((blinks, stone))) return cache[(blinks, stone)];

            long Cache(long value)
            {
                cache.Add((blinks, stone), value);
                return value;
            }

            if (stone is 0) return Cache(GetNumStones(blinks - 1, 1));

            var log = (long)Math.Log(stone, 10);
            if (log % 2 is 1)
            {
                var mod = (long)Math.Pow(10, (log + 1) / 2);
                return Cache(GetNumStones(blinks - 1, stone / mod)
                    + GetNumStones(blinks - 1, stone % mod));
            }
            else
            {
                return Cache(GetNumStones(blinks - 1, stone * 2024));
            }
        }

        return initial.Sum(_ => GetNumStones(75, _));
    }
}
