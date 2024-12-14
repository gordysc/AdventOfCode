using System.Text;
using System.Text.RegularExpressions;
using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 14");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    private const string Pattern = @"p=(-?\d+),(-?\d+)\s*v=(-?\d+),(-?\d+)";
    
    private const int Width = 101;
    private const int Height = 103;
    
    protected override Task<string> SolvePart1Async()
    {
        const int time = 100;
        
        var robots = ParseRobots(input);
        
        for (var loop = 0; loop < time; loop++)
        {
            foreach (var robot in robots)
                robot.Move();
        }
        
        var q1 = robots.Count(robot => robot is { X: < Width / 2, Y: < Height / 2 });
        var q2 = robots.Count(robot => robot is { X: > Width / 2, Y: < Height / 2 });
        var q3 = robots.Count(robot => robot is { X: < Width / 2, Y: > Height / 2 });
        var q4 = robots.Count(robot => robot is { X: > Width / 2, Y: > Height / 2 });
        
        var safety = q1 * q2 * q3 * q4;
        
        return Task.FromResult(safety.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var robots = ParseRobots(input);
        var time = 0;

        while (IsChristmasTree(robots) == false)
        {
            time++;
            
            foreach (var robot in robots)
                robot.Move();
        }
        
        return Task.FromResult(time.ToString());
    }

    private static bool IsChristmasTree(List<Robot> robots)
    {
        if (IsMaxEntropy(robots) is false)
            return false;
        
        var positions = robots.Select(r => (r.X, r.Y)).ToHashSet();

        for (var row = 0; row < Height; row++)
        {
            var sb = new StringBuilder();
            
            for (var col = 0; col < Width; col++)
            {
                sb.Append(positions.Contains((col, row)) ? '#' : ' ');
            }

            if (sb.ToString().Trim().Contains("##########"))
                return true;
        }
        
        return false;
    }
    
    // performance improvement from reddit
    private static bool IsMaxEntropy(List<Robot> robots) => 
        robots.Count == robots.Select(r => (r.X, r.Y)).ToHashSet().Count;

    private static List<Robot> ParseRobots(string[] lines)
    {
        var robots = new List<Robot>();
        
        foreach (var line in lines)
        {
            var match = Regex.Match(line, Pattern);

            var px = int.Parse(match.Groups[1].Value);
            var py = int.Parse(match.Groups[2].Value);

            var dx = int.Parse(match.Groups[3].Value);
            var dy = int.Parse(match.Groups[4].Value);
            
            robots.Add(new Robot(px, py, dx, dy));
        }

        return robots;
    }
}

internal sealed class Robot(int x, int y, int dx, int dy)
{
    private const int Width = 101;
    private const int Height = 103;
    
    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;

    public void Move()
    {
        X = ((X + dx) % Width + Width) % Width;
        Y = ((Y + dy) % Height + Height) % Height;
    }
}