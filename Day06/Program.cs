using Shared;

var input = File.ReadAllText("Input.txt")
    .Split(Environment.NewLine)
    .Select(x => x.ToCharArray())
    .ToArray();

var solution = new Solution(input);

Console.WriteLine("Day 06");

await solution.SolveAsync();

internal sealed class Solution(char[][] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var evaluation = EvaluateGrid(FindGuard(input), input);
        
        if (evaluation.PathResult != PathResult.Exited)
            throw new InvalidOperationException("Path did not exit the grid");
        
        return Task.FromResult(evaluation.Visits.Keys.Count.ToString());
    }
    
    protected override Task<string> SolvePart2Async()
    {
        var start = FindGuard(input);
        var height = input.Length;

        var visits = EvaluateGrid(start, input).Visits.Keys;

        var obstructions = visits.AsParallel().Count(i =>
        {
            if (i == start)
                return false;
            
            var grid = new char[height][];
            
            for (var loop = 0; loop < height; loop++)
                grid[loop] = (char[]) input[loop].Clone(); ;
            
            grid[i.Y][i.X] = '#';

            return EvaluateGrid(start, grid).PathResult == PathResult.Loop;
        });

        return Task.FromResult(obstructions.ToString());
    }
    
    private static GridEvaluation EvaluateGrid(Point start, char[][] grid)
    {
        var result = new GridEvaluation();
        
        var point = start;
        var direction = grid[point.Y][point.X];
        
        var width = grid[0].Length;
        var height = grid.Length;

        result.Visits = new Dictionary<Point, List<char>> { { point, [direction] } };

        while (true)
        {
            var next = CalculateNextPoint(point, direction);

            if (next.X < 0 || next.X >= width || next.Y < 0 || next.Y >= height)
            {
                result.PathResult = PathResult.Exited;
                break;
            }
            
            var value = grid[next.Y][next.X];

            if (value == '#')
            {
                direction = CalculateNextDirection(direction);
                continue;
            }
            if (result.Visits.TryGetValue(next, out var directions))
            {
                if (directions.Contains(direction))
                {
                    result.PathResult = PathResult.Loop;
                    break;
                }

                directions.Add(direction);
            }
            else            
            {
                result.Visits.Add(next, [direction]);
            }
            
            point = next;
        }

        return result;
    }
    
    private const char Up = '^';
    private const char Down = 'v';
    private const char Left = '<';
    private const char Right = '>';

    private static Point CalculateNextPoint(Point current, char direction)
    {
        return direction switch
        {
            Up => current with { Y = current.Y - 1 },
            Down => current with { Y = current.Y + 1 },
            Left => current with { X = current.X - 1 },
            Right => current with { X = current.X + 1 },
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }

    private static char CalculateNextDirection(char direction)
    {
        return direction switch
        {
            Up or Down => direction == Up ? Right : Left,
            Left or Right => direction == Left ? Up : Down,
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }

    private static Point FindGuard(IReadOnlyList<char[]> grid)
    {
        for (var y = 0; y < grid.Count; y++)
        {
            for (var x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] != '.' && grid[y][x] != '#')
                    return new Point(x, y);
            }
        }

        throw new InvalidOperationException("Guard not found");
    }
}

internal sealed record Point(int X, int Y);

internal enum PathResult
{
    Loop,
    Exited,
    Unknown
}

internal class GridEvaluation
{
    public PathResult PathResult { get; set; } = PathResult.Unknown;
    public Dictionary<Point, List<char>> Visits { get; set; } = new();
}