using Shared;

var input = File.ReadAllLines("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 07");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        long answer = 0;
        
        foreach (var line in input)
        {
            var parts = line.Split(':');
            var total = long.Parse(parts[0].Trim());
            var values = new List<long>(Array.ConvertAll(parts[1].Trim().Split(' '), long.Parse));

            if (CanFormTotal(total, values))
            {
                answer += total;
            }
        }
        
        return Task.FromResult(answer.ToString());
    }
    
    private static bool CanFormTotal(long target, List<long> values)
    {
        return CanCalibrate(target, values, 0, 0) || 
               CanCalibrate(target, values, 0, 1);
    }

    private static bool CanCalibrate(long target, List<long> values, int index, long currentResult)
    {
        if (currentResult == target && index == values.Count) 
            return true;

        if (index >= values.Count || currentResult > target) 
            return false;

        if (CanCalibrate(target, values, index + 1, currentResult + values[index]))
            return true;

        if (CanCalibrate(target, values, index + 1, currentResult * values[index]))
            return true;

        return false;
    }

    protected override Task<string> SolvePart2Async()
    {
        long answer = 0;
        
        foreach (var line in input)
        {
            var parts = line.Split(':');
            var total = long.Parse(parts[0].Trim());
            var values = new List<long>(Array.ConvertAll(parts[1].Trim().Split(' '), long.Parse));

            if (CanFormTotalV2(total, values))
            {
                answer += total;
            }
        }
        
        return Task.FromResult(answer.ToString());
    }
    
    private static bool CanFormTotalV2(long target, List<long> values)
    {
        return CanCalibrateV2(target, values, 0, 0) ||
               CanCalibrateV2(target, values, 0, 1);   
    }

    private static bool CanCalibrateV2(long target, List<long> values, int index, long currentResult)
    {
        if (currentResult == target && index == values.Count) 
            return true;

        if (index >= values.Count || currentResult > target) 
            return false;

        if (CanCalibrateV2(target, values, index + 1, currentResult + values[index]))
            return true;

        if (CanCalibrateV2(target, values, index + 1, currentResult * values[index]))
            return true;
        
        if (CanCalibrateV2(target, values, index + 1, Combine(currentResult, values[index])))
            return true;

        return false;
    }

    private static long Combine(long a, long b) => long.Parse(a + b.ToString());
}
