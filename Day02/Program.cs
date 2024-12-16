using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

solution.Solve();

internal sealed class Solution(string[] input) : AbstractSolution
{
    private const string Forward = "forward";
    private const string Up = "up";
    private const string Down = "down";
    
    protected override string SolvePart1()
    {
        var position = 0;
        var depth = 0;

        foreach (var line in input)
        {
            var instructions = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            
            var direction = instructions[0];
            var value = int.Parse(instructions[1]);
            
            switch (direction)
            {
                case Forward:
                    position += value;
                    break;
                case Up:
                    depth -= value;
                    break;
                case Down:
                    depth += value;
                    break;
            }
        }

        var result = position * depth;
        
        return result.ToString();
    }

    protected override string SolvePart2()
    {
        var position = 0;
        var depth = 0;
        var aim = 0;

        foreach (var line in input)
        {
            var instructions = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            
            var direction = instructions[0];
            var value = int.Parse(instructions[1]);
            
            switch (direction)
            {
                case Forward:
                    position += value;
                    depth += aim * value;
                    break;
                case Up:
                    aim -= value;
                    break;
                case Down:
                    aim += value;
                    break;
            }
        }

        var result = position * depth;
        
        return result.ToString();
    }
}