using Shared;

var input = File
    .ReadAllLines("Input.txt")
    .Select(x => x.ToCharArray())
    .ToArray();

var solution = new Solution(input);

Console.WriteLine("Day 03");

solution.Solve();

internal sealed class Solution(char[][] input) : AbstractSolution
{
    protected override string SolvePart1()
    {
        var slope = new Slope(3, 1);
        var count = CountTrees(input, slope);

        return count.ToString();
    }
    
    protected override string SolvePart2()
    {
        List<Slope> slopes = [new(1, 1), new(3, 1), new(5, 1), new(7, 1), new(1, 2)];
        
        var counts = slopes.Select(x => CountTrees(input, x));
        var total = counts.Aggregate(1L, (acc, i) => acc * i);

        return total.ToString();
    }

    private static long CountTrees(char[][] grid, Slope slope)
    {
        var queue = new Queue<Position>();
        var count = 0;
        
        queue.Enqueue(new Position(0, 0));

        while (queue.TryDequeue(out var position))
        {
            if (position.Row >= grid.Length)
                continue;

            if (grid[position.Row][position.Column] == '#')
                count++;
            
            queue.Enqueue(new Position
            {
                Row = position.Row + slope.Down, 
                Column = (position.Column + slope.Right) % grid[position.Row].Length
            });
        }

        return count;
    }
}

internal readonly record struct Slope(int Right, int Down);
internal readonly record struct Position(int Row, int Column);