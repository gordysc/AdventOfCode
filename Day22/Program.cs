using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 22");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var seeds = input.Select(long.Parse).ToList();
        var answer = seeds.Aggregate(0L, (acc, seed) =>
        {
            var sequence = GenerateSecretNumber(seed, 2_000);
            
            return acc + sequence[^1];
        });
        
        return Task.FromResult(answer.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var seeds = input.Select(long.Parse).ToList();
        var sequences = seeds.Select(x => GenerateSecretNumber(x, 2_000));
        var changes = sequences.Select(CalculateChanges);
        var maps = changes.Select(CreatePriceMap).ToList();
        var answer = FindMaximumBananas(maps);
        
        return Task.FromResult(answer.ToString());
    }

    private static int FindMaximumBananas(List<Dictionary<string, int>> maps)
    {
        var keys = new HashSet<string>();
        
        foreach (var map in maps)
            keys.UnionWith(map.Keys);
        
        var totals = keys.Select(seq => maps.Aggregate(0, (acc, map) => acc + map.GetValueOrDefault(seq, 0)));

        return totals.Max();
    }

    private static Dictionary<string, int> CreatePriceMap(List<(int Price, int Diff)> changes)
    {
        var result = new Dictionary<string, int>();

        for (var loop = 0; loop < changes.Count - 4; loop++)
        {
            var values = changes.Skip(loop).Take(4).ToArray();
            var key = string.Join(',', values.Select(x => x.Diff));
            var price = values[3].Price;

            result.TryAdd(key, price);
        }

        return result;
    }

    private static List<(int Price, int Diff)> CalculateChanges(List<long> sequence)
    {
        var result = new List<(int Price, int Diff)>();
        
        for (var loop = 0; loop < sequence.Count; loop++)
        {
            var price = (int)(sequence[loop] % 10);
            var diff = loop == 0 ? 0 : price - result[loop - 1].Price;

            result.Add((price, diff));
        }

        return result;
    }

    private static List<long> GenerateSecretNumber(long seed, int iterations)
    {
        List<long> sequence = [seed];

        for (var loop = 0; loop < iterations; loop++)
        {
            var next = sequence[^1];
            
            next = Prune(Mix(next, next * 64));
            next = Prune(Mix(next, next / 32));
            next = Prune(Mix(next, next * 2048));

            sequence.Add(next);
        }

        return sequence;
    }

    private static long Mix(long a, long b) => a ^ b;
    private static long Prune(long a) => a % 16777216;
}
