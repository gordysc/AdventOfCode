using Shared;

var input = File
    .ReadAllLines("Input.txt")
    .Select(x => x.ToCharArray())
    .ToArray();

var solution = new Solution(input);

Console.WriteLine("Day 20");

await solution.SolveAsync();

internal sealed class Solution(char[][] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var track = CreateRaceTrack(input);
        var answer = 0;

        for (var i = 0; i < track.Count - 1; i++)
        {
            for (var j = i + 1; j < track.Count; j++)
            {
                if (CalculateDistance(track[i], track[j]) != 2)
                    continue;

                if (j - i > 100)
                    answer++;
            }
        }
        
        return Task.FromResult(answer.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        var track = CreateRaceTrack(input);
        var answer = 0;

        for (var i = 0; i < track.Count - 100 - 1; i++)
        {
            for (var j = i + 101; j < track.Count; j++)
            {
                var distance = CalculateDistance(track[i], track[j]);
                 
                if (distance is < 2 or > 20)
                    continue;

                if (j - i >= distance + 100)
                    answer++;
            }
        }
        
        return Task.FromResult(answer.ToString());
    }
    
    private static int CalculateDistance(Location a, Location b)
    {
        return Math.Abs(a.Row - b.Row) + Math.Abs(a.Column - b.Column);
    }
    
    private static readonly (int RX, int CX)[] Deltas = [(0, 1), (1, 0), (0, -1), (-1, 0)];

    private static List<Location> CreateRaceTrack(char[][] input)
    {
        var cursor = FindStart(input, 'S');
        var track = new List<Location> { cursor };

        while (input[cursor.Row][cursor.Column] != 'E')
        {
            cursor = Deltas
                .Select(x => new Location { Row = cursor.Row + x.RX, Column = cursor.Column + x.CX })
                .Where(x => x.Row >= 0 && x.Row < input.Length && x.Column >= 0 && x.Column < input[x.Row].Length)
                .First(x => input[x.Row][x.Column] != '#' && !track.Contains(x));
            
            track.Add(cursor);
        }
        
        return track;
    }

    private static Location FindStart(char[][] input, char c)
    {
        for (var row = 0; row < input.Length; row++)
        {
            if (Array.IndexOf(input[row], c) is var column && column != -1)
                return new Location(row, column);
        }
        
        throw new InvalidOperationException($"Could not find location for: {c}");
    }
}

internal readonly record struct Location(int Row, int Column);
