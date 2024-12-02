using Shared;

var input = File.ReadAllLines("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 01");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var (left, right) = ParseInput();
        
        left.Sort();
        right.Sort();

        var total = 0;

        for (var loop = 0; loop < left.Count; loop++)
            total += Math.Abs(left[loop] - right[loop]);
        
        return Task.FromResult(total.ToString());
    }
    
    protected override Task<string> SolvePart2Async()
    {
        var (left, right) = ParseInput();
        
        var counts = right
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        var total = 0;

        foreach (var l in left)
        {
            if (counts.TryGetValue(l, out var count))
                total += l * count;
        }
        
        return Task.FromResult(total.ToString());
    }
    
    private (List<int>, List<int>) ParseInput()
    {
        var left = new List<int>(input.Length);
        var right = new List<int>(input.Length);

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var entries = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (entries.Length != 2)
                throw new InvalidDataException("Invalid input");
            
            left.Add(int.Parse(entries[0]));
            right.Add(int.Parse(entries[1]));
        }
        
        return (left, right);
    }
}