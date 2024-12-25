using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 25");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var (locks, keys) = ParseLocksAndKeys(input);
        
        var answer = keys.Aggregate(0, (acc, key) =>
        {
            foreach (var @lock in locks)
            {
                var result = new int[key.Length];

                for (var i = 0; i < key.Length; i++)
                    result[i] = @lock[i] + key[i];

                if (result.All(x => x <= 5))
                    acc++;
            }

            return acc;
        });
        
        return Task.FromResult(answer.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        return Task.FromResult("Not implemented");
    }

    private static (List<int[]> locks, List<int[]> keys) ParseLocksAndKeys(string[] input)
    {
        var locks = new List<int[]>();
        var keys = new List<int[]>();

        var buffer = new List<string>();
        var schematics = new List<List<string>>();

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                schematics.Add(buffer.ToList());
                buffer.Clear();
            }
            else
            {
                buffer.Add(line);
            }
        }
        
        if (buffer.Count > 0)
            schematics.Add(buffer.ToList());

        foreach (var schematic in schematics)
        {
            var heights = ParseHeights(schematic);
                
            if (schematic[0].All(x => x == '#'))
                locks.Add(heights);
            else
                keys.Add(heights);
        }

        return (locks, keys);
    }
    
    private static int[] ParseHeights(List<string> lines)
    {
        var width = lines[0].Length;
        var heights = new int[width];

        for (int col = 0; col < width; col++)
        {
            heights[col] = lines.Count(line => line[col] == '#') - 1;
        }

        return heights;
    }
}
