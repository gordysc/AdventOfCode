import ArgumentParser
import Framework
import Solutions

@main
struct AdventOfCode: ParsableCommand {
    static let configuration = CommandConfiguration(
        commandName: "aoc",
        abstract: "Run Advent of Code solutions",
        version: "1.0.0"
    )

    @Option(name: .shortAndLong, help: "Year to run (e.g., 2024)")
    var year: Int?

    @Option(name: .shortAndLong, help: "Day to run (1-25)")
    var day: Int?

    @Flag(name: .shortAndLong, help: "Run with sample input instead of puzzle input")
    var sample: Bool = false

    @Flag(name: .shortAndLong, help: "Run benchmarks (multiple iterations)")
    var benchmark: Bool = false

    @Option(name: .long, help: "Number of benchmark iterations")
    var iterations: Int = 100

    mutating func run() throws {
        let solutions = getSolutions(year: year, day: day)

        if solutions.isEmpty {
            if let year = year, let day = day {
                print("No solution found for \(year) Day \(day)")
            } else if let year = year {
                print("No solutions found for \(year)")
            } else if let day = day {
                print("No solutions found for Day \(day)")
            } else {
                print("No solutions found")
            }
            return
        }

        for info in solutions {
            runSolution(info)
        }
    }

    private func runSolution(_ info: SolutionInfo) {
        let inputType = sample ? "sample" : "puzzle"
        print("\n\(info.year) Day \(info.day): \(info.title)")
        print(String(repeating: "=", count: 50))

        guard let input = loadInput(year: info.year, day: info.day, sample: sample) else {
            print("Could not load \(inputType) input for \(info.year) Day \(info.day)")
            return
        }

        if input.trimmed.isEmpty {
            print("Input file is empty - add your puzzle input to:")
            print("  Sources/Solutions/Year\(info.year)/Day\(String(format: "%02d", info.day))/\(inputType).txt")
            return
        }

        let solution = info.create()

        if benchmark {
            runBenchmark(solution: solution, input: input)
        } else {
            let result = Framework.runSolution(solution, input: input)
            printResult(result)
        }
    }

    private func printResult(_ result: SolutionResult) {
        print("Part 1: \(result.part1)")
        print("  Time: \(result.part1Time.formatted)")
        print("Part 2: \(result.part2)")
        print("  Time: \(result.part2Time.formatted)")
        print("Total:  \(result.totalTime.formatted)")
    }

    private func runBenchmark(solution: any Solution, input: String) {
        print("Running \(iterations) iterations...")

        var part1Times: [Duration] = []
        var part2Times: [Duration] = []
        var lastResult: SolutionResult?

        let clock = ContinuousClock()

        for _ in 0..<iterations {
            var p1Result: Any = ""
            let p1Time = clock.measure {
                p1Result = solution.part1(input)
            }

            var p2Result: Any = ""
            let p2Time = clock.measure {
                p2Result = solution.part2(input)
            }

            part1Times.append(p1Time)
            part2Times.append(p2Time)

            lastResult = SolutionResult(
                part1: String(describing: p1Result),
                part2: String(describing: p2Result),
                part1Time: p1Time,
                part2Time: p2Time
            )
        }

        if let result = lastResult {
            print("Part 1: \(result.part1)")
            print("  Avg:  \(average(part1Times).formatted)")
            print("  Min:  \(part1Times.min()!.formatted)")
            print("  Max:  \(part1Times.max()!.formatted)")

            print("Part 2: \(result.part2)")
            print("  Avg:  \(average(part2Times).formatted)")
            print("  Min:  \(part2Times.min()!.formatted)")
            print("  Max:  \(part2Times.max()!.formatted)")
        }
    }

    private func average(_ durations: [Duration]) -> Duration {
        let totalAttos = durations.reduce(Int64(0)) { sum, d in
            sum + d.components.seconds * 1_000_000_000_000_000_000 + d.components.attoseconds
        }
        let avgAttos = totalAttos / Int64(durations.count)
        return Duration(secondsComponent: avgAttos / 1_000_000_000_000_000_000,
                       attosecondsComponent: avgAttos % 1_000_000_000_000_000_000)
    }
}
