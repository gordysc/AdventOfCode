using ConsoleTables;
using System.Diagnostics;

namespace Shared;

public abstract class AbstractSolution
{
    public void Solve()
    {
        var table = new ConsoleTable("Part", "Answer", "Time (ms)");
        
        var part1 = Execute(SolvePart1);
        var part2 = Execute(SolvePart2);
        
        table.AddRow("Part 1", part1.Answer, part1.ElapsedMilliseconds);
        table.AddRow("Part 2", part2.Answer, part2.ElapsedMilliseconds);

        table.Write(Format.MarkDown);

        Console.WriteLine();
    }
    
    protected abstract string SolvePart1();
    protected abstract string SolvePart2();
    
    private static SolutionResult Execute(Func<string> func)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = func();
        
        stopwatch.Stop();

        return new SolutionResult(result, stopwatch.Elapsed.TotalMilliseconds);
    }
}