using MartianRobots.Commands;
using MartianRobots.Models;

namespace MartianRobotsTest;

public class RobotTests
{
    private static void Run(Robot robot, SpaceGrid grid, string commands)
    {
        foreach (var c in commands)
            robot.Execute(RobotCommands.GetCommand(c), grid);
    }

    [Fact]
    public void Forward_MovesOneSquare_WhenInsideGrid()
    {
        var grid = new SpaceGrid(5, 3);
        var robot = new Robot(new Coordinate(1, 1), Direction.E);

        Run(robot, grid, "F");

        Assert.Equal(new Coordinate(2, 1), robot.Position);
        Assert.False(robot.IsLost);
    }

    [Fact]
    public void Forward_OffEdge_MarksLostAndLeavesScent()
    {
        var grid = new SpaceGrid(5, 3);
        var robot = new Robot(new Coordinate(5, 3), Direction.N);

        Run(robot, grid, "F");

        Assert.True(robot.IsLost);
        Assert.Equal(new Coordinate(5, 3), robot.Position);      // last valid square
        Assert.True(grid.HasScent(new Coordinate(5, 3)));
    }

    [Fact]
    public void Forward_OffEdge_FromScentedSquare_IsIgnored()
    {
        var grid = new SpaceGrid(5, 3);
        grid.AddScent(new Coordinate(5, 3));
        var robot = new Robot(new Coordinate(5, 3), Direction.N);

        Run(robot, grid, "F");

        Assert.False(robot.IsLost);
        Assert.Equal(new Coordinate(5, 3), robot.Position);
    }

    [Fact]
    public void Scent_OnlyProtectsTheSameSquare()
    {
        var grid = new SpaceGrid(5, 3);
        grid.AddScent(new Coordinate(5, 3));
        var robot = new Robot(new Coordinate(4, 3), Direction.N);   // adjacent, unscented

        Run(robot, grid, "F");

        Assert.True(robot.IsLost);
    }

    [Fact]
    public void LostRobot_IgnoresFurtherCommands()
    {
        var grid = new SpaceGrid(1, 1);
        var robot = new Robot(new Coordinate(1, 1), Direction.N);

        Run(robot, grid, "FLF");        // F = lost; L and F must be ignored

        Assert.True(robot.IsLost);
        Assert.Equal(Direction.N, robot.Direction);
        Assert.Equal(new Coordinate(1, 1), robot.Position);
    }
}