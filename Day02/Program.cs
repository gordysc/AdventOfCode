using Shared;

var input = File.ReadAllLines("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 02");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var total = 0;

        foreach (var line in input)
        {
            var values = line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            if (IsOrdered(values) && IsSafeDifference(values))
                total++;
        }
        
        return Task.FromResult(total.ToString());
    }
    
    protected override Task<string> SolvePart2Async()
    {
        var total = 0;

        foreach (var line in input)
        {
            var values = line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            for (var index = 0; index < values.Length; index++)
            {
                var data = values.Where((_, i) => i != index).ToArray();

                if (IsOrdered(data) && IsSafeDifference(data))
                {
                    total++;
                    break;
                }
            }
        }
        
        return Task.FromResult(total.ToString());
    }

    private static bool IsOrdered(int[] array)
    {
        if (array.Length == 1)
            return true;

        var isIncreasing = true;
        var isDecreasing = true;

        for (var i = 1; i < array.Length; i++)
        {
            if (array[i] < array[i - 1])
                isIncreasing = false;
            if (array[i] > array[i - 1])
                isDecreasing = false;

            if (!isIncreasing && !isDecreasing)
                return false;
        }

        return true;
    }

    private static bool IsSafeDifference(int[] values)
    {
        var differences = new int[values.Length - 1];
            
        for (var loop = 1; loop < values.Length; loop++)
            differences[loop - 1] = Math.Abs(values[loop] - values[loop - 1]);

        return differences.All(x => x is >= 1 and <= 3);
    }
}