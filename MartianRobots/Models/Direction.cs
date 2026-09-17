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
      
    public static Coordinate Step(this Direction d, Coordinate pos) => d switch
    {
        Direction.N => pos with { Y = pos.Y + 1 },
        Direction.E => pos with { X = pos.X + 1 },
        Direction.S => pos with { Y = pos.Y - 1 },
        Direction.W => pos with { X = pos.X - 1 },
                        _ => throw new ArgumentOutOfRangeException(nameof(d), d, "Unknown direction")
    };

  

    public static Coordinate GetNextPosition(this Direction d, Coordinate pos) => d switch
    {
        Direction.N => pos with { Y = pos.Y + 1 },
        Direction.E => pos with { X = pos.X + 1 },
        Direction.S => pos with { Y = pos.Y - 1 },
        Direction.W => pos with { X = pos.X - 1 },
        _ => throw new ArgumentOutOfRangeException(nameof(d), d, "Unknown direction")
    };
}

