using MartianRobots.Commands;
using MartianRobots.Models;
using MartianRobots.Services;
using System.Reflection.PortableExecutable;
namespace MartianRobotsTest
{
    public class RobotSimulatorTests
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

        [Fact]
        public void LostRobot_IgnoresFurtherCommands()
        {
            var grid = new SpaceGrid(1, 1);
            var robot = new Robot(new Coordinate(1, 1), Direction.N);

            robot.Execute(RobotCommands.GetCommand('F'), grid);   // falls off the top edge
            robot.Execute(RobotCommands.GetCommand('L'), grid);   // must be ignored
            robot.Execute(RobotCommands.GetCommand('F'), grid);   // must be ignored

            Assert.True(robot.IsLost);
            Assert.Equal(Direction.N, robot.Direction);
            Assert.Equal(new Coordinate(1, 1), robot.Position);
            Assert.True(grid.HasScent(new Coordinate(1, 1)));
        }

        [Fact]
        public void Simulator_StopsProcessingAfterRobotIsLost()
        {
            var input = InstructionParser.Parse("1 1\n1 1 N\nFLLF");   // F=lost, LLF must be ignored
            var result = new RobotSimulator().Execute(input).Single();

            Assert.True(result.IsLost);
            Assert.Equal(Direction.N, result.Direction);
            Assert.Equal(new Coordinate(1, 1), result.Position);
        }

        [Fact]
        public void ScentPreventsSecondRobotFromFallingOffSameEdge()
        {
            var input = InstructionParser.Parse("1 1\n1 1 N\nF\n1 1 N\nFL");
            var results = new RobotSimulator().Execute(input).ToArray();

            Assert.True(results[0].IsLost);
            Assert.False(results[1].IsLost);
            Assert.Equal(Direction.W, results[1].Direction);   // F ignored, L still applied
            Assert.Equal(new Coordinate(1, 1), results[1].Position);
        }

        [Fact]
        public void Robots_AreProcessedSequentially_ScentPersistsAcrossRobots()
        {
            // Robot 1 falls off at (0,0) facing S; robots 2 and 3 both survive the same edge.
            var input = InstructionParser.Parse("2 2\n0 0 S\nF\n0 0 S\nF\n0 0 S\nFF");
            var results = new RobotSimulator().Execute(input);

            Assert.Equal([true, false, false], results.Select(r => r.IsLost));
        }
    }
   
}
