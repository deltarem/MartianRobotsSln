using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobots.Services
{
    public sealed class InvalidInputException : Exception
    {
        public int LineNumber { get; }

        public InvalidInputException(int lineNumber, string message)
            : base($"Line {lineNumber}: {message}")
        {
            LineNumber = lineNumber;
        }
    }
}
