using MartianRobots.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobots.Interfaces
{
    public interface IRobotCommand
    {
        void Execute(Robot robot, SpaceGrid spaceGrid);
    }
}
