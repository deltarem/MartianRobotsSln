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

            string inputInstructions = $"5 3{Environment.NewLine}1 1 E{Environment.NewLine}RFRFRFRF";
            using var reader = new StringReader(inputInstructions);

            RobotSimulator robotSimulator = new RobotSimulator();
         
            Console.WriteLine(robotSimulator.Execute(reader.ReadLine(), reader.ReadLine(), reader.ReadLine()));
        }

      
    }
}
