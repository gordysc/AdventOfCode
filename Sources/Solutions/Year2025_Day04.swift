//
//  File.swift
//  AdventOfCode
//
//  Created by Luke Gordon on 12/3/25.
//

import Framework

extension Year2025 {
    private final class WeightedGrid {
        private var grid: [[Character]]
        private let width: Int
        private let height: Int
        
        private static let deltas: [(Int, Int)] = [
            (-1, -1), (-1, 0), (-1, 1),
            ( 0, -1),          ( 0, 1),
            ( 1, -1), ( 1, 0), ( 1, 1)
        ]
        
        init(grid: [[Character]], width: Int, height: Int) {
            self.grid = grid
            self.width = width
            self.height = height
        }
        
        func canRemove(x: Int, y: Int) -> Bool {
            guard grid[y][x] == "@" else { return false }
            
            let neighbors = getNeighbors(x: x, y: y)
            let rolls = neighbors.filter { $0 == "@" }.count
            
            return rolls < 4
        }
        
        func remove(x: Int, y: Int) -> Bool {
            let removable = canRemove(x: x, y: y)
            if removable {
                grid[y][x] = "."
            }
            return removable
        }
        
        private func getNeighbors(x: Int, y: Int) -> [Character] {
            var result: [Character] = []
            
            for (dx, dy) in Self.deltas {
                let nx = x + dx
                let ny = y + dy
                
                if nx >= 0 && nx < width && ny >= 0 && ny < height {
                    result.append(grid[ny][nx])
                }
            }
            
            return result
        }
    }
    
    /// Day 4
    /// https://adventofcode.com/2025/day/4
    public struct Day04: Solution {
        public static let year = 2025
        public static let day = 4
        public static let title = "Day 4"
        
        
        public init() {}
        
        public func part1(_ input: String) -> Any {
            var total = 0
            var grid = input.grid
            
            var mx = grid[0].count
            var my = grid.count
            
            var weighted = WeightedGrid.init(grid: grid, width: mx, height: my)
            
            for y in 0...(my-1) {
                total += (0...mx-1).count(where: { weighted.canRemove(x: $0, y: y) })
            }
            
            return total
        }
        
        public func part2(_ input: String) -> Any {
            var total = 0
            var grid = input.grid
            
            var mx = grid[0].count
            var my = grid.count
            
            var weighted = WeightedGrid.init(grid: grid, width: mx, height: my)
            var execute = true
            
            while (execute)
            {
                var previous = total
                
                for y in 0...(my-1) {
                    total += (0...mx-1).count(where: { weighted.remove(x: $0, y: y) })
                }
                
                execute = previous != total
            }
            
            return total
        }
    }
}
