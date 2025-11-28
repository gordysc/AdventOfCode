import Foundation

/// Protocol that all Advent of Code solutions must conform to
public protocol Solution: Sendable {
    /// The year this solution is for (e.g., 2024)
    static var year: Int { get }

    /// The day this solution is for (1-25)
    static var day: Int { get }

    /// Title/name of the puzzle (optional)
    static var title: String { get }

    /// Initialize the solution
    init()

    /// Solve part 1 of the puzzle
    func part1(_ input: String) -> Any

    /// Solve part 2 of the puzzle
    func part2(_ input: String) -> Any
}

// Default implementation for title
public extension Solution {
    static var title: String { "Day \(day)" }
}

/// Metadata about a solution for display and filtering
public struct SolutionInfo: Sendable {
    public let year: Int
    public let day: Int
    public let title: String
    public let create: @Sendable () -> any Solution

    public init(year: Int, day: Int, title: String, create: @escaping @Sendable () -> any Solution) {
        self.year = year
        self.day = day
        self.title = title
        self.create = create
    }
}
