using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

solution.Solve();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override string SolvePart1()
    {
        var previous = -1;
        var result = 0;

        foreach (var line in input)
        {
            var value = int.Parse(line);

            if (previous > 0 && value > previous)
                result++;

            previous = value;
        }

        return result.ToString();
    }

    protected override string SolvePart2()
    {
        var previous = -1;
        var result = 0;

        for (var loop = 0; loop < input.Length - 2; loop++)
        {
            var value = input.Skip(loop).Take(3).Select(int.Parse).Sum();
            
            if (previous > 0 && value > previous)
                result++;

            previous = value;
        }

        return result.ToString();
    }
}