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
        
        var computer = new Computer(a, b, c, program);
        var output = computer.Execute();
        
        return Task.FromResult(string.Join(',', output));
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

        while (queue.Count > 0)
        {
            var (value, index) = queue.Dequeue();

            for (var i = 0; i < 8; i++)
            {
                if (answer > 0)
                    continue;
                var a = (value << 3) + i;
                var computer = new Computer(a, b, c, program);
                var result = computer.Execute();

                if (result.SequenceEqual(program[index..]))
                {
                    if (index == 0)
                        answer = a;
                    else
                        queue.Enqueue((a, index - 1));
                }
            }
        }

        return Task.FromResult(answer.ToString());
    }
}

internal sealed class Computer(long a, long b, long c, long[] program)
{
    private long A { get; set; } = a;
    private long B { get; set; } = b;
    private long C { get; set; } = c;
    
    private long _pointer;

    public List<long> Execute()
    {
        var output = new List<long>();
        
        while (_pointer + 1 <= program.Length - 1)
        {
            var opcode = GetOpcode();

            if (opcode == 0)
            {
                var operand = GetComboOperand();
        
                var numerator = A;
                var denominator = Math.Pow(2, operand);
        
                A = (long) Math.Floor(numerator / denominator);
                _pointer += 2;
            }
            else if (opcode == 1)
            {
                var operand = GetLiteralOperand();
                
                B ^= operand;
                _pointer += 2;
            }
            else if (opcode == 2)
            {
                var operand = GetComboOperand();

                B = operand % 8;
                _pointer += 2;
            }
            else if (opcode == 3)
            {
                if (A == 0)
                    _pointer += 2;
                else
                    _pointer = GetLiteralOperand();
            }
            else if (opcode == 4)
            {
                B ^= C;
                _pointer += 2;
            }
            else if (opcode == 5)
            {
                var operand = GetComboOperand();
                var result = operand % 8;
                
                output.Add(result);
                _pointer += 2;
            }
            else if (opcode == 6)
            {
                var operand = GetComboOperand();
        
                B = (long) Math.Floor(A / Math.Pow(2, operand));
                _pointer += 2;
            }
            else if (opcode == 7)
            {
                var operand = GetComboOperand();
        
                C = (long) Math.Floor(A / Math.Pow(2, operand));
                _pointer += 2;
            }
        }

        return output;
    }
    
    private long GetOpcode() => program[_pointer];
    
    private long GetLiteralOperand() => program[_pointer + 1];

    private long GetComboOperand()
    {
        return program[_pointer + 1] switch
        {
            < 4 => program[_pointer + 1],
            4 => A,
            5 => B,
            6 => C,
            _ => throw new InvalidOperationException("Invalid operand")
        };
    }
}
