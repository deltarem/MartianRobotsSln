using MartianRobots.Commands;
using MartianRobots.Models;
using MartianRobots.Services;
using System.Reflection.PortableExecutable;
using static System.Net.Mime.MediaTypeNames;
namespace MartianRobots
{
    internal static class Program
    {
        static int Main(string[] args)
        {

        
            if (!Console.IsInputRedirected)
            {
                var eof = OperatingSystem.IsWindows() ? "Ctrl+Z then Enter" : "Ctrl+D";
                Console.Error.WriteLine($"Enter instructions, then press {eof} to finish.");
            }

            try
            {
                var input = InstructionParser.Parse(Console.In.ReadToEnd());
                var results = new RobotSimulator().Execute(input);
                Console.WriteLine(OutputFormatter.Format(results));
                return 0;
            }
            catch (InvalidInputException ex)
            {
                Console.Error.WriteLine($"Invalid input. {ex.Message}");
                return 1;
            }
        }



      
    }
}
