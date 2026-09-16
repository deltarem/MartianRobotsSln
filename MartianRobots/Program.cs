using MartianRobots.Commands;
using MartianRobots.Models;
using MartianRobots.Services;
using System.Reflection.PortableExecutable;
using static System.Net.Mime.MediaTypeNames;
namespace MartianRobots
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter instructions. When finished, press Enter on the last line, then Ctrl+Z, then Enter." );

            string input = Console.In.ReadToEnd();

            var parsedInstructions = InstructionParser.Parse(input);

            RobotSimulator robotSimulator = new RobotSimulator();
            var output = robotSimulator.Execute(parsedInstructions.gridSize, parsedInstructions.robots);

            Console.WriteLine("The output is:");
            Console.WriteLine(output);
        }

      
    }
}
