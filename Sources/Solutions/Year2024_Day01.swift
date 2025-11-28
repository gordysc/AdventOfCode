import Framework

public enum Year2024 {}

extension Year2024 {
    /// Day 1: Historian Hysteria
    /// https://adventofcode.com/2024/day/1
    public struct Day01: Solution {
        public static let year = 2024
        public static let day = 1
        public static let title = "Historian Hysteria"

        public init() {}

        public func part1(_ input: String) -> Any {
            let (left, right) = parseInput(input)
            let sortedLeft = left.sorted()
            let sortedRight = right.sorted()

            return zip(sortedLeft, sortedRight)
                .map { abs($0 - $1) }
                .reduce(0, +)
        }

        public func part2(_ input: String) -> Any {
            let (left, right) = parseInput(input)
            let rightCounts = Dictionary(grouping: right, by: { $0 })
                .mapValues { $0.count }

            return left
                .map { $0 * (rightCounts[$0] ?? 0) }
                .reduce(0, +)
        }

        private func parseInput(_ input: String) -> (left: [Int], right: [Int]) {
            var left: [Int] = []
            var right: [Int] = []

            for line in input.lines {
                let nums = line.allInts
                if nums.count >= 2 {
                    left.append(nums[0])
                    right.append(nums[1])
                }
            }

            return (left, right)
        }
    }
}
