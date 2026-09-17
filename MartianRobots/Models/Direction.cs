using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobots.Models;

public enum Direction { N, E, S, W }

public static class DirectionExtensions
{
    public static Direction TurnLeft(this Direction d) => d switch
    {
        Direction.N => Direction.W,
        Direction.W => Direction.S,
        Direction.S => Direction.E,
        Direction.E => Direction.N,
        _ => throw new ArgumentOutOfRangeException(nameof(d), d, "Unknown direction")
    };

    public static Direction TurnRight(this Direction d) => d switch
    {
        Direction.N => Direction.E,
        Direction.E => Direction.S,
        Direction.S => Direction.W,
        Direction.W => Direction.N,
        _ => throw new ArgumentOutOfRangeException(nameof(d), d, "Unknown direction")
    };
      
    public static Coordinate GetNextPosition(this Direction d, Coordinate from)
    {
        return d switch
        {
            Direction.N => new Coordinate(from.X, from.Y + 1),
            Direction.E => new Coordinate(from.X + 1, from.Y),
            Direction.S => new Coordinate(from.X, from.Y - 1),
            Direction.W => new Coordinate(from.X - 1, from.Y),
            _ => from
        };
    }
}

