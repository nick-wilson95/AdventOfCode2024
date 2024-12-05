using MoreLinq;
using System.Collections.Frozen;

namespace AdventOfCode2024.Days;

public record Day5 : Day<Day5>, IDay<Day5>
{
    public static int DayNumber => 5;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var (comesAfter, updates) = ParseInput(input);

        bool IsOrderedCorrectly(int[] update)
        {
            for (var i = 0; i < update.Length - 1; i++)
            {
                for (var j = i + 1; j < update.Length; j++)
                {
                    if (comesAfter.TryGetValue(update[i], out var values) && values.Contains(update[j])) return false;
                }
            }

            return true;
        }

        return updates.Where(IsOrderedCorrectly).Sum(_ => _[_.Length / 2]);
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var (comesAfter, updates) = ParseInput(input);

        bool Order(int[] update)
        {
            var modified = false;
            for (var i = 0; i < update.Length - 1; i++)
            {
                for (var j = i + 1; j < update.Length; j++)
                {
                    if (comesAfter.TryGetValue(update[i], out var values) && values.Contains(update[j]))
                    {
                        (update[j], update[i]) = (update[i], update[j]);
                        modified = true;
                        j = i;
                    }
                }
            }

            return modified;
        }

        return updates.Where(Order).Sum(_ => _[_.Length / 2]);
    }

    public static (FrozenDictionary<int, ImmutableHashSet<int>> ComesAfter, IEnumerable<int[]> Updates) ParseInput(ImmutableArray<string> input)
    {
        var parts = input.Split("").ToArray();
        var rules = parts[0].Select(_ => _.Split('|').Select(int.Parse).Fold((a, b) => (a, b)));
        var comesAfter = rules.GroupBy(_ => _.b)
            .ToFrozenDictionary(_ => _.Key, _ => _.Select(_ => _.a).ToImmutableHashSet());

        var updates = parts[1].Select(_ => _.Split(',').Select(int.Parse).ToArray());

        return (comesAfter, updates);
    }
}
