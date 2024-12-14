using ConsoleTables;
using System.Diagnostics;

namespace Shared;

public abstract class AbstractSolution
{
    public async Task SolveAsync()
    {
        var table = new ConsoleTable("Part", "Answer", "Time (ms)");
        
        var part1 = await TimeAsync(SolvePart1Async);
        var part2 = await TimeAsync(SolvePart2Async);
        
        table.AddRow("Part 1", part1.Answer, part1.ElapsedMilliseconds);
        table.AddRow("Part 2", part2.Answer, part2.ElapsedMilliseconds);

        table.Write(Format.MarkDown);

        Console.WriteLine();
    }
    
    protected abstract Task<string> SolvePart1Async();
    protected abstract Task<string> SolvePart2Async();
    
    private static async Task<SolutionResult> TimeAsync(Func<Task<string>> func)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await func();
        
        stopwatch.Stop();

        return new SolutionResult(result, stopwatch.Elapsed.TotalMilliseconds);
    }
}