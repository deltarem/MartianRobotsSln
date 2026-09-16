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

        public SpaceGrid CreateSpaceGrid(string gridSize)
        {
            var gridSizeParts = gridSize.Split(' ');
            int gridWidth = int.Parse(gridSizeParts[0]);
            int gridHeight = int.Parse(gridSizeParts[1]);
            // Create a new SpaceGrid instance
            var spaceGrid = new SpaceGrid(gridWidth, gridHeight);

            return spaceGrid;
        }

        public Robot CreateRobot(string robotPosition)
        {
            var robotPositionParts = robotPosition.Split(' ');
            int robotX = int.Parse(robotPositionParts[0]);
            int robotY = int.Parse(robotPositionParts[1]);
            DirectionEnum robotDirection = Enum.Parse<DirectionEnum>(robotPositionParts[2]);
            // Create a new Robot instance
            var robot = new Robot(new Coordinate(robotX, robotY), robotDirection);

            return robot;
        }

 
        public string Execute(string gridSize, string robotPosition, string commandSequence)
        {
            SpaceGrid spaceGrid = CreateSpaceGrid(gridSize);
            Robot robot = CreateRobot(robotPosition);

            foreach (char commandChar in commandSequence)
            {

                RobotCommands.GetCommand(commandChar).Execute(robot, spaceGrid);
            }
            return $"{robot.Position.x} {robot.Position.y} {robot.Direction}" + (robot.IsLost ? " LOST" : "");
        }

    }
}
