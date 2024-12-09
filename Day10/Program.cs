using Shared;

var input = File.ReadAllText("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 10");

await solution.SolveAsync();

internal sealed class Solution(string input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        return Task.FromResult("Not implemented");
    }

    protected override Task<string> SolvePart2Async()
    {
        return Task.FromResult("Not implemented");
    }
}