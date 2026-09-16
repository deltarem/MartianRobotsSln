using MartianRobots.Models;
using MartianRobots.Services;
using System.Reflection.PortableExecutable;
namespace MartianRobotsTest
{
    public class RobotSimulationTests
    {
        [Fact]
        public void ProcessInstructions_ProducesExpectedOutput_ForSampleInput()
        {

            // Arrange
            RobotSimulator robotSimulator = new RobotSimulator();
            string gridSize = "5 3";
            string robotPosition = "1 1 E";
            string commandSequence = "RFRFRFRF";

            string expectedOutput = "1 1 E";



            // Act
            var output = robotSimulator.Execute(gridSize, robotPosition, commandSequence);             
            // Assert
            Assert.Equal(expectedOutput, output);

        }
    }
}
