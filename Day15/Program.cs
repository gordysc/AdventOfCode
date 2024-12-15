using Shared;

var input = File.ReadAllText("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 15");

await solution.SolveAsync();

internal sealed class Solution(string input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var sections = input.Trim().Split("\n\n").ToArray();
        
        var grid = sections[0].Split("\n").Select(row => row.ToCharArray()).ToArray();
        var movements = sections[1].Replace("\n", "").ToCharArray();
        var builder = new WarehouseBuilder();

        for (var row = 0; row < grid.Length; row++)
        {
            for (var col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == '@')
                    builder.SetRobot(row, col);
                else if (grid[row][col] == '#')
                    builder.AddWall(row, col, 1);
                else if (grid[row][col] == 'O')
                    builder.AddBox(row, col, 1);
            }
        }
        
        var warehouse = builder.Build();
        
        warehouse.ProcessMovements(movements);

        var result = warehouse.Boxes.Sum(x => x.Row * 100 + x.Column);
        
        return Task.FromResult(result.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var sections = input.Trim().Split("\n\n").ToArray();
        
        var grid = sections[0].Split("\n").Select(row => row.ToCharArray()).ToArray();
        var movements = sections[1].Replace("\n", "").ToCharArray();
        var builder = new WarehouseBuilder();

        for (var row = 0; row < grid.Length; row++)
        {
            for (var col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == '@')
                    builder.SetRobot(row, col * 2);
                else if (grid[row][col] == '#')
                    builder.AddWall(row, col * 2, 2);
                else if (grid[row][col] == 'O')
                    builder.AddBox(row, col * 2, 2);
            }
        }
        
        var warehouse = builder.Build();
        
        warehouse.ProcessMovements(movements);

        var result = warehouse.Boxes.Sum(x => x.Row * 100 + x.Column);
        
        return Task.FromResult(result.ToString());
    }
}

internal sealed class Boundaries(int row, int column, int width)
{
    private int Row { get; } = row;
    private int Column { get; } = column;

    private int Width { get; } = width;
    private const int Height = 1;

    public bool Overlaps(Boundaries other)
    {
        return Column < other.Column + other.Width &&
               Column + Width > other.Column &&
               Row < other.Row + Height &&
               Row + Height > other.Row;
    }
}

internal sealed class Wall(int row, int column, int width)
{
    public Boundaries Boundaries => new(row, column, width);
}

internal sealed class Box(int row, int column, int width, int id)
{
    public int Id { get; } = id;
    public int Row { get; private set; } = row;
    public int Column { get; private set; } = column;
    public int Width => width;
    
    public Boundaries Boundaries => new(Row, Column, width);
    
    public void Move(int dx, int dy)
    {
        Column += dx;
        Row += dy;
    }
}

internal sealed class Robot(int row, int column)
{
    public int Row { get; private set; } = row;
    public int Column { get; private set; } = column;

    public void Move(int dx, int dy)
    {
        Column += dx;
        Row += dy;
    }
}

internal sealed class WarehouseBuilder
{
    private Robot? _robot;
    private readonly List<Wall> _walls = [];
    private readonly List<Box> _boxes = [];

    public void SetRobot(int row, int column)
    {
        _robot = new Robot(row, column);
    }
    
    public void AddWall(int row, int column, int width)
    {
        _walls.Add(new Wall(row, column, width));
    }
    
    public void AddBox(int row, int column, int width)
    {
        _boxes.Add(new Box(row, column, width, _boxes.Count));
    }
    
    public Warehouse Build()
    {
        if (_robot == null)
            throw new InvalidOperationException("Robot is not set");
        
        return new Warehouse(_walls, _boxes, _robot);
    }
}

internal sealed class Warehouse(List<Wall> walls, List<Box> boxes, Robot robot)
{
    private static readonly Dictionary<char, (int x, int y)> Instructions = new()
    {
        { '^', (0, -1) },
        { 'v', (0, 1) },
        { '<', (-1, 0) },
        { '>', (1, 0) }
    };
    
    public IReadOnlyList<Box> Boxes => boxes;

    public void ProcessMovements(char[] movements)
    {
        foreach (var move in movements)
        {
            var instruction = Instructions[move];
            
            var position = new Boundaries(
                row: robot.Row + instruction.y,
                column: robot.Column + instruction.x,
                width: 1
            );

            if (IsWallCollision(position)) continue;

            if (boxes.FirstOrDefault(x => x.Boundaries.Overlaps(position)) is not { } box)
            {
                robot.Move(instruction.x, instruction.y);
                continue;
            }

            if (MoveBox(box, instruction))
            {
                robot.Move(instruction.x, instruction.y);
            }
        }
    }

    private bool IsWallCollision(Boundaries position) =>
        walls.Any(wall => wall.Boundaries.Overlaps(position));

    private bool MoveBox(Box box, (int x, int y) instruction)
    {
        var candidates = new Queue<Box>();
        var ids = new HashSet<int>();

        candidates.Enqueue(new Box(box.Row + instruction.y, box.Column + instruction.x, box.Width, box.Id));

        while (candidates.Count > 0)
        {
            var candidate = candidates.Dequeue();

            if (IsWallCollision(candidate.Boundaries)) 
                return false;

            var collisions = boxes
                .Where(b => b.Boundaries.Overlaps(candidate.Boundaries) && b.Id != candidate.Id)
                .ToList();

            foreach (var collision in collisions)
            {
                candidates.Enqueue(new Box(
                    collision.Row + instruction.y,
                    collision.Column + instruction.x,
                    collision.Width,
                    collision.Id
                ));
            }

            ids.Add(candidate.Id);
        }

        foreach (var id in ids)
        {
            var boxToUpdate = boxes.First(b => b.Id == id);
            boxToUpdate.Move(instruction.x, instruction.y);
        }

        return true;
    }
}
