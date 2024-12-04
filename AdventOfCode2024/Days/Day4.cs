namespace AdventOfCode2024.Days;

public record Day4 : Day<Day4>, IDay<Day4>
{
    public static int DayNumber => 4;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var height = input.Length;
        var width = input[0].Length;

        (int Y, int X)[] vectors = [
            (1, 0),
            (1, 1),
            (0, 1),
            (-1, 1),
            (-1, 0),
            (-1, -1),
            (0, -1),
            (1, -1),
        ];

        var count = 0;
        for (var j = 0; j < height; j++)
        {
            for (var i = 0; i < width; i++)
            {
                if (input[j][i] is not 'X') continue;

                foreach (var (Y, X) in vectors)
                {
                    var outOfBounds =
                        j + Y * 3 < 0
                        || j + Y * 3 > height - 1
                        || i + X * 3 < 0
                        || i + X * 3 > width - 1;

                    if (outOfBounds) continue;

                    var match = input[j + Y][i + X] is 'M'
                        && input[j + 2 * Y][i + 2 * X] is 'A'
                        && input[j + 3 * Y][i + 3 * X] is 'S';

                    if (match) count++;
                }
            }
        }

        return count;
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var height = input.Length;
        var width = input[0].Length;

        string[] desiredCorners = [
            "SSMM",
            "MSSM",
            "MMSS",
            "SMMS",
        ];

        var count = 0;
        for (var j = 1; j < height - 1; j++)
        {
            for (var i = 1; i < width - 1; i++)
            {
                if (input[j][i] is not 'A') continue;

                string corners = new([
                    input[j - 1][i - 1],
                    input[j - 1][i + 1],
                    input[j + 1][i + 1],
                    input[j + 1][i - 1]
                ]);

                if (desiredCorners.Contains(corners)) count++;
            }
        }

        return count;
    }
}
