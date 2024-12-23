using System.Text.RegularExpressions;
using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 22");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var computers = ParseComputers(input);
        var parties = FindParties(computers);
        
        var answer = parties.Count(p => p.Computers.Any(n => n.StartsWith('t')));
        
        return Task.FromResult(answer.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var computers = ParseComputers(input);
        var parties = FindParties(computers);
        
        var groups = parties.GroupBy(p => p.A);
        var popular = groups.OrderByDescending(g => g.Count()).First();

        var network = popular.SelectMany(p => p.Computers).Distinct();
        var password = string.Join(",", network.Order());
        
        return Task.FromResult(password);
    }

    private static List<Party> FindParties(Dictionary<string, HashSet<string>> computers)
    {
        var parties = new HashSet<Party>();

        foreach (var node in computers.Keys)
        {
            foreach (var neighbor in computers[node])
            {
                foreach (var shared in computers[node].Intersect(computers[neighbor]))
                {
                    parties.Add(Party.Create(node, neighbor, shared));
                }
            }
        }

        return parties.ToList();
    }
    
    private static Dictionary<string, HashSet<string>> ParseComputers(string[] input)
    {
        var computers = new Dictionary<string, HashSet<string>>();

        foreach (var line in input)
        {
            var match = Regex.Matches(line, "([a-z]+)-([a-z]+)");
            
            var id1 = match[0].Groups[1].Value;
            var id2 = match[0].Groups[2].Value;

            computers.TryAdd(id1, []);
            computers.TryAdd(id2, []);

            computers[id1].Add(id2);
            computers[id2].Add(id1);
        }

        return computers;
    }
}

internal readonly record struct Party(string A, string B, string C)
{
    public IReadOnlyList<string> Computers => [A, B, C];
    
    public static Party Create(string c1, string c2, string c3)
    {
        var ordered = new[] { c1, c2, c3 }.Order().ToArray();
        
        return new Party(ordered[0], ordered[1], ordered[2]);
    }
}
