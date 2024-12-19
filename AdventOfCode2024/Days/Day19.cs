using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day19 : Day<Day19>, IDay<Day19>
{
    public static int DayNumber => 19;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var (elements, targets) = ParseInput(input);

        var groups = elements.GroupBy(_ => _.Length)
            .OrderBy(_ => _.Key);

        var relements = groups.First().ToList();
        foreach (var group in groups)
        {
            var nonConstructable = group.Where(_ => !CanConstruct(_, relements));
            relements.AddRange(nonConstructable);
        }

        return targets.Count(_ => CanConstruct(_, relements));
    }

    private static bool CanConstruct(string target, List<string> elements)
    {
        foreach (var element in elements)
        {
            if (element.Length > target.Length) continue;
            if (element == target) return true;
            if (target[..element.Length] == element)
            {
                if (CanConstruct(target[element.Length..], elements)) return true;
            }
        }

        return false;
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var (elements, targets) = ParseInput(input);

        Dictionary<string, long> cache = [];
        long CountWays(string target)
        {
            if (target.Length is 0) return 1;

            if (cache.TryGetValue(target, out var value)) return value;

            var result = elements
                .Where(_ => _.Length <= target.Length)
                .Where(_ => target[.._.Length] == _)
                .Sum(_ => CountWays(target[_.Length..]));

            cache.Add(target, result);
            return result;
        }

        return targets.Sum(CountWays);
    }

    private static (List<string> Elements, IEnumerable<string> Targets) ParseInput(ImmutableArray<string> input) =>
        input.Split("").Fold((a, b) => (
            a.Single().Split(", ").Distinct().ToList(),
            b));
}
