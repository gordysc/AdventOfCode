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
        
        var valid = updates.Where(update =>
        {
            for (var loop = 0; loop < update.Count; loop++)
            {
                if (loop == 0)
                    continue;
                if (rules.TryGetValue(update[loop], out var rule) is false)
                    continue;

                var previous = update[..loop];

                if (rule.Any(r => previous.Contains(r)))
                    return false;
            }

            return true;
        }).ToList();
        
        var total = valid.Aggregate(0, (acc, update) => acc + int.Parse(update[update.Count / 2]));
        
        return Task.FromResult(total.ToString());
    }
    
    protected override Task<string> SolvePart2Async()
    {
        var (rules, updates) = ParseInput(input);
        
        var invalids = updates.Where(update =>
        {
            for (var loop = 0; loop < update.Count; loop++)
            {
                if (loop == 0)
                    continue;
                if (rules.TryGetValue(update[loop], out var rule) is false)
                    continue;

                var previous = update[..loop];

                if (rule.Any(r => previous.Contains(r)))
                    return true;
            }

            return false;
        }).ToList();
        
        var total = invalids.Aggregate(0, (acc, update) =>
        {
            var invalid = CorrectOrder(rules, update).Select(int.Parse).ToList();
            
            return acc + invalid[invalid.Count / 2];
        });
        
        return Task.FromResult(total.ToString());
    }

    private static List<string> CorrectOrder(Dictionary<string, List<string>> rules, List<string> update)
    {
        var result = new List<string>(update.Count);
        
        for (var loop = 0; loop < update.Count; loop++)
        {
            if (loop == 0)
            {
                result.Add(update[loop]);
                continue;
            }
            
            if (rules.TryGetValue(update[loop], out var rule) is false)
            {
                result.Add(update[loop]);
                continue;
            }

            if (result.FirstOrDefault(x => rule.Contains(x)) is not null)
            {
                var index = result.FindIndex(x => rule.Contains(x));
                result.Insert(index, update[loop]);
            }
            else
            {
                result.Add(update[loop]);
            }
        }
        
        return result;
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