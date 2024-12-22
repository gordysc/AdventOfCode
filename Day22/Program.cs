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
        var markets = sequences.Select(CalculateMarkets);
        
        var answer = FindMaximumBananas(markets);
        
        return Task.FromResult(answer.ToString());
    }

    private static int FindMaximumBananas(IEnumerable<List<(int Price, int Diff)>> markets)
    {
        var totals = new Dictionary<string, int>();

        foreach (var changes in markets)
        {
            var seen = new HashSet<string>();
            
            for (var loop = 0; loop < changes.Count - 4; loop++)
            {
                var values = changes.Skip(loop).Take(4).ToArray();
                var key = string.Join(',', values.Select(x => x.Diff));
                var price = values[3].Price;

                if (seen.Add(key))
                {
                    totals.TryAdd(key, 0);
                    totals[key] += price;
                }
            }
        }

        return totals.Values.Max();
    }

    private static List<(int Price, int Diff)> CalculateMarkets(List<long> sequence)
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
