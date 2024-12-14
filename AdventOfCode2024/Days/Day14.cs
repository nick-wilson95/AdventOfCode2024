using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day14 : Day<Day14>, IDay<Day14>
{
    public static int DayNumber => 14;

    private const int Width = 101;
    private const int Height = 103;
    private static (int i, int j) Vector(string input) => input[2..].Split(',').Select(int.Parse).Fold((i, j) => (i, j));

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var robots = input.Select(_ => _.Split(' ').Fold((p, v) => (P: Vector(p), V: Vector(v))));

        var quads = new int[4];

        foreach (var (P, V) in robots)
        {
            var x = (P.i + 100 * V.i) % Width;
            var y = (P.j + 100 * V.j) % Height;

            if (x < 0) x += Width;
            if (y < 0) y += Height;

            var quad = (x - Width / 2, y - Height / 2) switch
            {
                ( < 0, < 0) => 0,
                ( > 0, < 0) => 1,
                ( < 0, > 0) => 2,
                ( > 0, > 0) => 3,
                _ => -1
            };

            if (quad != -1) quads[quad]++;
        }

        return quads.Aggregate((a, b) => a * b);
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var robots = input.Select(_ => _.Split(' ').Fold((p, v) => (P: Vector(p), V: Vector(v))))
            .ToArray();

        var grid = Enumerable.Range(0, Height)
            .Select(_ => Enumerable.Repeat(' ', Width).ToArray())
            .ToArray();

        for (var second = 0; true; second ++)
        {
            foreach (var (P, V) in robots)
            {
                grid[P.j][P.i] = '#';
            }

            foreach (var row in grid)
            {
                Console.WriteLine(new string(row));
            }

            Console.WriteLine($"After {second} seconds. Press any key to advance 1 second...");
            Console.ReadKey();
            Console.Clear();

            for (var i = 0; i < robots.Length; i++)
            {
                var (P, V) = robots[i];

                grid[P.j][P.i] = ' ';

                var x = (P.i + V.i) % Width;
                var y = (P.j + V.j) % Height;

                if (x < 0) x += Width;
                if (y < 0) y += Height;

                robots[i] = ((x, y), V);
            }

            // By inspection, there is a pattern in y at 28 (+ 103n) and a pattern in x at 55 (+ 101n)
            // The tree must occur when the patterns coincide; in particular - when n satisfies:
            // n = 28 % 103
            // n = 55 % 101
            // By mathematics, 6620 is the lowest such n
            return 6620;
        }
    }
}
