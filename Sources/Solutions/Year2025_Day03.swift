//
//  File.swift
//  AdventOfCode
//
//  Created by Luke Gordon on 12/3/25.
//

import Framework

extension Year2025 {
    /// Day 3
    /// https://adventofcode.com/2025/day/3
    public struct Day03: Solution {
        public static let year = 2025
        public static let day = 3
        public static let title = "Day 3"
        
        
        public init() {}
        
        public func part1(_ input: String) -> Any {
            var total = 0
            
            for line in input.split(whereSeparator: \.isNewline) {
                let joltage = Self.findJoltage(String(line), length: 2)
                
                total += Int(joltage)
            }
            
            return total
        }
        
        public func part2(_ input: String) -> Any {
            var total = 0
            
            for line in input.split(whereSeparator: \.isNewline) {
                let joltage = Self.findJoltage(String(line), length: 12)
                
                total += Int(joltage)
            }
            
            return total
        }
        
        private static func findJoltage(_ line: String, length: Int) -> Int64 {
            // Convert characters to digits
            let digits = line.compactMap { Int(String($0)) }

            var result = ""
            var index = 0

            for loop in stride(from: length - 1, through: 0, by: -1) {
                // window = digits[index ..< digits.count - loop]
                let end = digits.count - loop
                let window = digits[index..<end]

                // find max digit
                guard let maxDigit = window.max() else { continue }

                // append to result
                result.append(String(maxDigit))

                // move index to first occurrence of maxDigit starting from index, +1
                if let pos = digits[index...].firstIndex(of: maxDigit) {
                    index = pos + 1
                }
            }

            return Int64(result) ?? 0
        }
    }
}
