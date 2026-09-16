using MartianRobots.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobots.Commands
{
    public interface IRobotCommand
    {
        void Execute(Robot robot, SpaceGrid spaceGrid);
    }
}
