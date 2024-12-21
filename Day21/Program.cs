using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 21");

await solution.SolveAsync();

internal sealed class Solution(string[] codes) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var answer = codes.Sum(x =>
        {
            var numeric = long.Parse(x[..^1]);
            var length = FindShortestSequence(Keypad, x, 2, []);

            return numeric * length;
        });
        
        return Task.FromResult(answer.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var answer = codes.Sum(x =>
        {
            var numeric = long.Parse(x[..^1]);
            var length = FindShortestSequence(Keypad, x, 25, []);

            return numeric * length;
        });
        
        return Task.FromResult(answer.ToString());
    }
    
    private static long FindShortestSequence(Dictionary<char, Position> keypad, string code, int depth, Dictionary<(string, int), long> cache)
    {
        if (cache.TryGetValue((code, depth), out var result))
            return result;
        
        var length = 0L;
        var cursor = 'A';

        foreach (var character in code)
        {
            var paths = FindMinimumPaths(keypad, cursor, character);

            if (depth == 0)
                length += paths[0].Length;
            else
                length += paths.Select(x => FindShortestSequence(InstructionPad, x, depth - 1, cache)).Min();
            
            cursor = character;
        }

        cache[(code, depth)] = length;
        
        return length;
    }

    private static List<string> FindMinimumPaths(Dictionary<char, Position> keypad, char start, char finish)
    {
        var queue = new Queue<(Position Position, string Path)>();
        var paths = new List<string>();
        var shortest = long.MaxValue;
        
        queue.Enqueue((keypad[start], string.Empty));
        
        while (queue.TryDequeue(out var current))
        {
            if (current.Path.Length > shortest)
                continue;
            
            if (current.Position == keypad[finish])
            {
                if (current.Path.Length < shortest)
                {
                    paths.Clear();
                    shortest = current.Path.Length;
                }
                
                paths.Add(current.Path);
                continue;
            }
            
            foreach (var direction in Directions)
            {
                var delta = Deltas[direction];
                var position = new Position
                {
                    Row = current.Position.Row + delta.Row, 
                    Column = current.Position.Column + delta.Column
                };
                
                if (keypad.Values.Any(x => x == position))
                {
                    queue.Enqueue((position, current.Path + direction));
                }
            }
        }
        
        return paths.Select(p => new string(p + 'A')).ToList();
    }
    
    private const char Up = '^';
    private const char Down = 'v';
    private const char Left = '<';
    private const char Right = '>';
    
    private static readonly char[] Directions = [Up, Down, Left, Right];
    private static readonly Dictionary<char, Position> Deltas = new()
    {
        { Up, new Position(-1, 0) },
        { Down, new Position(1, 0) },
        { Left, new Position(0, -1) },
        { Right, new Position(0, 1) }
    };
    
    private static readonly Dictionary<char, Position> Keypad = new()
    {
        { '7', new Position(0, 0) },
        { '8', new Position(0, 1) },
        { '9', new Position(0, 2) },
        { '4', new Position(1, 0) },
        { '5', new Position(1, 1) },
        { '6', new Position(1, 2) },
        { '1', new Position(2, 0) },
        { '2', new Position(2, 1) },
        { '3', new Position(2, 2) },
        { '0', new Position(3, 1) },
        { 'A', new Position(3, 2) },
    };
    
    private static readonly Dictionary<char, Position> InstructionPad = new()
    {
        { '^', new Position(0, 1) },
        { 'A', new Position(0, 2) },
        { '<', new Position(1, 0) },
        { 'v', new Position(1, 1) },
        { '>', new Position(1, 2) }
    };
}

internal readonly record struct Position(int Row, int Column);
