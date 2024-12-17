using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 17");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var a = long.Parse(input[0].Split(':').Last().Trim());
        var b = long.Parse(input[1].Split(':').Last().Trim());
        var c = long.Parse(input[2].Split(':').Last().Trim());
        var instructions = input[4].Split(':').Last().Trim();
        var program = instructions.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        var result = Execute([a, b, c], program);
        
        return Task.FromResult(string.Join(',', result));
    }

    protected override Task<string> SolvePart2Async()
    {
        var b = long.Parse(input[1].Split(':').Last().Trim());
        var c = long.Parse(input[2].Split(':').Last().Trim());
        var instructions = input[4].Split(':').Last().Trim();
        var program = instructions.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        var queue = new Queue<(long A, int Index)>();
        var answer = -1L;
        
        queue.Enqueue((0, program.Length - 1));

        while (queue.TryDequeue(out var current) && answer < 0)
        {
            for (var i = 0; i < 8; i++)
            {
                var a = (current.A << 3) + i;
                var result = Execute([a, b, c], program);

                if (result.SequenceEqual(program[current.Index..]) is false)
                    continue;

                if (current.Index == 0)
                    answer = a;
                else
                    queue.Enqueue((a, current.Index - 1));
            }
        }

        return Task.FromResult(answer.ToString());
    }

    private static List<long> Execute(long[] registers, long[] program)
    {
        var output = new List<long>();
        var (a, b, c) = (registers[0], registers[1], registers[2]);
        
        for (long pointer = 0; pointer + 1 < program.Length;)
        {
            var (opcode, literal) = (program[pointer], program[pointer + 1]);
            var combo = literal switch { 4 => a, 5 => b, 6 => c, _ => literal };

            pointer = opcode == 3 && a != 0 ? literal : pointer + 2;

            switch (opcode)
            {
                case 0: a = (long)(a / Math.Pow(2, combo)); break;
                case 1: b ^= literal; break;
                case 2: b = combo % 8; break;
                case 4: b ^= c; break;
                case 5: output.Add(combo % 8); break;
                case 6: b = (long)(a / Math.Pow(2, combo)); break;
                case 7: c = (long)(a / Math.Pow(2, combo)); break;
            }
        }
        
        return output;
    }
}
