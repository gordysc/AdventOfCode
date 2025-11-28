import Foundation

/// Result from running a solution
public struct SolutionResult: Sendable {
    public let part1: String
    public let part2: String
    public let part1Time: Duration
    public let part2Time: Duration

    public var totalTime: Duration {
        part1Time + part2Time
    }

    public init(part1: String, part2: String, part1Time: Duration, part2Time: Duration) {
        self.part1 = part1
        self.part2 = part2
        self.part1Time = part1Time
        self.part2Time = part2Time
    }
}

/// Run a solution and measure timing
public func runSolution(_ solution: any Solution, input: String) -> SolutionResult {
    let clock = ContinuousClock()

    var part1Result: Any = ""
    let part1Time = clock.measure {
        part1Result = solution.part1(input)
    }

    var part2Result: Any = ""
    let part2Time = clock.measure {
        part2Result = solution.part2(input)
    }

    return SolutionResult(
        part1: String(describing: part1Result),
        part2: String(describing: part2Result),
        part1Time: part1Time,
        part2Time: part2Time
    )
}

/// Format a duration for display
public extension Duration {
    var formatted: String {
        let micros = Double(components.attoseconds) / 1_000_000_000_000
        let totalMicros = Double(components.seconds) * 1_000_000 + micros

        if totalMicros < 1000 {
            return String(format: "%.2f μs", totalMicros)
        } else if totalMicros < 1_000_000 {
            return String(format: "%.2f ms", totalMicros / 1000)
        } else {
            return String(format: "%.2f s", totalMicros / 1_000_000)
        }
    }
}
