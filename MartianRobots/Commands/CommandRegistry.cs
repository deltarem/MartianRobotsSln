using MartianRobots.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MartianRobots.Commands
{
    public static class RobotCommands
    {

        private static readonly IRobotCommand Left = new LeftCommand();
        private static readonly IRobotCommand Right = new RightCommand();
        private static readonly IRobotCommand Forward = new ForwardCommand();

        public static IRobotCommand GetCommand(char instruction) => instruction switch
        {
            'L' => Left,
            'R' => Right,
            'F' => Forward,
            _ => throw new ArgumentException($"Invalid instruction character: '{instruction}'", nameof(instruction))
        };

    }
}
