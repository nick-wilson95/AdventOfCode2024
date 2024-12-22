namespace AdventOfCode2024.Days;

public record Day22 : Day<Day22>, IDay<Day22>
{
    public static int DayNumber => 22;

    public static object SolvePart1(ImmutableArray<string> input) => input.Select(long.Parse).Sum(GetSecret);

    private static long GetSecret(long i)
    {
        for (var n = 0; n < 2000; n++) i = Iterate(i);
        return i;
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        Dictionary<(int, int, int, int), int> results = [];
        foreach(var item in input)
        {
            var i = long.Parse(item);
            HashSet<(int, int, int, int)> encountered = [];
            var buffer = new int[4];

            var prev = (int)(i % 10);
            for (var n = 0; n < 2000; n++)
            {
                int B(int k) => buffer[(n + k) % 4];

                i = Iterate(i);
                var lastDigit = (int)(i % 10);
                buffer[n % 4] = lastDigit - prev;
                prev = lastDigit;

                if (n < 4) continue;

                var key = (B(1), B(2), B(3), B(4));
                if (encountered.Contains(key)) continue;

                encountered.Add(key);
                results[key] = results.TryGetValue(key, out var value)
                    ? value + lastDigit
                    : lastDigit;
            }
        }

        return results.Values.Max();
    }

    private static long Iterate(long i)
    {
        i ^= i << 6;
        i %= 16777216;
        i ^= i >> 5;
        i %= 16777216;
        i ^= i << 11;
        i %= 16777216;

        return i;
    }
}
