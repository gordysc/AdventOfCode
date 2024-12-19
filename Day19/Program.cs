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
        var patterns = input[0]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToList();
        
        var designs = input[2..];
        
        var worker = new Worker(patterns);
        var count = worker.CountArrangements(designs.ToList());

        return Task.FromResult(count.ToString());
    }
}

internal sealed class Worker(List<string> patterns)
{
    private readonly Dictionary<string, bool> _possibilities = new();
    private readonly Dictionary<string, long> _arrangements = new();
    
    public int CountPossibilities(List<string> designs)
    {
        return designs.Count(design => IsPossible(design, 0));
    }

    public long CountArrangements(List<string> designs)
    {
        return designs.Sum(design => CountArrangements(design, 0));
    }
    
    private long CountArrangements(string design, int offset)
    {
        if (offset == design.Length)
            return 1;
        
        var remaining = design[offset..];

        if (_arrangements.TryGetValue(remaining, out var result))
            return result;
        
        var count = 0L;
        
        foreach (var pattern in patterns)
        {
            if (remaining.StartsWith(pattern) is false)
                continue;

            count += CountArrangements(design, offset + pattern.Length);
        }

        _arrangements[remaining] = count;
        
        return count;
    }
    
    private bool IsPossible(string design, int offset)
    {
        if (offset == design.Length)
            return true;
        
        var remaining = design[offset..];
        
        if (_possibilities.TryGetValue(remaining, out var result))
            return result;
        
        foreach (var pattern in patterns)
        {
            if (remaining.StartsWith(pattern) is false)
                continue;

            if (IsPossible(design, offset + pattern.Length) is false)
                continue;

            _possibilities[remaining] = true;
            
            break;
        }
        
        _possibilities.TryAdd(remaining, false);
        
        return _possibilities[remaining];
    }
}