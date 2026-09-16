using MartianRobots.Commands;
using MartianRobots.Models;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace MartianRobots.Services
{
    public class RobotSimulator
    {

        public SpaceGrid CreateSpaceGrid((int, int) gridSize)
        {
            int gridWidth = gridSize.Item1;
            int gridHeight = gridSize.Item2;
            // Create a new SpaceGrid instance
            var spaceGrid = new SpaceGrid(gridWidth, gridHeight);

            return spaceGrid;
        }

        public Robot CreateRobot(int robotX, int robotY, string direction)
        {
  
            DirectionEnum robotDirection = Enum.Parse<DirectionEnum>(direction);    
            // Create a new Robot instance
            var robot = new Robot(new Coordinate(robotX, robotY), robotDirection);

            return robot;
        }

 
        public string Execute((int, int) gridSize, ((int robotX, int robotY, string robotDirection) robotPosition, string commandSequence)[] robots)
        {
            SpaceGrid spaceGrid = CreateSpaceGrid(gridSize);
            string result = string.Empty;

            foreach (var robotData in robots)
            {
                var (robotPosition, commandSequence) = robotData;
                Robot robot = CreateRobot(robotPosition.robotX, robotPosition.robotY, robotPosition.robotDirection);
                foreach (char commandChar in commandSequence)
                {
                    RobotCommands.GetCommand(commandChar).Execute(robot, spaceGrid);
                    
                   // Console.WriteLine($"Robot Position: {robot.Position.x}, {robot.Position.y}, Direction: {robot.Direction}, IsLost: {robot.IsLost}");
                    if (robot.IsLost)
                    {
                        break;
                    }
                }
                result += $"{robot.Position.x} {robot.Position.y} {robot.Direction}" + (robot.IsLost ? " LOST" : "") + "\n";
            }
            
            return result.TrimEnd('\n');
        }

    }
}
