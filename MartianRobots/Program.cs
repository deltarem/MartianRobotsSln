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

            string sampleInstructions = "5 3\n3 2 N\nFRRFLLFFRRFLL";
            var parsedInstructions = InstructionParser.Parse(sampleInstructions);

            RobotSimulator robotSimulator = new RobotSimulator();
            var output = robotSimulator.Execute(parsedInstructions.gridSize, parsedInstructions.robots);

            Console.WriteLine(output);
        }

      
    }
}
