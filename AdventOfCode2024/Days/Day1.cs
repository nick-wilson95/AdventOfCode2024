namespace AdventOfCode2024.Days;

public record Day1 : Day<Day1>, IDay<Day1>
{
    public static int DayNumber => 1;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        List<int> listA = [];
        List<int> listB = [];

        foreach (var pair in input.Select(x => x.Split()))
        {
            listA.Add(int.Parse(pair[0]));
            listB.Add(int.Parse(pair[^1]));
        }

        listA.Sort();
        listB.Sort();

        return listA.Zip(listB).Sum(_ => Math.Abs(_.First - _.Second));
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        Dictionary<int, int> countsA = [];
        Dictionary<int, int> countsB = [];

        foreach (var pair in input.Select(x => x.Split()))
        {
            var a = int.Parse(pair[0]);
            countsA.TryAdd(a, 0);
            countsA[a]++;

            var b = int.Parse(pair[^1]);
            countsB.TryAdd(b, 0);
            countsB[b]++;
        }

        return countsA.Sum(_ => _.Key * _.Value * countsB.GetValueOrDefault(_.Key, 0));
    }
}
