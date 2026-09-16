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
            robot.MoveTo(robot.GetNextPosition());
        }
    }
}
