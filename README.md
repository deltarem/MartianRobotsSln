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

Blank lines are ignored. Tokens on a line may be separated by any amount of whitespace. Both `\n` and `\r\n` line endings are accepted.

## Output format

One line per robot, in input order: the final `x y direction`, with ` LOST` appended if the robot fell off the grid. Output lines are separated by `\n` on every platform so that piped output is byte-for-byte comparable with expected files.

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

## Assumptions

The brief leaves a few things open. These are the calls I made:

| Situation | Decision |
|---|---|
| Robot's start position is outside the grid | Treated as **invalid input** (error with line number), not as a robot that is immediately lost. |
| Grid larger than 50×50 or with a negative dimension | Invalid input. |
| Instruction string of 100 characters or more | Invalid input (the spec says "less than 100"). |
| Instruction character other than `L`, `R`, `F` | Invalid input; the whole run is rejected rather than skipping the character. |
| Direction letter in lower case (`n`, `e` …) | Invalid input — directions are case-sensitive. |
| Odd number of robot lines (position without instructions) | Invalid input. |
| Empty instruction string | Allowed — the robot simply stays where it started. |
| Output line endings | Always `\n`, regardless of platform. |
| Errors | Written to `stderr` with the offending line number; the process exits with code `1`. Successful runs exit `0`. |

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Build and test

```bash
git clone https://github.com/deltarem/MartianRobotsSln.git
cd MartianRobotsSln
dotnet build
dotnet test
```

### Run

Pipe an input file in:

```bash
dotnet run --project MartianRobots < input.txt
```

Or run it interactively — the program prints a prompt (to `stderr`, so it never pollutes redirected output) and reads until end-of-input:

```bash
dotnet run --project MartianRobots
```

Finish input with `Ctrl+Z` then Enter on Windows, or `Ctrl+D` on Linux/macOS.

Invalid input produces a message like the following on `stderr` and an exit code of `1`:

```
Invalid input. Line 2: start position 9 9 is outside the 5x3 grid
```

## Project structure

```
MartianRobotsSln.slnx
├── MartianRobots/                     # Console application
│   ├── Program.cs                     # Entry point: stdin → parser → simulator → stdout; exit codes
│   ├── Interfaces/
│   │   └── IRobotCommand.cs           # Command contract: Execute(Robot, SpaceGrid)
│   ├── Models/
│   │   ├── Direction.cs               # Direction enum + TurnLeft / TurnRight / GetNextPosition extensions
│   │   ├── Robot.cs                   # Position, heading, lost flag; ignores commands once lost
│   │   ├── SpaceGrid.cs               # Grid bounds (0..50), scent set, out-of-bounds check
│   │   └── RobotTypes.cs              # Coordinate, RobotInstruction, SimulationInput, RobotResult
│   ├── Services/
│   │   ├── InstructionParser.cs       # Text → SimulationInput, with line-numbered validation
│   │   ├── InvalidInputException.cs   # Carries the line number of the offending input
│   │   ├── RobotSimulator.cs          # Runs each robot's commands, returns structured results
│   │   └── OutputFormatter.cs         # RobotResult list → spec output text
│   └── Commands/
│       ├── LeftCommand.cs
│       ├── RightCommand.cs
│       ├── ForwardCommand.cs          # Edge detection and scent logic
│       └── RobotCommands.cs           # Maps 'L' / 'R' / 'F' to shared command instances
└── MartianRobotsTest/                 # xUnit tests
    └── RobotSimulationTests.cs
```

## Design notes

- **Pipeline of three stages** – `InstructionParser` turns text into a typed `SimulationInput`; `RobotSimulator` turns that into a list of `RobotResult`; `OutputFormatter` turns results into text. Each stage is a pure function of the previous one, so the core can be tested without the console and could be fronted by something other than stdin/stdout without changing it.
- **Typed boundaries** – the parser produces domain objects (`Coordinate`, `Direction`, `SpaceGrid`), not tuples or strings, so nothing downstream has to re-parse or re-validate.
- **Validation happens once, in the parser** – every problem with the input is reported as an `InvalidInputException` carrying the line number. `SpaceGrid` still enforces its own 0–50 invariant in its constructor; the parser wraps that so the user sees a line number rather than a bare `ArgumentException`.
- **Command pattern** – each instruction character maps to an `IRobotCommand`. The three commands are stateless (all state lives in `Robot` and `SpaceGrid`), so they are created once and shared. Adding a new instruction means one class and one entry in `RobotCommands`.
- **Lost robots ignore commands** – `Robot.Execute` is a no-op once `IsLost` is set, so the simulator loop can't accidentally keep moving a lost robot. This is enforced in the domain object rather than by a `break` in the caller.
- **Scents live on the grid** – `SpaceGrid` owns the set of scented squares; `ForwardCommand` is the single place the rule "don't fall off where someone already did" is applied.
- **`SimulationInput` is single-use** – it carries the live `SpaceGrid` the simulation mutates (scents accumulate), so the same input object should not be run twice.

## Tests

`dotnet test` runs the xUnit suite, which covers:

- the single-robot and three-robot sample scenarios from the brief, end-to-end through parser, simulator and formatter;
- parser rejection of empty input, malformed grid and position lines, non-integer values, bad direction letters, unknown instruction characters, instruction strings of 100+ characters, and an odd number of robot lines — each asserting the reported line number;
- CRLF line endings and extra whitespace being accepted;
- a lost robot ignoring all subsequent commands, both at the `Robot` level and through the simulator.

## What I would build around it if it went further

The brief asked for the program only, so none of this is implemented. If it were going into a real system:

- **Input/output abstractions** – an `IInstructionSource` / `IResultSink` pair so the same core can be driven from a file, an HTTP endpoint or a message queue. The current `Program.cs` would become one thin adapter among several.
- **Dependency injection** – register the parser, simulator and formatter behind interfaces with `Microsoft.Extensions.Hosting`, so an API host can inject them and tests can substitute fakes.
- **Web API** – a minimal-API `POST /simulate` accepting the raw instruction text (or a JSON model) and returning `RobotResult[]` as JSON; `RobotResult` is already a plain record so it serialises as-is.
- **Persistence** – if runs need to be stored, a small table of runs and results keyed by an ID. Because `SpaceGrid` state is per-run, nothing else needs to change.
- **Property-based tests** with FsCheck for the direction tables (four left turns always return to the start; `L` then `R` is the identity; a robot on an unbounded grid never becomes lost).
- **CI** – a GitHub Actions workflow running `dotnet build` and `dotnet test` on push, with `dotnet format --verify-no-changes` to keep style consistent.
- **Observability** – structured logging of each robot's final state and any rejected input, which becomes useful once this runs as a service.

## Use of AI tooling

I used AI at two points. At the start, to scaffold the project structure and draft the initial types, tests and this README; the core logic was written by me. After the first working version, I used it as a code reviewer: it read the repository and suggested improvements to code quality (typed models instead of tuples, structured results instead of string output, line-numbered validation, moving the lost-robot guard into `Robot`, sharing command instances) and pointed out gaps in the unit tests, which I then implemented. The design decisions and the assumptions above are my own.

## Licence

This project is provided as-is for demonstration purposes.
