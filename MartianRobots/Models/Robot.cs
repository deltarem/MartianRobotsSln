using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

namespace MartianRobots.Models
{

    public class Robot
    {
        public Coordinate Position { get; private set; }
        public Direction Direction { get; private set; }

        public bool IsLost { get; private set; } = false;

        public Robot(Coordinate initPosition, Direction initDirection)
        {
            Position = initPosition;
            Direction = initDirection;
        }

        public void UpdateIsLost()
        {
            IsLost = true;
        }
        

        public void TurnLeft() => Direction = Direction.TurnLeft();
        public void TurnRight() => Direction = Direction.TurnRight();
        public Coordinate NextPosition() => Direction.GetNextPosition(Position);
        public void MoveTo(Coordinate position) => Position = position;
        public void MarkLost() => IsLost = true;

    }
}
