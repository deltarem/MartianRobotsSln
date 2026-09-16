# Martian Robots

A C# / .NET 10 console application that simulates robots moving on a bounded rectangular grid on Mars. Robots follow simple instruction sequences; if one drives off the edge it is marked as **LOST**, but it leaves a "scent" behind that stops later robots from falling off at the same spot.

## The problem

The surface of Mars is modelled as a rectangular grid. The bottom-left corner is `0 0` and the top-right corner is given as input (maximum `50 50`). Each robot has a position (`x y`) and a facing direction (`N`, `S`, `E` or `W`), and receives a string of instructions:

| Instruction | Effect |
|---|---|
| `L` | Turn left 90° (stay on the same square) |
| `R` | Turn right 90° (stay on the same square) |
| `F` | Move one square forward in the current direction |

Robots are processed one at a time; the next robot only starts once the previous one has finished.

A robot that moves off the grid is **lost** forever, and the program reports its last valid position followed by `LOST`. When a robot is lost it leaves a scent on the square it fell from. Any later robot standing on a scented square that receives an instruction which would take it off the grid simply ignores that instruction and carries on.

## Input format

```
<maxX> <maxY>            # upper-right coordinates of the grid
<x> <y> <direction>      # robot 1 start position and heading
<instructions>           # robot 1 instruction string (L, R, F)
<x> <y> <direction>      # robot 2 ...
<instructions>
...
```

Blank lines are ignored.

## Output format

One line per robot, in input order: the final `x y direction`, with ` LOST` appended if the robot fell off the grid.

## Example

Input:

```
5 3
1 1 E
RFRFRFRF
3 2 N
FRRFLLFFRRFLL
0 3 W
LLFFFLFLFL
```

Output:

```
1 1 E
3 3 N LOST
2 3 S
```

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### 1. Clone the repository

```bash
git clone https://github.com/deltarem/MartianRobotsSln.git
cd MartianRobotsSln
```

### 2. Verify .NET installation

```bash
dotnet --version
```

For .NET 10 Preview:

```bash
dotnet --list-sdks
```

### 3. Restore packages

```bash
dotnet restore
```

### 4. Build the solution

```bash
dotnet build
```

Or Release build:

```bash
dotnet build -c Release
```

### 5. Run unit tests

```bash
dotnet test
```

Tests are written with [xUnit](https://xunit.net/) and cover the single-robot and multi-robot sample scenarios above.

### 6. Run the application

```bash
dotnet run --project MartianRobots
```

The user can enter instructions directly in the console. When finished, press Enter on the last line, then `Ctrl+Z`, then Enter.

> On Linux / macOS, use `Ctrl+D` instead of `Ctrl+Z`.

You can also pipe a file straight in:

```bash
dotnet run --project MartianRobots < input.txt
```

## Project structure

```
MartianRobotsSln.slnx
├── MartianRobots/                 # Console application
│   ├── Program.cs                 # Entry point – reads stdin, parses, runs the simulation
│   ├── Models/
│   │   ├── Robot.cs               # Position, direction, turning and movement
│   │   └── SpaceGrid.cs           # Grid bounds, scent tracking, out-of-bounds checks
│   ├── Services/
│   │   ├── InstructionParser.cs   # Parses raw text input into grid size + robot definitions
│   │   └── RobotSimulator.cs      # Runs each robot's instructions and builds the output
│   └── Commands/
│       ├── IRobotCommand.cs       # Command interface
│       ├── LeftCommand.cs
│       ├── RightCommand.cs
│       ├── ForwardCommand.cs      # Handles edge detection and scent logic
│       └── RobotCommands.cs       # Maps instruction characters to command objects
└── MartianRobotsTest/             # xUnit test project
    └── RobotSimulationTests.cs
```

## Design notes

- **Command pattern** – each instruction character (`L`, `R`, `F`) maps to a small `IRobotCommand` implementation, so adding a new instruction means adding one class and one entry in `RobotCommands`.
- **Separation of concerns** – parsing (`InstructionParser`), domain state (`Robot`, `SpaceGrid`) and orchestration (`RobotSimulator`) are kept apart, which keeps the core logic testable without touching the console.
- **Scents live on the grid** – `SpaceGrid` owns the set of scented coordinates, so the rule "don't fall off where someone already did" is enforced in one place (`ForwardCommand`).
- **Validation** – grid dimensions must be non-negative and no larger than 50×50; unknown instruction characters throw an `ArgumentException`.

## Use of AI tooling

I used AI to set up the project structure, draft the initial types and tests, and prepare this README document. I did not use AI to develop the code. I could have added more unit tests and validation, but as this was a 2–3-hour project, I tried to keep it concise.

## Licence

This project is provided as-is for demonstration purposes.
