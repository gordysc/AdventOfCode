using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 19");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    private readonly Dictionary<string, long> _cache = new();
    
    private readonly List<string> _patterns = input[0]
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Trim())
        .ToList();
        
    private readonly string[] _designs = input[2..];
    
    protected override Task<string> SolvePart1Async()
    {
        return Task.FromResult(Possibilities.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        return Task.FromResult(Arrangements.ToString());
    }
    
    private int Possibilities =>
        _designs.Count(design => CountArrangements(design, 0) > 0);

    private long Arrangements =>
        _designs.Sum(design => CountArrangements(design, 0));
    
    private long CountArrangements(string design, int offset)
    {
        if (offset == design.Length)
            return 1;
        
        var remaining = design[offset..];

        if (_cache.TryGetValue(remaining, out var result))
            return result;
        
        var count = 0L;
        
        foreach (var pattern in _patterns)
        {
            if (remaining.StartsWith(pattern) is false)
                continue;

            count += CountArrangements(design, offset + pattern.Length);
        }

        _cache[remaining] = count;
        
        return count;
    }
}
