using MoreLinq;
using System.Diagnostics;

namespace AdventOfCode2024.Days;

public record Day9 : Day<Day9>, IDay<Day9>
{
    public static int DayNumber => 9;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var line = input
            .Single()
            .Select(_ => int.Parse(_.ToString()))
            .ToArray();

        var totalNumbers = line.TakeEvery(2).Sum();

        var numbers = line
            .TakeEvery(2)
            .SelectMany((value, id) => Enumerable.Repeat(id, value));

        var forward = numbers.GetEnumerator();
        var backward = numbers.Reverse().GetEnumerator();

        var seq = line.GetEnumerator();

        long result = 0;
        var pos = 0;
        for (var i = 0; i < line.Length; i++)
        {
            var enumerator = i % 2 is 0
                ? forward : backward;

            for (var j = 0; j < line[i]; j++)
            {
                enumerator.MoveNext();
                result += pos * enumerator.Current;
                pos++;

                if (pos == totalNumbers) return result;
            }
        }

        throw new UnreachableException();
    }
    public static object SolvePart2(ImmutableArray<string> input)
    {
        var line = input
            .Single()
            .Select(_ => int.Parse(_.ToString()))
            .ToArray();

        List<(int id, int position, int length)> numbers = [];
        List<(int position, int length)> gaps = [];

        var pos = 0;
        for (var i = 0; i < line.Length; i++)
        {
            if (i % 2 == 0)
            {
                numbers.Add((i / 2, pos, line[i]));
            }
            else
            {
                gaps.Add((pos, line[i]));
            }

            pos += line[i];
        }

        for (var i = numbers.Count - 1; i > 0; i--)
        {
            var number = numbers[i];
            for (var j = 0; j < gaps.Count; j++)
            {
                var gap = gaps[j];
                if (gap.position > number.position) break;

                if (gap.length >= number.length)
                {
                    numbers[i] = number with { position = gap.position };

                    gaps[j] = (
                        position: gap.position + number.length,
                        length: gap.length - number.length
                    );

                    break;
                }
            }
        }

        return numbers.Sum(_ => (long)_.id * (_.position * _.length + (_.length * (_.length - 1)) / 2));
    }
}
