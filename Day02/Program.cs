using System.Text.RegularExpressions;
using Shared;

var input = File.ReadAllLines("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 02");

solution.Solve();

internal sealed partial class Solution(string[] input) : AbstractSolution
{
    [GeneratedRegex(@"(\d+)-(\d+) ([a-z]): ([a-z]+)")]
    private static partial Regex Pattern();
    private static readonly Regex Regex = Pattern();
    
    protected override string SolvePart1()
    {
        var answer = input.Count(line =>
        {
            var match = Regex.Match(line);

            var min = int.Parse(match.Groups[1].Value);
            var max = int.Parse(match.Groups[2].Value);

            var character = match.Groups[3].Value[0];
            var password = match.Groups[4].Value;

            var count = password.Count(x => character == x);

            return count >= min && count <= max;
        });

        return answer.ToString();
    }
    
    protected override string SolvePart2()
    {
        var answer = input.Count(line =>
        {
            var match = Regex.Match(line);

            var idx1 = int.Parse(match.Groups[1].Value) - 1;
            var idx2 = int.Parse(match.Groups[2].Value) - 1;

            var character = match.Groups[3].Value[0];
            var password = match.Groups[4].Value;

            var m1 = password[idx1] == character ? 1 : 0;
            var m2 = password[idx2] == character ? 1 : 0;

            return m1 + m2 == 1;
        });

        return answer.ToString();
    }
}