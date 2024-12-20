using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 19");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var track = new RaceTrack();
        var walls = new List<Location>();
        
        for (var row = 0; row < input.Length; row++)
        {
            for (var column = 0; column < input[row].Length; column++)
            {
                switch (input[row][column])
                {
                    case '#':
                        walls.Add(new Location(row, column));
                        break;
                    case 'S':
                        track.SetStart(new Location(row, column));
                        break;
                    case 'E':
                        track.SetFinish(new Location(row, column));
                        break;
                    default:
                        track.AddTrack(new Location(row, column));
                        break;
                }
            }
        }

        var cheats = 0;

        foreach (var wall in walls)
        {
            track.AddTrack(wall);
            
            if (track.CanQualify(100))
                cheats++;
            
            track.RemoveTrack(wall);
        }
        
        return Task.FromResult(cheats.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        return Task.FromResult("Not implemented");
    }
}

internal readonly record struct Location(int Row, int Column)
{
    public bool IsNeighbor(Location other) => 
        Math.Abs(Row - other.Row) + Math.Abs(Column - other.Column) == 1;
}

internal sealed class Node(Location location)
{
    private readonly HashSet<Node> _neighbors = [];
    
    public Location Location => location;
    public IReadOnlySet<Node> Neighbors => _neighbors;

    public void AddNeighbor(Node node)
    {
        _neighbors.Add(node);
    }
    
    public void RemoveNeighbor(Node node)
    {
        _neighbors.Remove(node);
    }

    public bool IsNeighbor(Node node) => 
        Location.IsNeighbor(node.Location);
}

internal sealed class RaceTrack
{
    private Location _start = new(int.MaxValue, int.MaxValue);
    private Location _finish = new(int.MaxValue, int.MaxValue);
    
    private readonly HashSet<Node> _nodes = [];

    public void SetStart(Location location)
    {
        _start = location;
        
        AddTrack(location);
    }
    
    public void SetFinish(Location location)
    {
        _finish = location;
        
        AddTrack(location);
    }

    public void AddTrack(Location location)
    {
        if (_nodes.Any(n => n.Location == location))
            return;
        
        var node = new Node(location);
        
        if (_nodes.Contains(node)) 
            return;

        foreach (var other in _nodes.Where(node.IsNeighbor))
        {
            node.AddNeighbor(other);
            other.AddNeighbor(node);
        }
        
        _nodes.Add(node);
    }

    public void RemoveTrack(Location location)
    {
        if (_nodes.FirstOrDefault(n => n.Location == location) is not { } node)
            return;

        foreach (var neighbor in node.Neighbors)
        {
            neighbor.RemoveNeighbor(node);
            node.RemoveNeighbor(neighbor);
        }
        
        _nodes.Remove(node);
    }

    public bool CanQualify(int threshold)
    {
        var times = new Dictionary<Location, int>();
        var queue = new PriorityQueue<Node, int>();
        var timeLimit = _nodes.Count - threshold - 1;
        
        queue.Enqueue(_nodes.First(n => n.Location == _start), 0);

        while (queue.TryDequeue(out var node, out var time))
        {
            if (node.Location == _finish)
                return time <= timeLimit;

            if (time >= timeLimit)
                return false;
            
            if (times.TryGetValue(node.Location, out var best) && time >= best)
                continue;
            
            times[node.Location] = time;
            
            foreach (var neighbor in node.Neighbors)
            {
                queue.Enqueue(neighbor, time + 1);
            }
        }

        return false;
    }
}