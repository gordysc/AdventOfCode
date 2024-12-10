using Shared;

var input = File
    .ReadAllLines("Input.txt")
    .Select(x => x.Select(i => i - '0').ToArray())
    .ToArray();

var solution = new Solution(input);

Console.WriteLine("Day 10");

await solution.SolveAsync();

internal sealed class Solution(int[][] input) : AbstractSolution
{
    private int Width => input[0].Length;
    private int Height => input.Length;
    
    protected override Task<string> SolvePart1Async()
    {
        var scores = new HashSet<(Position, Position)>();
        
        EvaluateTrails(trail => scores.Add((trail.Trailhead, trail.Position)));
     
        return Task.FromResult(scores.Count.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var total = 0;
        
        EvaluateTrails(_ => total++);
        
        return Task.FromResult(total.ToString());
    }

    private void EvaluateTrails(Action<Trail> callback)
    {
        var trailheads = GetTrailheads();
        var trails = new Queue<Trail>();
        
        foreach (var trailhead in trailheads)
            trails.Enqueue(new Trail(trailhead, trailhead, 0));
        
        while (trails.Count > 0)
        {
            var trail = trails.Dequeue();
            var positions = GetNextPositions(trail.Position, trail.Height);

            foreach (var position in positions)
            {
                if (input[position.Y][position.X] == 9)
                {
                    callback(trail.Move(position));
                    continue;
                }
                
                trails.Enqueue(trail.Move(position));
            }
        }
    }

    private IEnumerable<Position> GetNextPositions(Position position, int height)
    {
        if (position.X > 0 && input[position.Y][position.X - 1] == height + 1)
            yield return position with { X = position.X - 1 };
        if (position.X < Width - 1 && input[position.Y][position.X + 1] == height + 1)
            yield return position with { X = position.X + 1 };
        if (position.Y > 0 && input[position.Y - 1][position.X] == height + 1)
            yield return position with { Y = position.Y - 1 };
        if (position.Y < Height - 1 && input[position.Y + 1][position.X] == height + 1)
            yield return position with { Y = position.Y + 1 };
    }

    private List<Position> GetTrailheads()
    {
        var trailheads = new List<Position>();
        
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (input[y][x] == 0)
                    trailheads.Add(new Position(x, y));
            }
        }
        
        return trailheads;
    }
}

internal record Trail(Position Trailhead, Position Position, int Height)
{
    public Trail Move(Position position) => 
        this with { Position = position, Height = Height + 1 };
}

internal record Position(int X, int Y);
