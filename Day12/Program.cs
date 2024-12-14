using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 12");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var regions = GetRegions();
        var price = regions.Sum(x => x.Price);
        
        return Task.FromResult(price.ToString());
    }
    
    protected override Task<string> SolvePart2Async()
    {
        var regions = GetRegions();
        var price = regions.Sum(x => x.BulkPrice);
        
        return Task.FromResult(price.ToString());
    }

    private List<Region> GetRegions()
    {
        var rows = input.Length;
        var columns = input[0].Length;
        
        var visited = new bool[rows, columns];
        var regions = new List<Region>();
        
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                if (visited[row, column])
                    continue;
                
                var plant = input[row][column];
                
                visited[row, column] = true;
                
                var plots = new List<Plot>();
                var queue = new Queue<Plot>();
                
                queue.Enqueue(new Plot(row, column));

                while (queue.Count > 0)
                {
                    var plot = queue.Dequeue();
                    
                    plots.Add(plot);

                    if (plot.Row > 0 && visited[plot.Row - 1, plot.Column] is false && input[plot.Row - 1][plot.Column] == plant)
                    {
                        visited[plot.Row - 1, plot.Column] = true;
                        queue.Enqueue(plot with { Row = plot.Row - 1 });
                    }
                    
                    if (plot.Row < rows - 1 && visited[plot.Row + 1, plot.Column] is false && input[plot.Row + 1][plot.Column] == plant)
                    {
                        visited[plot.Row + 1, plot.Column] = true;
                        queue.Enqueue(plot with { Row = plot.Row + 1 });
                    }
                    
                    if (plot.Column > 0 && visited[plot.Row, plot.Column - 1] is false && input[plot.Row][plot.Column - 1] == plant)
                    {
                        visited[plot.Row, plot.Column - 1] = true;
                        queue.Enqueue(plot with { Column = plot.Column - 1 });
                    }
                    
                    if (plot.Column < columns - 1 && visited[plot.Row, plot.Column + 1] is false && input[plot.Row][plot.Column + 1] == plant)
                    {
                        visited[plot.Row, plot.Column + 1] = true;
                        queue.Enqueue(plot with { Column = plot.Column + 1 });
                    }
                }
                
                regions.Add(new Region(plots));
            }
        }
        
        return regions;
    }
}

internal readonly record struct Plot(int Row, int Column);

internal sealed class Region(IReadOnlyList<Plot> plots)
{
    private int Area => plots.Count;
    
    public int Price => Area * CalculatePerimeter();
    public int BulkPrice => Area * CalculateVertices();

    private int CalculatePerimeter()
    {
        var perimeter = 0;
        
        foreach (var plot in plots)
        {
            if (plots.Any(x => x.Row == plot.Row - 1 && x.Column == plot.Column) is false)
                perimeter++;
            if (plots.Any(x => x.Row == plot.Row + 1 && x.Column == plot.Column) is false)
                perimeter++;
            if (plots.Any(x => x.Row == plot.Row && x.Column == plot.Column - 1) is false)
                perimeter++;
            if (plots.Any(x => x.Row == plot.Row && x.Column == plot.Column + 1) is false)
                perimeter++;
        }
        
        return perimeter;
    }

    private int CalculateVertices()
    {
        var vertices = 0;
        
        foreach (var plot in plots)
        {
            var top = plots.Any(x => x.Row == plot.Row - 1 && x.Column == plot.Column);
            var bottom = plots.Any(x => x.Row == plot.Row + 1 && x.Column == plot.Column);
            var left = plots.Any(x => x.Row == plot.Row && x.Column == plot.Column - 1);
            var right = plots.Any(x => x.Row == plot.Row && x.Column == plot.Column + 1);

            var topLeft = plots.Any(x => x.Row == plot.Row - 1 && x.Column == plot.Column - 1);
            var topRight = plots.Any(x => x.Row == plot.Row - 1 && x.Column == plot.Column + 1);
            var bottomLeft = plots.Any(x => x.Row == plot.Row + 1 && x.Column == plot.Column - 1);
            var bottomRight = plots.Any(x => x.Row == plot.Row + 1 && x.Column == plot.Column + 1);
            
            if (top is false && left is false)
                vertices++;
            if (top && left && topLeft == false)
                vertices++;
            
            if (top is false && right is false)
                vertices++;
            if (top && right && topRight == false)
                vertices++;
            
            if (bottom is false && left is false)
                vertices++;
            if (bottom && left && bottomLeft == false)
                vertices++;
            
            if (bottom is false && right is false)
                vertices++;
            if (bottom && right && bottomRight == false)
                vertices++;
        }

        return vertices;
    }
}