using Shared;

var input = File.ReadAllText("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 11");

await solution.SolveAsync();

internal sealed class Solution(string input) : AbstractSolution
{
    private readonly Dictionary<Stone, int> _cache = new();
    
    protected override Task<string> SolvePart1Async()
    {
        var stones = input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new Stone(long.Parse(x), 0))
            .ToList();
            
        var calculator = new StoneCalculator(stones);
        var total = calculator.Calculate(25);
        
        return Task.FromResult(total.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var stones = input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new Stone(long.Parse(x), 0))
            .ToList();
            
        var calculator = new StoneCalculator(stones);
        var total = calculator.Calculate(75);
        
        return Task.FromResult(total.ToString());
    }
}

internal sealed class StoneCalculator(List<Stone> stones)
{
    private readonly Dictionary<Stone, long> _cache = new();

    public long Calculate(int maxDepth)
    {
        return stones.Sum(x => Count(x, maxDepth));
    }
    
    private long Count(Stone stone, int maxDepth)
    {
        if (stone.Depth == maxDepth)
            return 1;

        if (_cache.TryGetValue(stone, out var count))
            return count;

        if (stone.Number == 0)
            _cache[stone] = Count(new Stone(1, stone.Depth + 1), maxDepth);
        else if (stone.Digits.Length % 2 == 1)
            _cache[stone] = Count(new Stone(stone.Number * 2024, stone.Depth + 1), maxDepth);
        else
        {

            var half = stone.Digits.Length / 2;
            var left = long.Parse(stone.Digits[..half]);
            var right = long.Parse(stone.Digits[half..]);

            _cache[stone] = Count(new Stone(left, stone.Depth + 1), maxDepth) +
                            Count(new Stone(right, stone.Depth + 1), maxDepth);
        }
        
        return _cache[stone];
    }
}

internal sealed record Stone(long Number, int Depth)
{
    public string Digits => Number.ToString();
}
