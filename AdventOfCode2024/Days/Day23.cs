using MoreLinq;
using System.Collections.Frozen;
using System.Diagnostics;

namespace AdventOfCode2024.Days;

public record Day23 : Day<Day23>, IDay<Day23>
{
    public static int DayNumber => 23;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var graph = GetGraph(input);

        var groups = graph.Keys.ToHashSet();
        for (var i = 0; i < 2; i++)
        {
            groups = groups.SelectMany(_ => Expand(_, graph)).ToHashSet();
        }

        return groups.Count(_ => _[0] is 't' || _[2] is 't' || _[4] is 't');
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var graph = GetGraph(input);

        var groups = graph.Keys.ToHashSet();
        while (groups.Count > 1)
        {
            groups = groups.SelectMany(_ => Expand(_, graph)).ToHashSet();
        }

        return groups.Single().Chunk(2).Select(_ => new string(_)).ToDelimitedString(",");
    }

    private static IEnumerable<string> Expand(string group, FrozenDictionary<string, ImmutableHashSet<string>> graph)
    {
        var nodes = Enumerable.Range(0, group.Length / 2)
            .Select(i => group[(i * 2)..((i + 1) * 2)])
            .ToArray();

        return nodes
            .Select(_ => graph[_])
            .Aggregate((a, b) => a.Intersect(b))
            .Select(_ => string.Join("", nodes.Append(_).Order()));
    }

    private static FrozenDictionary<string, ImmutableHashSet<string>> GetGraph(ImmutableArray<string> input)
    {
        var edges = input.Select(_ => _.Split('-'));

        var nodes = edges
            .SelectMany(_ => _)
            .Distinct()
            .ToDictionary(_ => _, _ => new HashSet<string>());

        foreach (var e in edges)
        {
            nodes[e[0]].Add(e[1]);
            nodes[e[1]].Add(e[0]);
        }

        return nodes.ToDictionary(_ => _.Key, _ => _.Value.ToImmutableHashSet()).ToFrozenDictionary();
    }
}
