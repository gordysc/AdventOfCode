using System.Text.RegularExpressions;
using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 13");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    private const string ButtonRegex = @"X\+(\d+),\s*Y\+(\d+)";
    private const string PrizeRegex = @"X=(\d+),\s*Y=(\d+)";
    
    protected override Task<string> SolvePart1Async()
    {
        var tokens = 0L;
        
        foreach (var chunk in input.Chunk(4))
        {
            var buttonA = ParseLine(chunk[0], ButtonRegex);
            var buttonB = ParseLine(chunk[1], ButtonRegex);
            var prizes = ParseLine(chunk[2], PrizeRegex);

            var p1 = new Coefficients(buttonA.X, buttonB.X, prizes.X);
            var p2 = new Coefficients(buttonA.Y, buttonB.Y, prizes.Y);
            
            tokens += CalculateTokens(p1, p2);
        }
        
        return Task.FromResult(tokens.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var tokens = 0L;
        
        foreach (var chunk in input.Chunk(4))
        {
            var buttonA = ParseLine(chunk[0], ButtonRegex);
            var buttonB = ParseLine(chunk[1], ButtonRegex);
            var prizes = ParseLine(chunk[2], PrizeRegex);

            var p1 = new Coefficients(buttonA.X, buttonB.X, prizes.X + 10_000_000_000_000);
            var p2 = new Coefficients(buttonA.Y, buttonB.Y, prizes.Y + 10_000_000_000_000);
            
            tokens += CalculateTokens(p1, p2);
        }
        
        return Task.FromResult(tokens.ToString());
    }
    
    private static long CalculateTokens(Coefficients p1, Coefficients p2)
    {
        var determinant = p1.A * p2.B - p2.A * p1.B;
        
        // No solution if this is zero
        if (determinant == 0)
            return 0;

        double determinantA = p1.C * p2.B - p2.C * p1.B;
        double determinantB = p1.A * p2.C - p2.A * p1.C;

        var a = determinantA / determinant;
        var b = determinantB / determinant;

        // If either are not an integer, there is no combination that satisfies the equation
        if (a % 1 != 0 || b % 1 != 0)
            return 0;

        return (long)(a * 3 + b);
    }
    
    private static (long X, long Y) ParseLine(string line, string pattern)
    {
        var match = Regex.Match(line, pattern);

        var x = long.Parse(match.Groups[1].Value);
        var y = long.Parse(match.Groups[2].Value);
        
        return (x, y);
    }
}

internal readonly record struct Coefficients(long A, long B, long C);