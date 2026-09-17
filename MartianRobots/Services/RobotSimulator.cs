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
        
        public IEnumerable<RobotResult> Execute(SimulationInput simulationInput)
        {
            var results = new List<RobotResult>(simulationInput.Robots.Count);

            SpaceGrid spaceGrid = new SpaceGrid(simulationInput.Grid.X, simulationInput.Grid.Y);
            
            foreach (var robotInstruction in simulationInput.Robots)
            {
                Robot robot = new Robot(robotInstruction.Coordinate, robotInstruction.Direction);
                foreach (char commandChar in robotInstruction.Commands)
                {
                    RobotCommands.GetCommand(commandChar).Execute(robot, spaceGrid);
                    
                    if (robot.IsLost)
                    {
                        break;
                    }
                }
                results.Add(new RobotResult(robot.Position, robot.Direction, robot.IsLost));
            }
            
            return results; 
        }

    }
}
