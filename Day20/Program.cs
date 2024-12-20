using Shared;

var input = File.ReadAllLines("Input.txt");

var solution = new Solution(input);

Console.WriteLine("Day 20");

await solution.SolveAsync();

internal sealed class Solution(string[] input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var raceTrack = CreateRaceTrack(input.Select(x => x.ToCharArray()).ToArray());
        var walls = 0;

        for (var x = 0; x < raceTrack.Count; x++)
        {
            var track = raceTrack[x];
            
            for (var j = x + 1; j < raceTrack.Count; j++)
            {
                var candidate = raceTrack[j];
                var distance = CalculateDistance(track, candidate);
                var difference = candidate.Distance - track.Distance;

                if (distance == 2 && difference > 100)
                    walls++;
            }
        }

        return Task.FromResult(walls.ToString());
    }

    protected override Task<string> SolvePart2Async()
    {
        return Task.FromResult("Not implemented");
    }
    
    private static int CalculateDistance(Track a, Track b)
    {
        return Math.Abs(a.Location.Row - b.Location.Row) + 
               Math.Abs(a.Location.Column - b.Location.Column);
    }
    
    private static readonly (int RX, int CX)[] Deltas = [(0, 1), (1, 0), (0, -1), (-1, 0)];

    private static List<Track> CreateRaceTrack(char[][] input)
    {
        var cursor = FindLocation(input, 'S');
        var start = new Track(cursor, 0);
        
        var visited = new HashSet<Location> { cursor };
        var result = new List<Track> { start };

        while (input[cursor.Row][cursor.Column] != 'E')
        {
            cursor = Deltas
                .Select(x => new Location { Row = cursor.Row + x.RX, Column = cursor.Column + x.CX })
                .Where(x => x.Row >= 0 && x.Row < input.Length && x.Column >= 0 && x.Column < input[x.Row].Length)
                .First(x => input[x.Row][x.Column] != '#' && !visited.Contains(x));
            
            visited.Add(cursor);
            result.Add(new Track(cursor, result.Count));
        }
        
        return result;
    }

    private static Location FindLocation(char[][] input, char c)
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
internal readonly record struct Track(Location Location, int Distance);