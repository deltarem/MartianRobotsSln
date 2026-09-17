using MartianRobots.Models;

namespace MartianRobotsTest;

public class SpaceGridTests
{
    const int spaceGridMaxAllowed = 50;
    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 3)]   // top-right corner is ON the grid
    [InlineData(5, 0)]
    [InlineData(0, 3)]
    public void IsOutOfBounds_CornersAreInside(int x, int y)
    {
        var grid = new SpaceGrid(5, 3);
        Assert.False(grid.IsOutOfBounds(new Coordinate(x, y)));
    }

    [Theory]
    [InlineData(6, 0)]
    [InlineData(0, 4)]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void IsOutOfBounds_OneStepPastEdgeIsOutside(int x, int y)
    {
        var grid = new SpaceGrid(5, 3);
        Assert.True(grid.IsOutOfBounds(new Coordinate(x, y)));
    }

    [Fact]
    public void Scent_IsRememberedPerSquare()
    {
        var grid = new SpaceGrid(5, 3);
        grid.AddScent(new Coordinate(3, 3));

        Assert.True(grid.HasScent(new Coordinate(3, 3)));
        Assert.False(grid.HasScent(new Coordinate(3, 2)));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(spaceGridMaxAllowed + 1, 0)]
    [InlineData(0, spaceGridMaxAllowed + 1)]
    public void Constructor_RejectsInvalidSize(int x, int y) =>
        Assert.Throws<ArgumentException>(() => new SpaceGrid(x, y));

    [Fact]
    public void Constructor_AcceptsMaxAllowed() =>
        _ = new SpaceGrid(spaceGridMaxAllowed, spaceGridMaxAllowed);
}