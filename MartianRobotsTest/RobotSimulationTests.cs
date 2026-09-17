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

        [Theory]
        [InlineData("", 1, "empty")]
        [InlineData("5", 1, "maxX maxY")]
        [InlineData("5 x", 1, "integer")]
        [InlineData("5 3\n1 1", 2, "Line 2: each robot needs a position line")]
        [InlineData("5 3\n1 1 Q\nF", 2, "direction")]
        [InlineData("5 3\n1 1 N\nFXF", 3, "unknown instruction")]
        [InlineData("5 3\n1 1 N", 2, "followed by")]
        public void Parse_RejectsBadInput(string input, int line, string messageFragment)
        {
            var ex = Assert.Throws<InvalidInputException>(() => InstructionParser.Parse(input));
            Assert.Equal(line, ex.LineNumber);
            Assert.Contains(messageFragment, ex.Message);
        }

        [Fact]
        public void Parse_RejectsInstructionOf100Chars()
        {
            var input = "5 3\n1 1 N\n" + new string('F', 100);
            var ex = Assert.Throws<InvalidInputException>(() => InstructionParser.Parse(input));
            Assert.Equal(3, ex.LineNumber);
        }

        [Fact]
        public void Parse_AcceptsCrlfAndExtraWhitespace()
        {
            var input = "5  3\r\n\r\n1 1   E\r\nRFRFRFRF\r\n";
            var parsed = InstructionParser.Parse(input);
            Assert.Equal(new Coordinate(1, 1), parsed.Robots[0].Coordinate);
        }
    }
   
}
