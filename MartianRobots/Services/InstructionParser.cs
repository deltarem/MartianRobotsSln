using MartianRobots.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MartianRobots.Services
{

    /*
     * 
     *     public sealed record RobotInstruction(Coordinate Start, DirectionEnum Facing, string Commands);
    public sealed record SimulationInput(SpaceGrid Grid, IReadOnlyList<RobotInstruction> Robots);

     */
    public static class InstructionParser
    {
        public static SimulationInput Parse(string instructions)
        {

            var lines = instructions
               .Split('\n')
               .Select(l => l.Trim())
               .Where(l => l.Length > 0)
               .ToList();

            if (lines.Count == 0)
            {
                throw new Exception("Instructions are empty");
            }

            var gridSizeParts = lines[0].Split(' ');
            if (gridSizeParts.Length != 2)
            {
                throw new Exception($"Line 1: expected 'maxX maxY', got '{lines[0]}'");
            }
            GridSize gridSize = new GridSize(int.Parse(gridSizeParts[0]), int.Parse(gridSizeParts[1]));


            var robotLines = lines.Skip(1).ToList();
            if (robotLines.Count % 2 != 0)
            {
                throw new Exception("Each robot needs a position line followed by an instruction line");
            }

            var robots = new List<RobotInstruction>();
            
            for (var i = 0; i < robotLines.Count; i += 2)
            {
                var robotPositionParts = robotLines[i].Split(' ');
                int robotX = int.Parse(robotPositionParts[0]);
                int robotY = int.Parse(robotPositionParts[1]);
                DirectionEnum robotDirection = Enum.Parse<DirectionEnum>(robotPositionParts[2]);

                string commandSequence = robotLines[i + 1];

                robots.Add(new RobotInstruction(new Coordinate(robotX, robotY), robotDirection, commandSequence));                
            }   

            return new SimulationInput(gridSize, robots);
        }


    }
}
