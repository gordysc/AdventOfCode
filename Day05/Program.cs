using Shared;

var input = File.ReadAllLines("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 05");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var (rules, updates) = ParseInput(input);
        
        var total = 0;

        foreach (var update in updates)
        {
            var sorted = CorrectOrder(rules, update);
            
            if (update.SequenceEqual(sorted))
                total += int.Parse(sorted[sorted.Count / 2]);
        }
        
        return Task.FromResult(total.ToString());
    }
    
    protected override Task<string> SolvePart2Async()
    {
        var (rules, updates) = ParseInput(input);

        var total = 0;

        foreach (var update in updates)
        {
            var sorted = CorrectOrder(rules, update);
            
            if (update.SequenceEqual(sorted) is false)
                total += int.Parse(sorted[sorted.Count / 2]);
        }
        
        return Task.FromResult(total.ToString());
    }

    private static List<string> CorrectOrder(Dictionary<string, List<string>> rules, List<string> update)
    {
        var sorted = update.ToList();
            
        sorted.Sort((x, y) =>
        {
            if (rules.TryGetValue(x, out var value) is false)
                return 1;
                
            return value.Contains(y) ? -1 : 1;
        });
        
        return sorted;
    }

    private static (Dictionary<string, List<string>>, List<List<string>>) ParseInput(string[] input)
    {
        var parseRule = true;
        var rules = new Dictionary<string, List<string>>();
        var updates = new List<List<string>>();

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                parseRule = false;
                continue;
            }

            if (parseRule)
            {
                var rule = line.Split('|');
                
                if (rules.TryGetValue(rule[0], out var entry))
                    entry.Add(rule.Last());
                else
                    rules[rule[0]] = [rule.Last()];
            }
            else
            {
                updates.Add(line.Split(',').ToList());
            }
        }
        
        return (rules, updates);
    }
}