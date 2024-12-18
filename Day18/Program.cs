using System.Text;
using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 18");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    private const int Width = 70;
    private const int Height = 70;
    
    private static readonly (int X, int Y) Up = (0, 1);
    private static readonly (int X, int Y) Down = (0, -1);
    private static readonly (int X, int Y) Left = (-1, 0);
    private static readonly (int X, int Y) Right = (1, 0);
    private static readonly (int X, int Y)[] Directions = [Up, Down, Left, Right];
    
    protected override Task<string> SolvePart1Async()
    {
        var bytes = ParseBytes(input);
        var minimum = FindMinimumSteps(bytes.Take(1024).ToHashSet());
        
        return Task.FromResult(minimum.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var bytes = ParseBytes(input);
        var unreachable = int.MaxValue;
        
        for (var loop = 1; loop < input.Length; loop++)
        {
            var minimum = FindMinimumSteps(bytes.Take(loop).ToHashSet());
            
            if (minimum == int.MaxValue)
            {
                unreachable = loop;
                break;
            }
        }
        
        var result = bytes[unreachable - 1];
        var x = result.X;
        var y = Height - result.Y;
        
        return Task.FromResult($"{x},{y}");
    }

    private int FindMinimumSteps(HashSet<Position> bytes)
    {
        var start = new Position(0, Height);
        var finish = new Position(Width, 0);
        var cache = new Dictionary<Position, int>();
        
        var queue = new PriorityQueue<List<Position>, int>();
        var minimum = int.MaxValue;
        
        queue.Enqueue([start], 0);

        while (queue.TryDequeue(out var path,out var steps))
        {
            if (steps > minimum)
                continue;

            var position = path.Last();
            
            if (position == finish)
            {
                minimum = Math.Min(minimum, steps);
                continue;
            }
            
            if (cache.TryGetValue(position, out var value) && value <= steps)
                continue;
            
            cache[position] = steps;

            foreach (var delta in Directions)
            {
                var next = new Position(position.X + delta.X, position.Y + delta.Y);
                
                if (next.X < 0 || next.X > Width || next.Y < 0 || next.Y > Height)
                    continue;
                
                if (bytes.Contains(next))
                    continue;
                
                if (path.Contains(next))
                    continue;

                var newPath = new List<Position>(path) {next};
                
                queue.Enqueue(newPath, steps + 1);
            }
        }
        
        return minimum;
    }

    private static List<Position> ParseBytes(string[] input, int limit = -1)
    {
        var bytes = new List<Position>();

        foreach (var line in input)
        {
            if (limit > 0 && bytes.Count == limit)
                break;
            
            var coordinates = line.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            
            var x = coordinates[0];
            var y = Height - coordinates[1];
            
            bytes.Add(new Position(x, y));
        }
        
        return bytes;
    }
}

internal readonly record struct Position(int X, int Y);