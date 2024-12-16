using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 16");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    private readonly Maze _maze = BuildMaze(input);
    private readonly Dictionary<(Location, char), int> _cache = new();
    private int _fastest = int.MaxValue;

    protected override Task<string> SolvePart1Async()
    {
        var queue = new PriorityQueue<Reindeer, int>();
        
        queue.Enqueue(new Reindeer(_maze.Start, '>', new Path(_maze.Start)), 0);

        while (queue.Count > 0)
        {
            var reindeer = queue.Dequeue();

            if (reindeer.Path.Score >= _fastest)
                continue;

            if (reindeer.Position == _maze.Finish)
            {
                if (reindeer.Path.Score < _fastest)
                    _fastest = reindeer.Path.Score;
            }

            foreach (var next in GetValidMoves(reindeer))
            {
                if (_cache.TryGetValue(next.CacheKey, out var cached) && next.Path.Score >= cached)
                    continue;

                _cache[next.CacheKey] = next.Path.Score;
                
                queue.Enqueue(next, next.Path.Score);
            }
        }

        return Task.FromResult(_fastest.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var queue = new PriorityQueue<Reindeer, int>();
        var locations = new HashSet<Location>();
        
        queue.Enqueue(new Reindeer(_maze.Start, '>', new Path(_maze.Start)), 0);
        
        while (queue.Count > 0)
        {
            var reindeer = queue.Dequeue();

            if (reindeer.Position == _maze.Finish)
            {
                if (reindeer.Path.Score == _fastest)
                    locations.UnionWith(reindeer.Path.Visited);

                continue;
            }
            
            if (reindeer.Path.Score >= _fastest)
                continue;
            
            if (_cache.TryGetValue(reindeer.CacheKey, out var cached) && reindeer.Path.Score > cached)
                continue;
            
            foreach (var next in GetValidMoves(reindeer))
            {   
                queue.Enqueue(next, next.Path.Score);
            }
        }
        
        return Task.FromResult(locations.Count.ToString());
    }

    private IEnumerable<Reindeer> GetValidMoves(Reindeer reindeer)
    {
        foreach (var direction in GetAllowedDirections(reindeer.Direction))
        {
            var position = CalculatePosition(reindeer.Position, direction);

            if (_maze.IsWall(position))
                continue;
            
            if (reindeer.Path.Contains(position))
                continue;

            var path = new Path(reindeer.Path);
            
            path.Add(position, direction == reindeer.Direction ? 1 : 1001);
            
            yield return new Reindeer(position, direction, path);
        }
    }

    private static Maze BuildMaze(string[] input)
    {
        var builder = new MazeBuilder();

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                var location = new Location(row, col);
                switch (input[row][col])
                {
                    case 'S':
                        builder.SetStart(location);
                        break;
                    case 'E':
                        builder.SetFinish(location);
                        break;
                    case '#':
                        builder.AddWall(location);
                        break;
                }
            }
        }

        return builder.Build();
    }

    private static IEnumerable<char> GetAllowedDirections(char direction) => direction switch
    {
        '^' => new[] { '^', '<', '>' },
        'v' => new[] { 'v', '<', '>' },
        '<' => new[] { '<', '^', 'v' },
        '>' => new[] { '>', '^', 'v' },
        _ => throw new InvalidOperationException("Invalid direction")
    };

    private static Location CalculatePosition(Location position, char direction) => direction switch
    {
        '^' => position with { Row = position.Row - 1 },
        'v' => position with { Row = position.Row + 1 },
        '<' => position with { Column = position.Column - 1 },
        '>' => position with { Column = position.Column + 1 },
        _ => throw new InvalidOperationException("Invalid direction")
    };
}

internal readonly record struct Location(int Row, int Column);

internal readonly record struct Reindeer(Location Position, char Direction, Path Path)
{
    public (Location, char) CacheKey => (Position, Direction);
}

internal sealed class Path
{
    private readonly HashSet<Location> _visited = [];

    public int Score { get; private set; }

    public IReadOnlySet<Location> Visited => _visited;

    public Path(Location location)
    {
        _visited.Add(location);
    }

    public Path(Path path)
    {
        Score = path.Score;
        _visited = [..path._visited];
    }

    public void Add(Location location, int score)
    {
        _visited.Add(location);
        Score += score;
    }
    
    public bool Contains(Location location) => _visited.Contains(location);
}

internal sealed class Maze(Location start, Location finish, HashSet<Location> walls)
{
    public Location Start { get; } = start;
    public Location Finish { get; } = finish;

    public bool IsWall(Location location) => walls.Contains(location);
}

internal sealed class MazeBuilder
{
    private Location _start;
    private Location _finish;
    private readonly HashSet<Location> _walls = [];

    public void SetStart(Location start) => _start = start;
    public void SetFinish(Location finish) => _finish = finish;
    public void AddWall(Location wall) => _walls.Add(wall);

    public Maze Build()
    {
        if (_start == default || _finish == default)
            throw new InvalidOperationException("Start and finish locations must be set.");
        return new Maze(_start, _finish, _walls);
    }
}
