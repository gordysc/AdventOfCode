using System.Text.RegularExpressions;
using Shared;

var input = File.ReadAllText("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 03");

await solution.SolveAsync();

internal sealed class Solution(string input) : AbstractSolution
{
    private static readonly Regex Pattern = new(@"\bmul\((\d+),(\d+)\)");
    
    protected override Task<string> SolvePart1Async()
    {
        var matches = Pattern.Matches(input);

        var total = matches.Aggregate(0, (total, match) =>
        {
            var x = int.Parse(match.Groups[1].Value);
            var y = int.Parse(match.Groups[2].Value);
            
            return total + x * y;
        });
            
        return Task.FromResult(total.ToString());
    }
    
    private static readonly Regex DoPattern = new(@"\bdo\(\)");
    private static readonly Regex DontPattern = new(@"\bdon't\(\)");

    protected override Task<string> SolvePart2Async()
    {
        var matches = Pattern.Matches(input);
        
        var doIndexes = DoPattern.Matches(input).Select(x => x.Index).ToArray();
        var dontIndexes = DontPattern.Matches(input).Select(x => x.Index).ToArray();
        
        var total = matches.Aggregate(0, (total, match) =>
        {
            var doMatch = doIndexes.LastOrDefault(x => x < match.Index);
            var dontMatch = dontIndexes.LastOrDefault(x => x < match.Index);
            
            if (dontMatch > doMatch)
                return total;
            
            var x = int.Parse(match.Groups[1].Value);
            var y = int.Parse(match.Groups[2].Value);
            
            return total + x * y;
        });
            
        return Task.FromResult(total.ToString());
    }
}