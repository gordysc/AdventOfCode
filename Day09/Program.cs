using Shared;

var input = File.ReadAllText("Input.txt");
var solution = new Solution(input);

Console.WriteLine("Day 09");

await solution.SolveAsync();

internal sealed class Solution(string input) : AbstractSolution
{
    protected override Task<string> SolvePart1Async()
    {
        var diskMap = CreateDiskMap(input);
        var compacted = CompactDiskMap(diskMap);
        var checkSum = CalculateCheckSum(compacted);
        
        return Task.FromResult(checkSum);
    }

    protected override Task<string> SolvePart2Async()
    {
        var diskMap = CreateDiskMap(input);
        var compacted = CompactDiskMapV2(diskMap);
        var checkSum = CalculateCheckSum(compacted);
        
        return Task.FromResult(checkSum);
    }

    private static Block[] CreateDiskMap(string text)
    {
        var id = 0;
        var blocks = new List<Block>();
        
        for (var idx = 0; idx < text.Length; idx++)
        {
            var size = int.Parse(text[idx].ToString());
            
            if (idx % 2 == 0)
            {
                for (var loop = 0; loop < size; loop++)
                    blocks.Add(new FileBlock(id, size));
                id++;
            }
            else
            {
                for (var loop = 0; loop < size; loop++)
                    blocks.Add(new SpaceBlock(size));
            }
        }

        return blocks.ToArray();
    }
    
    private static Block[] CompactDiskMap(Block[] original)
    {
        var result = new Block[original.Length];
        
        Array.Copy(original, result, original.Length);
        
        var left = 0;
        var right = original.Length - 1;
        
        while (left < right)
        {
            while (result[left] is not SpaceBlock && left < right)
                left++;
            
            while (result[right] is SpaceBlock && left < right)
                right--;

            if (left < right)
            {
                result[left] = result[right];
                result[right] = new SpaceBlock(1);
            }
        }

        return result;
    }
    
    private static Block[] CompactDiskMapV2(Block[] original)
    {
        var result = new Block[original.Length];
        
        Array.Copy(original, result, original.Length);

        for (var idx = 0; idx < original.Length; idx++)
        {
            if (result[idx] is FileBlock)
                continue;
            
            var size = result[idx].Size;
            
            for (var i = result.Length - 1; i >= 0; i--)
            {
                if (i == idx)
                    break;
                
                if (result[i] is FileBlock fileBlock && fileBlock.Size <= size)
                {
                    Array.Copy(result, i - fileBlock.Size + 1, result, idx, fileBlock.Size);
                    Array.Fill(result, new SpaceBlock(fileBlock.Size), i - fileBlock.Size + 1, fileBlock.Size);
                    
                    var remaining = size - fileBlock.Size;
                    
                    if (remaining > 0)
                        Array.Fill(result, new SpaceBlock(remaining), idx + fileBlock.Size, remaining);
                    
                    break;
                }
            }
        }

        return result;
    }
    
    private static string CalculateCheckSum(Block[] diskMap) =>
        diskMap
        .Select((x, idx) => x is FileBlock block ? block.Id * idx : 0)
        .Sum()
        .ToString();
}

internal abstract record Block(int Size);

internal sealed record SpaceBlock(int Size) : Block(Size);

internal sealed record FileBlock(long Id, int Size) : Block(Size);
