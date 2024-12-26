using System.Text;
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

        var answer = ExecuteCircuit(wires, gates);
        
        return Task.FromResult(answer.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var (_, gates) = ParseCircuit(input);
        var invalid = new HashSet<string>();

        foreach (var gate in gates)
        {
            // Rule #1 - all z outputs except z45 must have an xor gate (z45 would be the carry bit)
            if (gate.Output.StartsWith('z') && gate.Output != "z45" && gate.Operation != "XOR")
                invalid.Add(gate.Output);

            // Rule #2 - Only x,y inputs or z outputs can have an XOR gate
            var isX = gate.Input1.StartsWith('x') || gate.Input2.StartsWith('x') || gate.Output.StartsWith('x');
            var isY = gate.Input1.StartsWith('y') || gate.Input2.StartsWith('y') || gate.Output.StartsWith('y');
            var isZ = gate.Input1.StartsWith('z') || gate.Input2.StartsWith('z') || gate.Output.StartsWith('z');
            
            if (gate.Operation == "XOR" && !isX && !isY && !isZ)
                invalid.Add(gate.Output);

            // Rule #3 - if a gate is an AND operation it must feed into an OR gate
            if (gate.Operation == "AND" && gate.Input1 != "x00" && gate.Input2 != "x00")
            {
                foreach (var other in gates.Except([gate]))
                {
                    if ((gate.Output == other.Input1 || gate.Output == other.Input2) && other.Operation != "OR")
                        invalid.Add(gate.Output);
                }
            }
            
            // Rule #4 - if a gate is an XOR operation it cannot feed into an OR gate
            if (gate.Operation == "XOR")
            {
                foreach (var other in gates.Except([gate]))
                {
                    if ((gate.Output == other.Input1 || gate.Output == other.Input2) && other.Operation == "OR")
                        invalid.Add(gate.Output);
                }
            }
        }

        var answer = string.Join(',', invalid.Order());
        
        return Task.FromResult(answer);
    }

    private static long ExecuteCircuit(Dictionary<string, int> wires, List<Gate> gates)
    {
        var queue = new Queue<Gate>();
        
        foreach (var gate in gates)
            queue.Enqueue(gate);

        while (queue.TryDequeue(out var gate))
        {
            if (wires[gate.Input1] == -1 || wires[gate.Input2] == -1)
            {
                queue.Enqueue(gate);
            }
            else if (wires[gate.Output] == -1)
            {
                wires[gate.Output] = gate.Operation switch
                {
                    "AND" => wires[gate.Input1] & wires[gate.Input2],
                    "XOR" => wires[gate.Input1] ^ wires[gate.Input2],
                    "OR" => wires[gate.Input1] | wires[gate.Input2],
                    _ => throw new ArgumentOutOfRangeException()
                };   
            }
        }
        
        var bits = wires.Keys
            .Where(x => x.StartsWith('z')).OrderDescending()
            .Select(x => wires[x]);
        
        return Convert.ToInt64(string.Join("", bits), 2);
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

internal readonly record struct Gate(string Input1, string Input2, string Operation, string Output)
{
    public override string ToString()
    {
        return string.CompareOrdinal(Input1, Input2) < 0 
            ? $"{Input1} {Operation} {Input2} -> {Output}" 
            : $"{Input2} {Operation} {Input1} -> {Output}";
    }
}