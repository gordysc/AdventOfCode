import Framework

extension Year2025 {
    /// Day 2:
    /// https://adventofcode.com/2025/day/2
    public struct Day02: Solution {
        public static let year = 2025
        public static let day = 2
        public static let title = "Day 2"
        
        
        public init() {}
        
        public func part1(_ input: String) -> Any {
            var total = 0
            
            for range in input.trimmingCharacters(in: .whitespacesAndNewlines).split(separator: ",")
            {
                let boundaries = range.split(separator: "-")
                
                guard boundaries.count == 2,
                      let start = Int(boundaries[0]),
                      let end = Int(boundaries[1]) else {
                    continue
                }
                
                for value in start...end
                {
                    let text = String(value)
                    
                    guard text.count % 2 == 0 else { continue }
                    
                    let midpoint = text.count / 2
                    let index = text.index(text.startIndex, offsetBy: midpoint)
                    
                    if text[..<index] == text[index...] {
                        total += value
                    }
                }
            }
            
            return total
        }
        
        public func part2(_ input: String) -> Any {
            var total = 0
            
            for range in input.trimmingCharacters(in: .whitespacesAndNewlines).split(separator: ",")
            {
                let boundaries = range.split(separator: "-")
                
                guard boundaries.count == 2,
                      let start = Int(boundaries[0]),
                      let end = Int(boundaries[1]) else {
                    continue
                }
                
                for value in start...end
                {
                    let text = String(value)
                    
                    guard text.count > 1 else { continue }
                    
                    let max = text.count / 2
                    var match = false
                    
                    for chunkSize in 1...max
                    {
                        guard text.count.isMultiple(of: chunkSize) else { continue }
                        
                        let chunk = text.prefix(chunkSize)
                        
                        if text.chunks(of: chunkSize).allSatisfy( { $0 == chunk } ) {
                            match = true
                            break
                        }
                    }
                    
                    if match
                    {
                        total += value
                    }
                }
            }
            
            return total
        }
    }
}

extension String {
    func chunks(of size: Int) -> [Substring] {
        precondition(size > 0, "Chunk size must be positive")
        
        var result: [Substring] = []
        var index = startIndex
        
        while index < endIndex {
            let next = self.index(index, offsetBy: size, limitedBy: endIndex) ?? endIndex
            result.append(self[index..<next])
            index = next
        }
        
        return result
    }
}
