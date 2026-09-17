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

            try
            {
                Console.WriteLine("Enter instructions. When finished, press Enter on the last line, then Ctrl+Z, then Enter.");


                var parsedInstructions = InstructionParser.Parse(Console.In.ReadToEnd());
                var results = new RobotSimulator().Execute(parsedInstructions);

                Console.WriteLine("The output is:");
                Console.WriteLine(OutputFormatter.Format(results));
            }
            catch (InvalidInputException ex)
            {
                Console.Error.WriteLine($"Invalid input. {ex.Message}");
            }
        }



      
    }
}
