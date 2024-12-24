using System.Text.RegularExpressions;
using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 24");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var (wires, gates) = ParseCircuit(input);
        var queue = new Queue<Gate>();
        
        foreach (var gate in gates)
            queue.Enqueue(gate);

        while (queue.TryDequeue(out var gate))
        {
            if (wires[gate.Wire1] == -1 || wires[gate.Wire2] == -1)
            {
                queue.Enqueue(gate);
            }
            else if (wires[gate.Output] == -1)
            {
                wires[gate.Output] = gate.Operation switch
                {
                    "AND" => (wires[gate.Wire1] == 1 && wires[gate.Wire2] == 1) ? 1 : 0,
                    "XOR" => wires[gate.Wire1] ^ wires[gate.Wire2],
                    "OR" => (wires[gate.Wire1] == 1 || wires[gate.Wire2] == 1) ? 1 : 0,
                    _ => throw new ArgumentOutOfRangeException()
                };   
            }
        }

        var zs = wires.Keys
            .Where(x => x.StartsWith("z")).OrderDescending()
            .Select(x => wires[x]);
        
        var answer = Convert.ToInt64(string.Join("", zs), 2);
        
        return Task.FromResult(answer.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        return Task.FromResult("Not implemented");
    }

    private static (Dictionary<string, int> wires, List<Gate> gates) ParseCircuit(string[] input)
    {
        var wires = new Dictionary<string, int>();
        var gates = new List<Gate>();
        
        var section = 1;

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                section = 2;
                continue;
            }
            
            if (section == 1)
            {
                var entries = line.Split(':', StringSplitOptions.TrimEntries);
                wires.Add(entries[0], int.Parse(entries[1]));
            }
            else
            {
                var values = Regex.Match(line, @"(\w+) (AND|OR|XOR) (\w+) -> (\w+)");
                
                var wire1 = values.Groups[1].Value;
                var operation = values.Groups[2].Value;
                var wire2 = values.Groups[3].Value;
                var output = values.Groups[4].Value;

                wires.TryAdd(wire1, -1);
                wires.TryAdd(wire2, -1);
                wires.TryAdd(output, -1);
                
                gates.Add(new Gate(wire1, wire2, operation, output));
            }
        }

        return (wires, gates);
    }
}
 
internal readonly record struct Gate(string Wire1, string Wire2, string Operation, string Output);