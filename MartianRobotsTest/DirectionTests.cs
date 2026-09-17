using MartianRobots.Models;

namespace MartianRobotsTest;

public class DirectionTests
{
    [Theory]
    [InlineData(Direction.N, Direction.W)]
    [InlineData(Direction.W, Direction.S)]
    [InlineData(Direction.S, Direction.E)]
    [InlineData(Direction.E, Direction.N)]
    public void TurnLeft_RotatesAnticlockwise(Direction from, Direction expected) =>
        Assert.Equal(expected, from.TurnLeft());

    [Theory]
    [InlineData(Direction.N, Direction.E)]
    [InlineData(Direction.E, Direction.S)]
    [InlineData(Direction.S, Direction.W)]
    [InlineData(Direction.W, Direction.N)]
    public void TurnRight_RotatesClockwise(Direction from, Direction expected) =>
        Assert.Equal(expected, from.TurnRight());

    [Theory]
    [InlineData(Direction.N, 0, 1)]
    [InlineData(Direction.E, 1, 0)]
    [InlineData(Direction.S, 0, -1)]
    [InlineData(Direction.W, -1, 0)]
    public void GetNextPosition_StepsOneSquare(Direction d, int dx, int dy)
    {
        var from = new Coordinate(2, 2);
        Assert.Equal(new Coordinate(2 + dx, 2 + dy), d.GetNextPosition(from));
    }

    [Fact]
    public void FourLeftTurns_ReturnToStart()
    {
        var d = Direction.N;
        for (var i = 0; i < 4; i++) d = d.TurnLeft();
        Assert.Equal(Direction.N, d);
    }
}