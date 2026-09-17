using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobots.Models
{
    public record Coordinate(int X, int Y);
    public enum DirectionEnum
    {
        N,
        E,
        S,
        W
    }

    public sealed record GridSize(int X, int Y);
    public sealed record RobotInstruction(Coordinate Coordinate, DirectionEnum Direction, string Commands);
    public sealed record SimulationInput(GridSize Grid, IReadOnlyList<RobotInstruction> Robots);

    public sealed record RobotResult(Coordinate Position, DirectionEnum Direction, bool IsLost)
    {
        public override string ToString() =>
            $"{Position.X} {Position.Y} {Direction}" + (IsLost ? " LOST" : "");
    }

}
