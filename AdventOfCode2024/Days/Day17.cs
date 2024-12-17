using MoreLinq;

namespace AdventOfCode2024.Days;

public record Day17 : Day<Day17>, IDay<Day17>
{
    public static int DayNumber => 17;

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var (a, b, c, program) = PaseInput(input);
        return string.Join(',', GetOutput(a, b, c, program));
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var parts = input.Split("").ToArray();

        var targetSeq = parts[1]
            .Single()[9..]
            .Split(',')
            .Select(int.Parse)
            .Reverse()
            .ToArray();

        bool TrySolve(long a, int fromIndex, out long newA)
        {
            if (fromIndex == targetSeq.Length)
            {
                newA = a;
                return true;
            }

            for (var i = 0; i < 8; i++)
            {
                // Note that this sequence of steps is specific to my input
                var tryA = a * 8 + i;
                var b = i;
                b ^= 5;
                var c = (tryA >> b) % 8;
                b ^= 6;
                b ^= (int)c;

                if (b == targetSeq[fromIndex])
                {
                    if (TrySolve(tryA, fromIndex + 1, out var result))
                    {
                        newA = result;
                        return true;
                    }
                }
            }

            newA = 0;
            return false;
        }

        TrySolve(0, 0, out var result);

        return result;
    }

    private static (int a, int b, int c, (int code, int operand)[] program) PaseInput(ImmutableArray<string> input)
    {
        var parts = input.Split("").ToArray();

        var (a, b, c) = parts[0]
            .Select(_ => _[12..])
            .Select(int.Parse)
            .Fold((a, b, c) => (a, b, c));

        var program = parts[1]
            .Single()[9..]
            .Split(',')
            .Select(int.Parse)
            .Chunk(2)
            .Select(_ => _.Fold((x, y) => (Code: x, Operand: y)))
            .ToArray();

        return (a, b, c, program);
    }

    private static IEnumerable<long> GetOutput(long a, long b, long c, (int code, int operand)[] program)
    {
        long Combo(int i) => i switch
        {
            4 => a,
            5 => b,
            6 => c,
            _ => i
        };

        for (var i = 0; i < program.Length; i++)
        {
            var (code, operand) = program[i];

            var combo = Combo(operand);

            long Div() => (long)(a / Math.Pow(2, combo));

            if (code is 0) a = Div();
            if (code is 1) b ^= operand;
            if (code is 2) b = combo % 8;
            if (code is 3) i = a is 0 ? i : (operand / 2) - 1;
            if (code is 4) b ^= c;
            if (code is 5) yield return combo % 8;
            if (code is 6) b = Div();
            if (code is 7) c = Div();
        }
    }
}
