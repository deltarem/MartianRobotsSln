using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

namespace MartianRobots.Models
{
    public record Coordinate(int x, int y);
    public enum DirectionEnum
    {
        N,
        E,
        S,
        W
    }

    public class Robot
    {
        public Coordinate Position { get; private set; }
        public DirectionEnum Direction { get; private set; }

        public bool IsLost { get; private set; } = false;

        public Robot(Coordinate initPosition, DirectionEnum initDirection)
        {
            Position = initPosition;
            Direction = initDirection;
        }

        public void UpdateIsLost(bool isLost)
        {
            IsLost = isLost;
        }

        public void TurnLeft()
        {
            Direction = Direction switch
            {
                DirectionEnum.N => DirectionEnum.W,
                DirectionEnum.W => DirectionEnum.S,
                DirectionEnum.S => DirectionEnum.E,
                DirectionEnum.E => DirectionEnum.N,
                _ => Direction
            };
        }

        public void TurnRight()
        {
            Direction = Direction switch
            {
                DirectionEnum.N => DirectionEnum.E,
                DirectionEnum.E => DirectionEnum.S,
                DirectionEnum.S => DirectionEnum.W,
                DirectionEnum.W => DirectionEnum.N,
                _ => Direction
            };
        }

        public Coordinate GetNextPosition()
        {
            return Direction switch
            {
                DirectionEnum.N => new Coordinate(Position.x, Position.y + 1),
                DirectionEnum.E => new Coordinate(Position.x + 1, Position.y),
                DirectionEnum.S => new Coordinate(Position.x, Position.y - 1),
                DirectionEnum.W => new Coordinate(Position.x - 1, Position.y),
                _ => Position
            };
        }

        public void MoveTo(Coordinate newPosition) => Position = newPosition;


    }
}
