using Shared;

var input = File
    .ReadAllLines("Input.txt")
    .Select(x => x.ToCharArray())
    .ToArray();

var solution = new Solution(input);

Console.WriteLine("Day 04");

await solution.SolveAsync();

internal sealed class Solution(char[][] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var length = input[0].Length;
        var height = input.Length;

        var total = 0;
        
        var coordinates = new List<Coordinate>();

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] == 'X')
                {
                    coordinates.Add(new Coordinate(row, col));
                }
            }
        }

        foreach (var (row, column) in coordinates)
        {
            // Check forward
            if (column <= length - 4 && input[row][column + 1] == 'M' && input[row][column + 2] == 'A' && input[row][column + 3] == 'S')
                total++;
            // Check backward
            if (column >= 3 && input[row][column - 1] == 'M' && input[row][column - 2] == 'A' && input[row][column - 3] == 'S')
                total++;
            // Check upward
            if (row >= 3 && input[row - 1][column] == 'M' && input[row - 2][column] == 'A' && input[row - 3][column] == 'S')
                total++;
            // Check downward
            if (row <= height - 4 && input[row + 1][column] == 'M' && input[row + 2][column] == 'A' && input[row + 3][column] == 'S')
                total++;
            // Check diagonal down right
            if (row <= height - 4 && column <= length - 4 && input[row + 1][column + 1] == 'M' && input[row + 2][column + 2] == 'A' && input[row + 3][column + 3] == 'S')
                total++;
            // Check diagonal down left
            if (row <= height - 4 && column >= 3 && input[row + 1][column - 1] == 'M' && input[row + 2][column - 2] == 'A' && input[row + 3][column - 3] == 'S')
                total++;
            // Check diagonal up right
            if (row >= 3 && column <= length - 4 && input[row - 1][column + 1] == 'M' && input[row - 2][column + 2] == 'A' && input[row - 3][column + 3] == 'S')
                total++;
            // Check diagonal up left
            if (row >= 3 && column >= 3 && input[row - 1][column - 1] == 'M' && input[row - 2][column - 2] == 'A' && input[row - 3][column - 3] == 'S')
                total++;
        }
        
        return Task.FromResult(total.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var length = input[0].Length;
        var height = input.Length;

        var total = 0;
        
        var coordinates = new List<Coordinate>();

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] == 'A')
                {
                    coordinates.Add(new Coordinate(row, col));
                }
            }
        }

        foreach (var (row, column) in coordinates)
        {
            if (row < 1 || row > height - 2 || column < 1 || column > length - 2)
                continue;

            // M.S
            // .A.
            // M.S
            if (input[row - 1][column - 1] == 'M' && input[row + 1][column + 1] == 'S' && input[row - 1][column + 1] == 'S' && input[row + 1][column - 1] == 'M')
                total++;
            
            // S.S
            // .A.
            // M.M
            if (input[row - 1][column - 1] == 'S' && input[row + 1][column + 1] == 'M' && input[row - 1][column + 1] == 'S' && input[row + 1][column - 1] == 'M')
                total++;
            
            // M.M
            // .A.
            // S.S
            if (input[row - 1][column - 1] == 'M' && input[row + 1][column + 1] == 'S' && input[row - 1][column + 1] == 'M' && input[row + 1][column - 1] == 'S')
                total++;
            
            // S.M
            // .A.
            // S.M
            if (input[row - 1][column - 1] == 'S' && input[row + 1][column + 1] == 'M' && input[row - 1][column + 1] == 'M' && input[row + 1][column - 1] == 'S')
                total++;
        }
        
        return Task.FromResult(total.ToString());
    }
}

internal sealed record Coordinate(int Row, int Column);