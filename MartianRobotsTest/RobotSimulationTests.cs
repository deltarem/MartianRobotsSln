using MartianRobots.Models;
using MartianRobots.Services;
using System.Reflection.PortableExecutable;
namespace MartianRobotsTest
{
    public class RobotSimulationTests
    {
        [Fact]
        public void SampleInstructions_ProducesExpectedOutput()
        {

            // Arrange
            RobotSimulator robotSimulator = new RobotSimulator();
            string sampleInstructions = "5 3\n1 1 E\nRFRFRFRF";
            string expectedOutput = "1 1 E";


            // Act
            var output = OutputFormatter.Format(robotSimulator.Execute(InstructionParser.Parse(sampleInstructions)));             
            // Assert
            Assert.Equal(expectedOutput, output);
        }



       
        [Fact]
        public void MultipleRobots_SampleInstructions_ProducesExpectedOutput()
        {

            // Arrange
            RobotSimulator robotSimulator = new RobotSimulator();
            string sampleInstructions = "5 3\n1 1 E\nRFRFRFRF\n3 2 N\nFRRFLLFFRRFLL\n0 3 W\nLLFFFLFLFL";

            string expectedOutput = "1 1 E\n3 3 N LOST\n2 3 S";

            // Act
            var output = OutputFormatter.Format(robotSimulator.Execute(InstructionParser.Parse(sampleInstructions)));
            // Assert
            Assert.Equal(expectedOutput, output);

        }
    }
   
}
