using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MartianRobots.Models
{
    public class SpaceGrid
    {
        private int spaceGridMaxX = 50;
        private int spaceGridMaxY = 50;

        public int MaxX { get; private set; }
        public int MaxY { get; private set; }

        private readonly HashSet<Coordinate> _scents = new();

        public bool IsOutOfBounds(Coordinate coordinate) =>
          coordinate.x < 0 || coordinate.x > MaxX || coordinate.y < 0 || coordinate.y > MaxY;

        public bool HasScent(Coordinate coordinate) => _scents.Contains(coordinate);

        public void AddScent(Coordinate coordinate) => _scents.Add(coordinate);

        public SpaceGrid(int maxX, int maxY)
        {

            if (maxX < 0 || maxY < 0)
            {
                throw new ArgumentException("Grid dimensions must be non-negative.");
            }
            if (maxX > spaceGridMaxX || maxY > spaceGridMaxY)
            {
                throw new ArgumentException($"Grid dimensions must not exceed {spaceGridMaxX}x{spaceGridMaxY}.");
            }

            MaxX = maxX;
            MaxY = maxY;
        }

    }
}
