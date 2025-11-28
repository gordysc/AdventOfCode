# 🎄 Advent of Code in Swift 🎄

My solutions to [Advent of Code](https://adventofcode.com) puzzles, written in Swift.

## ⭐ Features

- 🎁 Clean, modular solution framework
- ⏱️ Built-in timing for each part
- 📊 Benchmarking mode for performance testing
- 🧪 Support for sample and puzzle inputs

## 🛷 Requirements

- Swift 6.0+
- macOS 13+

## 🎅 Usage

Build and run with Swift Package Manager:

```bash
# Build the project
swift build

# Run all solutions
swift run aoc

# Run a specific year
swift run aoc --year 2024

# Run a specific day
swift run aoc --year 2024 --day 1

# Use sample input instead of puzzle input
swift run aoc --year 2024 --day 1 --sample

# Run benchmarks (100 iterations by default)
swift run aoc --year 2024 --day 1 --benchmark

# Custom number of benchmark iterations
swift run aoc --year 2024 --day 1 --benchmark --iterations 1000
```

## 🎁 Project Structure

```
Sources/
├── CLI/              # Command-line interface
├── Framework/        # Core solution protocol and utilities
└── Solutions/        # Puzzle solutions organized by year
    ├── Year2024/     # Input files for 2024
    ├── Year2025/     # Input files for 2025
    └── *.swift       # Solution implementations
```

## ❄️ Adding a New Solution

1. Create a new solution file in `Sources/Solutions/` following the naming convention `Year{YYYY}_Day{DD}.swift`

2. Implement the `Solution` protocol:

```swift
import Framework

extension Year2024 {
    public struct Day02: Solution {
        public static let year = 2024
        public static let day = 2
        public static let title = "Puzzle Title"

        public init() {}

        public func part1(_ input: String) -> Any {
            // Your solution here
        }

        public func part2(_ input: String) -> Any {
            // Your solution here
        }
    }
}
```

3. Register the solution in `Sources/Solutions/Registry.swift`

4. Add your input files:
   - `Sources/Solutions/Year2024/Day02/puzzle.txt` - Your puzzle input
   - `Sources/Solutions/Year2024/Day02/sample.txt` - Sample input from the puzzle description

## 🌟 Progress

### 2024

| Day | Title              | Stars |
| --- | ------------------ | ----- |
| 1   | Historian Hysteria | ⭐⭐  |

### 2025

| Day | Title | Stars |
| --- | ----- | ----- |
| 1   | -     |       |

---

🎄 _Happy coding and may your solutions be ever optimal!_ 🎄
