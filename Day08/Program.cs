using System.Text;
using Shared;

var input = File
    .ReadAllLines("Input.txt")
    .Select(x => x.ToCharArray())
    .ToArray();

var solution = new Solution(input);

Console.WriteLine("Day 08");

await solution.SolveAsync();

internal sealed class Solution(char[][] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var antennas = new Dictionary<char, List<Point>>();
        
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                var token = input[y][x];
                
                if (token == '.')
                    continue;
                
                if (antennas.TryGetValue(token, out var points))
                    points.Add(new Point(x, y));
                else
                    antennas[token] = [new Point(x, y)];
            }
        }
        
        var width = input[0].Length;
        var height = input.Length;
        var antinodes = new HashSet<Point>();
        
        foreach (var (_, points) in antennas)
        {
            for (var i = 0; i < points.Count; i++)
            {
                for (var j = i + 1; j < points.Count; j++)
                {
                    var (x1, y1) = points[i];
                    var (x2, y2) = points[j];
                    
                    var dx = x2 - x1;
                    var dy = y2 - y1;
                    
                    var nx1 = x1 - dx;
                    var ny1 = y1 - dy;
                    
                    if (nx1 >= 0 && nx1 < width && ny1 >= 0 && ny1 < height)
                        antinodes.Add(new Point(nx1, ny1));
                    
                    var nx2 = x2 + dx;
                    var ny2 = y2 + dy;
                    
                    if (nx2 >= 0 && nx2 < width && ny2 >= 0 && ny2 < height)
                        antinodes.Add(new Point(nx2, ny2));
                }
            }
        }
        
        return Task.FromResult(antinodes.Count.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var antennas = new Dictionary<char, List<Point>>();
        
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                var token = input[y][x];
                
                if (token == '.')
                    continue;
                
                if (antennas.TryGetValue(token, out var points))
                    points.Add(new Point(x, y));
                else
                    antennas[token] = [new Point(x, y)];
            }
        }
        
        var width = input[0].Length;
        var height = input.Length;
        var antinodes = new HashSet<Point>();
        
        foreach (var (_, points) in antennas)
        {
            for (var i = 0; i < points.Count; i++)
            {
                for (var j = i + 1; j < points.Count; j++)
                {
                    var (x1, y1) = points[i];
                    var (x2, y2) = points[j];

                    var dx = x2 - x1;
                    var dy = y2 - y1;

                    var nx1 = x1 - dx;
                    var ny1 = y1 - dy;

                    while (nx1 >= 0 && nx1 < width && ny1 >= 0 && ny1 < height)
                    {
                        antinodes.Add(new Point(nx1, ny1));
                        
                        nx1 -= dx;
                        ny1 -= dy;
                    }

                    var nx2 = x2 + dx;
                    var ny2 = y2 + dy;

                    while (nx2 >= 0 && nx2 < width && ny2 >= 0 && ny2 < height)
                    {
                        antinodes.Add(new Point(nx2, ny2));

                        nx2 += dx;
                        ny2 += dy;
                    }
                    
                    antinodes.Add(new Point(x1, y1));
                    antinodes.Add(new Point(x2, y2));
                }
            }
        }

        return Task.FromResult(antinodes.Count.ToString());
    }
}

internal sealed record Point(int X, int Y);