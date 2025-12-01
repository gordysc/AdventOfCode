import Framework

public enum Year2025 {}

extension Year2025 {
    /// Day 1: Secret Entrance
    /// https://adventofcode.com/2025/day/1
    public struct Day01: Solution {
        public static let year = 2025
        public static let day = 1
        public static let title = "Day 1"

        /// The circular track has 100 positions (0-99)
        private let trackLength = 100

        public init() {}

        /// Part 1: Count how many times we land exactly on position 0 after each rotation.
        /// We start at position 50 and move R (right/clockwise) or L (left/counter-clockwise).
        public func part1(_ input: String) -> Any {
            let rotations = parseInput(input)
            var position = 50
            var zeros = 0

            for (direction, distance) in rotations {
                let modifier = direction == "R" ? 1 : -1

                position = wrap(position + distance * modifier)

                if position == 0 {
                    zeros += 1
                }
            }

            return zeros
        }

        /// Part 2: Count how many times we pass through position 0 during each rotation.
        /// Unlike part 1, we count every crossing of 0, not just landing on it.
        public func part2(_ input: String) -> Any {
            let rotations = parseInput(input)
            var position = 50
            var zeros = 0

            for (direction, distance) in rotations {
                let modifier = direction == "R" ? 1 : -1
                // When moving left, mirror the position to calculate crossings as if moving right
                let effectivePosition = direction == "L" ? wrap(trackLength - position) : position

                // Integer division gives us how many complete laps (crossings of 0) occur
                zeros += (effectivePosition + distance) / trackLength
                position = wrap(position + distance * modifier)
            }

            return zeros
        }

        /// Parse input lines in format "R45" or "L30" into direction and distance tuples
        private func parseInput(_ input: String) -> [(direction: Character, distance: Int)] {
            input.lines.compactMap { line in
                guard let direction = line.first,
                      let distance = Int(line.dropFirst()) else { return nil }
                return (direction, distance)
            }
        }

        /// Wrap a value to stay within [0, trackLength) using modular arithmetic
        private func wrap(_ value: Int) -> Int {
            ((value % trackLength) + trackLength) % trackLength
        }
    }
}
