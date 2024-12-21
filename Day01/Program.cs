using Shared;

var input = File.ReadAllLines("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 01");

solution.Solve();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override string SolvePart1()
    {
        var values = input.Select(int.Parse).ToList();
        
        for (var i = 0; i < values.Count; i++)
        {
            for (var j = i + 1; j < values.Count; j++)
            {
                if (values[i] + values[j] == 2020)
                {
                    return (values[i] * values[j]).ToString();
                }
            }
        }
        
        return "Not found";
    }
    
    protected override string SolvePart2()
    {
        var values = input.Select(int.Parse).ToList();
        
        for (var i = 0; i < values.Count; i++)
        {
            for (var j = i + 1; j < values.Count; j++)
            {
                for (var k = j + 1; k < values.Count; k++)
                {
                    if (values[i] + values[j] + values[k] == 2020)
                    {
                        return (values[i] * values[j] * values[k]).ToString();
                    }
                }
            }
        }
        
        return "Not found";
    }
}