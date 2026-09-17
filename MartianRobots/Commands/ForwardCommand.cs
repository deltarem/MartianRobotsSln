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
            var nextPosition = robot.NextPosition();

            if (spaceGrid.IsOutOfBounds(nextPosition))
            {
                if (!spaceGrid.HasScent(robot.Position))
                {
                    spaceGrid.AddScent(robot.Position);
                    robot.UpdateIsLost();
                }
            }
            else
            {
                robot.MoveTo(nextPosition);
            }

        }
    }
}
