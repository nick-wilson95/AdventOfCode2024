namespace AdventOfCode2024.Days;

public record Day12 : Day<Day12>, IDay<Day12>
{
    public static int DayNumber => 12;

    public static object SolvePart1(ImmutableArray<string> input) => Solve(input, (_, _, _) => true);

    public static object SolvePart2(ImmutableArray<string> input) => Solve(input,
        (ImmutableArray<string> input, (int j, int i) item, (int j, int i) adj) =>
        {
            bool Match((int j, int i) square) => InBounds(input, square) && input[square.j][square.i] == input[item.j][item.i];

            var precedent = (j: item.j + (item.i - adj.i), i: item.i + (item.j - adj.j));
            var diagonal = (j: adj.j + (item.i - adj.i), i: adj.i + (item.j - adj.j));

            return !Match(precedent) || Match(diagonal);
        });

    public static object Solve(ImmutableArray<string> input, Func<ImmutableArray<string>, (int j, int i), (int j, int i), bool> bumpPerimeter)
    {
        var hasChecked = input
            .Select(_ => _.Select(_ => false).ToArray())
            .ToArray();

        var result = 0;
        for (var j = 0; j < input.Length; j++)
        {
            for (var i = 0; i < input[0].Length; i++)
            {
                if (hasChecked[j][i]) continue;

                var area = 0;
                var perimeter = 0;

                var queue = new Queue<(int j, int i)>();
                queue.Enqueue((j, i));

                while (queue.Count > 0)
                {
                    var (y, x) = queue.Dequeue();

                    if (hasChecked[y][x]) continue;

                    area++;
                    hasChecked[y][x] = true;

                    (int j, int i)[] adj = [(y + 1, x), (y, x + 1), (y - 1, x), (y, x - 1)];

                    foreach (var item in adj)
                    {
                        if (InBounds(input, item) && input[item.j][item.i] == input[y][x])
                        {
                            queue.Enqueue(item);
                        }
                        else if (bumpPerimeter(input, (y, x), item))
                        {
                            perimeter++;
                        }
                    }
                }

                result += area * perimeter;
            }
        }

        return result;
    }

    private static bool InBounds(ImmutableArray<string> input, (int j, int i) item) =>
        item.i >= 0 && item.j >= 0 && item.i < input[0].Length && item.j < input.Length;
}