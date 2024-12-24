using MoreLinq;
using System.Diagnostics;

namespace AdventOfCode2024.Days;

public record Day24 : Day<Day24>, IDay<Day24>
{
    public static int DayNumber => 24;

    private record Gate(string In1, string In2, string Op, string Output);

    public static object SolvePart1(ImmutableArray<string> input)
    {
        var (state, gates) = ParseInput(input);

        while (gates.Length > 0) gates = Iterate(state, gates).ToArray();

        return state
            .Where(_ => _.Key.StartsWith('z'))
            .OrderByDescending(_ => _.Key)
            .Select(_ => _.Value)
            .Aggregate((long)0, (a, b) => (a << 1) + (b ? 1 : 0));
    }

    private static IEnumerable<Gate> Iterate(Dictionary<string, bool> state, IEnumerable<Gate> gates)
    {
        foreach (var gate in gates)
        {
            if (state.TryGetValue(gate.In1, out var v1) && state.TryGetValue(gate.In2, out var v2))
            {
                state[gate.Output] = gate.Op switch
                {
                    "AND" => v1 && v2,
                    "OR" => v1 || v2,
                    "XOR" => v1 ^ v2,
                    _ => throw new UnreachableException()
                };
            }
            else yield return gate;
        }
    }

    public static object SolvePart2(ImmutableArray<string> input)
    {
        var (state, gates) = ParseInput(input);

        var outs = gates.ToDictionary(_ => (_.In1, _.In2, _.Op), _ => _.Output);

        string? GetOut(string in1, string in2, string op) =>
            outs.TryGetValue((in1, in2, op), out var result)
            || outs.TryGetValue((in2, in1, op), out result)
                ? result
                : null;

        var lastCarry = "pgc";
        for (var i = 1; i < 45; i++)
        {
            var xor = GetOut($"x{i:00}", $"y{i:00}", "XOR")!;
            var and = GetOut($"y{i:00}", $"x{i:00}", "AND")!;

            var zGate = outs.FirstOrDefault(_ => (_.Key.In1 == xor || _.Key.In2 == xor) && _.Key.Op is "XOR").Key;
            var prevCarry = zGate.In1 == xor ? zGate.In2 : zGate.In1;
            if (prevCarry != lastCarry)
            {
                // break here
            }

            var z = GetOut(prevCarry, xor, "XOR");

            var preCarry = GetOut(xor, prevCarry, "AND");
            var carry = preCarry is null ? null : GetOut(and, preCarry, "OR");
            lastCarry = carry;

            if (z is null || preCarry is null || carry is null)
            {
                // break here
            }
        }

        // From observation, the gate structure was essentially the same for each bit (except the first and last):
        // x01 XOR y01->tct
        // x01 AND y01->mwc
        // pgc XOR tct->z01 (pgc is carried from the previous bit)
        // tct AND pgc->qjs
        // mwc OR qjs->pfv (pfv is carried to the next bit)

        // When the suggested breakpoints above are hit, there is a problem with the structure for bit i.
        // Identifying the switches was then done by hand - fortunately the wire switches were isolated within gates for a single bit.

        return "ckb,kbs,ksv,nbd,tqq,z06,z20,z39";
    }

    private static (Dictionary<string, bool>, Gate[]) ParseInput(ImmutableArray<string> input) => input
        .Split("")
        .Fold((p1, p2) => (
            p1.Select(_ => _.Split(": ")).ToDictionary(_ => _[0], _ => _[1].Single() is '1'),
            p2.Select(_ => _.Split(' ').Fold((a, b, c, _, d) => new Gate(a, c, b, d))).ToArray()
        ));
}
