import Foundation
import Framework

/// Registry of all available solutions
/// Add new solutions here as you implement them
public let allSolutions: [SolutionInfo] = [
    // Year 2024
    SolutionInfo(year: 2024, day: 1, title: "Historian Hysteria", create: { Year2024.Day01() }),

    // Year 2025
    SolutionInfo(year: 2025, day: 1, title: "Day 1", create: { Year2025.Day01() }),
    SolutionInfo(year: 2025, day: 2, title: "Day 2", create: { Year2025.Day02() }),
    SolutionInfo(year: 2025, day: 3, title: "Day 3", create: { Year2025.Day03() }),
    SolutionInfo(year: 2025, day: 4, title: "Day 4", create: { Year2025.Day04() }),
]

/// Get solutions filtered by year and/or day
public func getSolutions(year: Int? = nil, day: Int? = nil) -> [SolutionInfo] {
    allSolutions.filter { solution in
        (year == nil || solution.year == year) &&
        (day == nil || solution.day == day)
    }
    .sorted { ($0.year, $0.day) < ($1.year, $1.day) }
}

/// Load input file for a solution
public func loadInput(year: Int, day: Int, sample: Bool = false) -> String? {
    let filename = sample ? "sample" : "puzzle"
    let yearFolder = String(format: "Year%04d", year)
    let dayFolder = String(format: "Day%02d", day)

    guard let url = Bundle.module.url(
        forResource: filename,
        withExtension: "txt",
        subdirectory: "\(yearFolder)/\(dayFolder)"
    ) else {
        return nil
    }

    return try? String(contentsOf: url, encoding: .utf8)
}
