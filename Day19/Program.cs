using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 19");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var patterns = input[0]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToList();
        
        var designs = input[2..];
        
        var worker = new Worker(patterns);
        var count = worker.CountPossibilities(designs.ToList());

        return Task.FromResult(count.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        return Task.FromResult("Not implemented");
    }
}

internal sealed class Worker(List<string> patterns)
{
    private readonly Dictionary<string, bool> _cache = new();
    
    public int CountPossibilities(List<string> designs)
    {
        return designs.Count(design => IsPossible(design, 0));
    }
    
    private bool IsPossible(string design, int offset)
    {
        if (offset == design.Length)
            return true;
        
        var remaining = design[offset..];
        
        if (_cache.TryGetValue(remaining, out var result))
            return result;
        
        foreach (var pattern in patterns)
        {
            if (remaining.StartsWith(pattern) is false)
                continue;

            if (IsPossible(design, offset + pattern.Length) is false)
                continue;

            _cache[remaining] = true;
            
            break;
        }
        
        _cache.TryAdd(remaining, false);
        
        return _cache[remaining];
    }
}