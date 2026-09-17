using MartianRobots.Interfaces;
using MartianRobots.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobots.Commands
{
    public sealed class LeftCommand : IRobotCommand
    {
        public void Execute(Robot robot, SpaceGrid spaceGrid)
        {
            robot.TurnLeft();
        }
    }
}
