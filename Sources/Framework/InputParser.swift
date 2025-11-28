import Foundation

/// Extensions for parsing puzzle input strings
public extension String {
    /// Split input into lines, removing empty trailing lines
    var lines: [String] {
        components(separatedBy: .newlines)
            .reversed()
            .drop(while: { $0.isEmpty })
            .reversed()
    }

    /// Parse all integers from the input (one per line)
    var ints: [Int] {
        lines.compactMap { Int($0) }
    }

    /// Parse all integers from the input (one per line) as Int64
    var longs: [Int64] {
        lines.compactMap { Int64($0) }
    }

    /// Split input into paragraphs (separated by blank lines)
    var paragraphs: [[String]] {
        components(separatedBy: "\n\n")
            .map { $0.lines }
    }

    /// Parse input as a 2D character grid
    var grid: [[Character]] {
        lines.map { Array($0) }
    }

    /// Parse input as a 2D integer grid (single digits)
    var digitGrid: [[Int]] {
        lines.map { $0.compactMap { $0.wholeNumberValue } }
    }

    /// Extract all integers from the string (including negative numbers)
    var allInts: [Int] {
        let regex = try! Regex(#"-?\d+"#)
        return matches(of: regex).compactMap { Int(self[$0.range]) }
    }

    /// Trimmed version of the string
    var trimmed: String {
        trimmingCharacters(in: .whitespacesAndNewlines)
    }
}

/// Common grid operations
public extension Array where Element == [Character] {
    /// Get character at position, or nil if out of bounds
    subscript(safe row: Int, col: Int) -> Character? {
        guard row >= 0, row < count,
              col >= 0, col < self[row].count else { return nil }
        return self[row][col]
    }

    /// Number of rows in the grid
    var rows: Int { count }

    /// Number of columns in the grid (assumes rectangular)
    var cols: Int { first?.count ?? 0 }

    /// Find all positions of a character
    func positions(of char: Character) -> [(row: Int, col: Int)] {
        var result: [(Int, Int)] = []
        for row in indices {
            for col in self[row].indices {
                if self[row][col] == char {
                    result.append((row, col))
                }
            }
        }
        return result
    }
}
