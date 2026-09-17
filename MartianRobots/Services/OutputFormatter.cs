using System;
using System.Collections.Generic;
using System.Text;
using MartianRobots.Models;

namespace MartianRobots.Services
{
    public static class OutputFormatter
    {
        public static string Format(IEnumerable<RobotResult> results) =>
            string.Join('\n', results);  
    }
}
