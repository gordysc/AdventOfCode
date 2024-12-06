using Shared;

var input = File.ReadAllLines("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 06");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    private int Height => input.Length;
    private int Width => input[0].Length;
    
    private Guard Guard = input
        .SelectMany((line, y) => line.Select((c, x) => (c, x, y)))
        .Where(t => t.c is '^' or 'v' or '<' or '>')
        .Select(t => new Guard { Position = new Position(t.x, t.y), Direction = t.c })
        .Single();
    
    private List<Position> Obstructions = input
        .SelectMany((line, y) => line.Select((c, x) => (c, x, y)))
        .Where(t => t.c == '#')
        .Select(t => new Position(t.x, t.y))
        .ToList();
    
    protected override Task<string> SolvePart1Async()
    {
        var positions = GetGuardPath();
        
        return Task.FromResult(positions.Count.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var positions = GetGuardPath();

        var answer = positions.Count(p =>
        {
            var guard = Guard with { };
            var obstructions = new HashSet<Position>(Obstructions) { p };
            var result = PathResult.Unknown;
            var traveled = new HashSet<(Position, char)> { (guard.Position, guard.Direction) };

            while (result == PathResult.Unknown)
            {
                var next = guard.GetNextPosition();

                if (next.IsWithinBounds(Width, Height) is false)
                    result = PathResult.Exited;
                else if (obstructions.Contains(next))
                    guard.Turn();
                else if (traveled.Add((next, guard.Direction)) is false)
                    result = PathResult.Loop;
                else
                    guard.Move();
            }

            return result == PathResult.Loop;
        });
        
        return Task.FromResult(answer.ToString());
    }

    private HashSet<Position> GetGuardPath()
    {
        var result = PathResult.Unknown;
        var guard = Guard with { };
        var path = new HashSet<Position> { guard.Position };

        while (result == PathResult.Unknown)
        {
            var next = guard.GetNextPosition();
            
            if (next.IsWithinBounds(Width, Height) is false)
                result = PathResult.Exited;
            else if (Obstructions.Contains(next))
                guard.Turn();
            else
            {
                guard.Move();
                path.Add(guard.Position);
            }
        }

        return path;
    }
}

internal sealed record Position(int X, int Y)
{
    public bool IsWithinBounds(int width, int height) => 
        X >= 0 && X < width && Y >= 0 && Y < height;
}

internal sealed record Guard
{
    private const char Up = '^';
    private const char Down = 'v';
    private const char Left = '<';
    private const char Right = '>';
    
    public required Position Position { get; set; }
    public required char Direction { get; set; }

    public void Move()
    {
        Position = GetNextPosition();
    }
    
    public Position GetNextPosition()
    {
        return Direction switch
        {
            Up => Position with { Y = Position.Y - 1 },
            Down => Position with { Y = Position.Y + 1 },
            Left => Position with { X = Position.X - 1 },
            Right => Position with { X = Position.X + 1 },
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }
    
    public void Turn()
    {
        Direction = Direction switch
        {
            Up => Right,
            Right => Down,
            Down => Left,
            Left => Up,
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }
}

internal enum PathResult
{
    Loop,
    Exited,
    Unknown
}