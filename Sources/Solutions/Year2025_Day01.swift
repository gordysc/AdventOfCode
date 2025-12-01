import Framework

public enum Year2025 {}

extension Year2025 {
    /// Day 1: [Title TBD]
    /// https://adventofcode.com/2025/day/1
    public struct Day01: Solution {
        public static let year = 2025
        public static let day = 1
        public static let title = "Day 1"

        public init() {}

        public func part1(_ input: String) -> Any {
            var position = 50;
            var zeros = 0;
            
            for line in input.lines
            {
                let direction = line.first!.description;
                let distance = Int(line.dropFirst())!;
                let modifier = direction == "R" ? 1 : -1;
                
                position += distance * modifier;
                position = (position + 100) % 100;
                
                if (position == 0)
                {
                    zeros += 1;
                }
            }
            
            return zeros.description;
        }

        public func part2(_ input: String) -> Any {
            var position = 50;
            var zeros = 0;

            for line in input.lines
            {
                let direction = line.first!.description;
                let distance = Int(line.dropFirst())!;
                let modifier = direction == "R" ? 1 : -1;
                
                let newPosition = direction == "L" ? (100 - position) % 100 : position;

                zeros += (newPosition + distance) / 100;
                position = (position + distance * modifier % 100 + 100) % 100;
            }

            return zeros.description;
        }
    }
}
