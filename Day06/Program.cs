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
        var visited = FindGuardPath(input);

        return Task.FromResult(visited.Count.ToString());
    }
    
    protected override Task<string> SolvePart2Async()
    {
        var start = FindGuardCoordinates(input);
        var height = input.Length;

        var visited = FindGuardPath(input);

        var obstructions = visited.AsParallel().Count(i =>
        {
            if (i == start)
                return false;
            
            var grid = new char[height][];
            
            for (var loop = 0; loop < height; loop++)
                grid[loop] = (char[]) input[loop].Clone(); ;
            
            grid[i.Y][i.X] = '#';

            return IsLoop(grid, start);
        });

        return Task.FromResult(obstructions.ToString());
    }

    private static HashSet<Coordinates> FindGuardPath(char[][] grid)
    {
        var coordinates = FindGuardCoordinates(grid);
        var direction = grid[coordinates.Y][coordinates.X];
        
        var width = grid[0].Length;
        var height = grid.Length;

        var visited = new HashSet<Coordinates> { coordinates };

        while (true)
        {
            var next = CalculateNextCoordinates(coordinates, direction);
            
            if (next.X < 0 || next.X >= width || next.Y < 0 || next.Y >= height)
                break;
            
            var value = grid[next.Y][next.X];

            if (value == '#')
                direction = CalculateNextDirection(direction);
            else
            {
                coordinates = next;
                visited.Add(next);
            }
        }
        
        return visited;
    }
    
    private static bool IsLoop(char[][] grid, Coordinates start)
    {
        var coordinates = start;
        var direction = grid[coordinates.Y][coordinates.X];
        
        var width = grid[0].Length;
        var height = grid.Length;

        var visited = new Dictionary<Coordinates, List<char>> { { coordinates, [direction] } };

        while (true)
        {
            var next = CalculateNextCoordinates(coordinates, direction);

            if (next.X < 0 || next.X >= width || next.Y < 0 || next.Y >= height)
                return false;
            
            var value = grid[next.Y][next.X];

            if (value == '#')
            {
                direction = CalculateNextDirection(direction);
                continue;
            }
            if (visited.TryGetValue(next, out var directions))
            {
                if (directions.Contains(direction))
                    return true;

                directions.Add(direction);
            }
            else            
            {
                visited.Add(next, [direction]);
            }
            
            coordinates = next;
        }
    }
    
    private static Coordinates FindGuardCoordinates(IReadOnlyList<char[]> grid)
    {
        for (var y = 0; y < grid.Count; y++)
        {
            for (var x = 0; x < grid[y].Length; x++)
            {
                var c = grid[y][x];
                
                if (c is Up or Down or Left or Right)
                {
                    return new Coordinates(x, y);
                }
            }
        }

        throw new InvalidOperationException("Guard not found");
    }
    
    private const char Up = '^';
    private const char Down = 'v';
    private const char Left = '<';
    private const char Right = '>';

    private static Coordinates CalculateNextCoordinates(Coordinates current, char direction)
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
}

internal sealed record Coordinates(int X, int Y);
internal enum PathResult
{
    Loop,
    Exited
}