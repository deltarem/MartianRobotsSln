using MartianRobots.Interfaces;
using MartianRobots.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobots.Commands
{
    public sealed class ForwardCommand : IRobotCommand
    {
        public void Execute(Robot robot, SpaceGrid spaceGrid)
        {
            var next = robot.NextPosition();

            if (!spaceGrid.IsOutOfBounds(next))
            {
                robot.MoveTo(next);
                return;
            }

            if (spaceGrid.HasScent(robot.Position))
                return;                          // scented square: ignore the instruction

            spaceGrid.AddScent(robot.Position);
            robot.UpdateIsLost();

        }
    }
}
