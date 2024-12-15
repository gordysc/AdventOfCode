using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 15");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var (robot, blocks, directions) = ParseInput(input);

        while (directions.Count > 0)
        {
            var direction = directions.Dequeue();
            var (dy, dx) = CalculateDeltas(direction);
            var candidate = new Coordinates(robot.Coordinates.Row + dy, robot.Coordinates.Column + dx);

            if (blocks.FirstOrDefault(x => x.Contains(candidate.Row, candidate.Column)) is not { } block)
            {
                robot.Coordinates = candidate;
                continue;
            }
            
            if (block is Box box)
            {
                List<Box> boxes = [box];
                
                var next = new Coordinates(box.Coordinates.Row + dy, box.Coordinates.Column + dx);
                
                while (blocks.FirstOrDefault(x => x.Contains(next.Row, next.Column)) is Box nextBox)
                {
                    boxes.Add(nextBox);
                    next = new Coordinates(nextBox.Coordinates.Row + dy, nextBox.Coordinates.Column + dx);
                }
                
                if (blocks.FirstOrDefault(x => x.Contains(next.Row, next.Column)) is not Wall)
                {
                    boxes.ForEach(x => x.Move(dy, dx));
                    robot.Coordinates = candidate;
                }
            }
        }

        var result = blocks.OfType<Box>().Sum(x => x.GpsCoordinates);
        
        return Task.FromResult(result.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        return Task.FromResult("Not implemented");
    }
    
    private static (int, int) CalculateDeltas(char direction) => direction switch
    {
        '^' => (-1, 0),
        'v' => (1, 0),
        '>' => (0, 1),
        '<' => (0, -1),
        _ => throw new ArgumentException("Invalid direction")
    };
    
    private static (Robot, List<Block>, Queue<char>) ParseInput(string[] input, int width = 1)
    {
        var robot = new Robot();
        var blocks = new List<Block>();
        var directions = new Queue<char>();
        
        var section = 1;

        for (var row = 0; row < input.Length; row++)
        {
            if (string.IsNullOrWhiteSpace(input[row]))
            {
                section++;
                continue;
            }
            
            if (section == 1)
            {
                for (var col = 0; col < input[row].Length; col++)
                {
                    if (input[row][col] == '#')
                        blocks.Add(new Wall(new Coordinates(row, col), width));
                    else if (input[row][col] == 'O')
                        blocks.Add(new Box(new Coordinates(row, col), width));
                    else if (input[row][col] == '@')
                        robot.Coordinates = new Coordinates(row, col);
                }
            }
            
            if (section == 2)
            {
                foreach (var c in input[row])
                    directions.Enqueue(c);
            }
        }
        
        return (robot, blocks, directions);
    }
}

internal sealed class Robot
{
    public Coordinates Coordinates { get; set; } = new(0, 0);
}

internal sealed class Coordinates(int row, int column)
{
    public int Row { get; set; } = row;
    public int Column { get; set; } = column;
}

internal abstract class Block(Coordinates coordinates, int width)
{
    public Coordinates Coordinates = coordinates;

    public bool Contains(int row, int column)
    {
        return row == Coordinates.Row && 
               column >= Coordinates.Column && 
               column < Coordinates.Column + width;
    }
}

internal sealed class Wall(Coordinates coordinates, int width) : Block(coordinates, width);
internal sealed class Box(Coordinates coordinates, int width) : Block(coordinates, width)
{
    public void Move(int dy, int dx)
    {
        Coordinates = new Coordinates(Coordinates.Row + dy, Coordinates.Column + dx);
    }
    
    public int GpsCoordinates => Coordinates.Row * 100 + Coordinates.Column;
}
