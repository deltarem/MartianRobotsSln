using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MartianRobots.Commands
{
    public static class RobotCommands
    {
        public static IRobotCommand GetCommand(char instruction) => instruction switch
        {
            'L' => new LeftCommand(),
            'R' => new RightCommand(),
            'F' => new ForwardCommand(),
            _ => throw new ArgumentException($"Invalid instruction character: {instruction}")
        };
    }
}
