using MartianRobots.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MartianRobots.Services
{
    public static class InstructionParser
    {
        public static ( (int, int) gridSize, ((int robotX, int robotY, string robotDirection) robotPosition, string commandSequence)[] robots) Parse(string instructions)
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
            (int maxX, int maxY) gridSize = (int.Parse(gridSizeParts[0]), int.Parse(gridSizeParts[1]));


            var robotLines = lines.Skip(1).ToList();
            if (robotLines.Count % 2 != 0)
            {
                throw new Exception("Each robot needs a position line followed by an instruction line");
            }

            var robots = new List<((int robotX, int robotY, string robotDirection) robotPosition, string commandSequence)>();
            
            for (var i = 0; i < robotLines.Count; i += 2)
            {
                var robotPositionParts = robotLines[i].Split(' ');
                int robotX = int.Parse(robotPositionParts[0]);
                int robotY = int.Parse(robotPositionParts[1]);
                DirectionEnum robotDirection = Enum.Parse<DirectionEnum>(robotPositionParts[2]);

                string commandSequence = robotLines[i + 1];

                robots.Add(((robotX, robotY, robotDirection.ToString()), commandSequence));
            }   

            return (gridSize, robots.ToArray());
        }


    }
}
