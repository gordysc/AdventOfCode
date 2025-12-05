//
//  File.swift
//  AdventOfCode
//
//  Created by Luke Gordon on 12/4/25.
//

import Framework

extension Year2025 {
    /// Day 5
    /// https://adventofcode.com/2025/day/5
    public struct Day05: Solution {
        public static let year = 2025
        public static let day = 5
        public static let title = "Day 5"
        
        
        public init() {}
        
        public func part1(_ input: String) -> Any {
                let partitions = input.components(separatedBy: "\n\n")
                let rangeLines = lines(from: partitions[0])
                let idLines = lines(from: partitions[1])

                var ranges: [IngredientRange] = []

                for line in rangeLines {
                    let parts = line.split(separator: "-")
                    let min = Int64(parts[0])!
                    let max = Int64(parts[1])!
                    
                    ranges.append(IngredientRange(min: min, max: max))
                }

                var total = 0
            
                for id in idLines {
                    let value = Int64(id)!
                    if ranges.contains(where: { $0.includes(value) }) {
                        total += 1
                    }
                }

                return String(total)
            }


            public func part2(_ input: String) -> Any {
                let partitions = input.components(separatedBy: "\n\n")
                let rangeLines = lines(from: partitions[0])

                var ranges: [IngredientRange] = []

                for line in rangeLines {
                    let parts = line.split(separator: "-")
                    let min = Int64(parts[0])!
                    let max = Int64(parts[1])!
                    
                    ranges.append(IngredientRange(min: min, max: max))
                }

                // Combine overlapping ranges
                var combined: [IngredientRange] = []

                for range in ranges {
                    let overlapping = combined.filter { $0.overlaps(range) }

                    if overlapping.isEmpty {
                        combined.append(range)
                    } else {
                        // Start with range, merge all overlapping ones into it
                        var newRange = range
                        for item in overlapping {
                            newRange = newRange.combine(with: item)
                        }

                        // Remove old ones, add merged one
                        combined.removeAll { overlapping.contains($0) }
                        combined.append(newRange)
                    }
                }

                // Sum total size
                let total = combined.reduce(0) { $0 + $1.size }
                
                return String(total)
            }

            // Utility to split by lines safely
            private func lines(from text: String) -> [String] {
                return text.split(whereSeparator: \.isNewline).map(String.init)
            }

            struct IngredientRange: Equatable {
                let min: Int64
                let max: Int64

                func includes(_ value: Int64) -> Bool {
                    return value >= min && value <= max
                }

                func overlaps(_ other: IngredientRange) -> Bool {
                    return !(other.max < min || other.min > max)
                }

                func combine(with other: IngredientRange) -> IngredientRange {
                    return IngredientRange(
                        min: Swift.min(min, other.min),
                        max: Swift.max(max, other.max)
                    )
                }

                var size: Int64 {
                    return max - min + 1
                }
            }
    }
}
