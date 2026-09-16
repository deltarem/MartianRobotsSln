using MartianRobots.Commands;
using MartianRobots.Models;
using static System.Net.Mime.MediaTypeNames;
namespace MartianRobots
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string inputInstructions = $"5 3{Environment.NewLine}1 1 E{Environment.NewLine}RFRFRFRF";

            ProcessInstructions(inputInstructions);

            Console.WriteLine("Hello, World!");
        }

        public static void ProcessInstructions(string inputText)
        {
            using var reader = new StringReader(inputText);

            string line1 = reader.ReadLine(); // First line
            Console.WriteLine(line1);
            string line2 = reader.ReadLine(); // Second line
            Console.WriteLine(line2);
            string line3 = reader.ReadLine(); // Third line
            Console.WriteLine(line3);

            // Split the input into lines
            var lines = line1.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Parse the grid size from the first line
            var gridSizeParts = lines[0].Split(' ');
            int gridWidth = int.Parse(gridSizeParts[0]);
            int gridHeight = int.Parse(gridSizeParts[1]);
            // Create a new SpaceGrid instance
            var spaceGrid = new SpaceGrid(gridWidth, gridHeight);
            // Parse the robot's initial position and direction from the second line
            var robotPositionParts = line2.Split(' ');
            int robotX = int.Parse(robotPositionParts[0]);
            int robotY = int.Parse(robotPositionParts[1]);
            DirectionEnum robotDirection = Enum.Parse<DirectionEnum>(robotPositionParts[2]);
            // Create a new Robot instance
            var robot = new Robot(new Coordinate(robotX, robotY), robotDirection);
            // Get the command sequence from the third line
            string commandSequence = line3;
            // Execute each command in the sequence
            foreach (char commandChar in commandSequence)
            {
          
                RobotCommands.GetCommand(commandChar).Execute(robot, spaceGrid);
            }
            // Output the final position and direction of the robot
            Console.WriteLine($"{robot.Position.x} {robot.Position.y} {robot.Direction}" + (robot.IsLost ? " LOST" : ""));
        }
    }
}
