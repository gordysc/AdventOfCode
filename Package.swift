// swift-tools-version: 6.0
import PackageDescription

let package = Package(
    name: "AdventOfCode",
    platforms: [.macOS(.v13)],
    dependencies: [
        .package(url: "https://github.com/apple/swift-argument-parser", from: "1.3.0"),
    ],
    targets: [
        .target(
            name: "Framework",
            path: "Sources/Framework"
        ),
        .target(
            name: "Solutions",
            dependencies: ["Framework"],
            path: "Sources/Solutions",
            resources: [
                .copy("Year2024"),
                .copy("Year2025"),
            ]
        ),
        .executableTarget(
            name: "AdventOfCode",
            dependencies: [
                "Framework",
                "Solutions",
                .product(name: "ArgumentParser", package: "swift-argument-parser"),
            ],
            path: "Sources/CLI"
        ),
    ]
)
