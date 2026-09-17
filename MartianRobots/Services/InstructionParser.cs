using MartianRobots.Models;
using static System.Net.Mime.MediaTypeNames;

namespace MartianRobots.Services
{
    public static class InstructionParser
    {
        private const int MaxInstructionLength = 99;
        public static SimulationInput Parse(string instructions)
        {

            var lines = instructions
             .Split(["\r\n", "\n"], StringSplitOptions.None)
             .Select((raw, i) => (Number: i + 1, Text: raw.Trim()))
             .Where(l => l.Text.Length > 0)
             .ToList();

            if (lines.Count == 0)
                throw new InvalidInputException(1, "Instructions are empty");

          
            var gridSizeParts = lines[0].Text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            if (gridSizeParts.Length != 2)
            {
                throw new InvalidInputException(1, $"expected 'maxX maxY', got '{lines[0]}'");
            }
           

            GridSize gridSize = new GridSize(ParseInt(gridSizeParts[0], lines[0].Number, "X"), ParseInt(gridSizeParts[1], lines[0].Number, "Y"));


            var robotLines = lines.Skip(1).ToList();
            if (robotLines.Count % 2 != 0)
                throw new InvalidInputException(robotLines[^1].Number,
                    "each robot needs a position line followed by an instruction line");



            var robots = new List<RobotInstruction>(robotLines.Count / 2);
            for (var i = 0; i < robotLines.Count; i += 2)
                robots.Add(ParseRobot(robotLines[i], robotLines[i + 1], gridSize));

            return new SimulationInput(gridSize, robots);
        }

        private static RobotInstruction ParseRobot(
        (int Number, string Text) positionLine,
        (int Number, string Text) commandLine,
        GridSize gridSize)
        {
            var parts = positionLine.Text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                throw new InvalidInputException(positionLine.Number,
                    $"expected 'x y direction', got '{positionLine.Text}'");

            var coordinate = new Coordinate(
                ParseInt(parts[0], positionLine.Number, "x"),
                ParseInt(parts[1], positionLine.Number, "y"));


            if (!Enum.TryParse<Direction>(parts[2], ignoreCase: false, out var direction))
                throw new InvalidInputException(positionLine.Number,
                    $"direction must be N, E, S or W, got '{parts[2]}'");

            var commands = commandLine.Text;
            if (commands.Length > MaxInstructionLength)
                throw new InvalidInputException(commandLine.Number,
                    $"instruction string must be shorter than 100 characters, got {commands.Length}");

            var bad = commands.FirstOrDefault(c => c is not ('L' or 'R' or 'F'));
            if (bad != default)
                throw new InvalidInputException(commandLine.Number,
                    $"unknown instruction '{bad}' (allowed: L, R, F)");

            return new RobotInstruction(coordinate, direction, commands);
        }

        private static int ParseInt(string token, int lineNumber, string name)
        {
            if (!int.TryParse(token, out var value))
                throw new InvalidInputException(lineNumber, $"{name} must be an integer, got '{token}'");
            return value;
        }


    }
}
