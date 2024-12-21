using Shared;

var input = File.ReadAllLines("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 01");

solution.Solve();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override string SolvePart1()
    {
        return "Not implemented";
    }
    
    protected override string SolvePart2()
    {
        return "Not implemented";
    }
}