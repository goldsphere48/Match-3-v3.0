using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Utils
{
    static class GridUtil
    {
        public static bool IsNeighbours(Cell first, Cell second)
        {
            var difference = first.PositionInGrid - second.PositionInGrid;
            var length = difference.ToVector2().Length();
            return length == 1;
        }

        public static Neighbours GetNeighbours(Point positionInGrid, int width, int height)
        {
            int x = positionInGrid.X;
            int y = positionInGrid.Y;
            var mask = Neighbours.None;
            if (x > 0)
            {
                mask |= Neighbours.Left;
            }

            if (y > 0)
            {
                mask |= Neighbours.Bottom;
            }

            if (x < width - 1)
            {
                mask |= Neighbours.Right;
            }

            if (y < height - 1)
            {
                mask |= Neighbours.Top;
            }
            return mask;
        }

        public static Point NeighbourToVector2(Neighbours neighbours)
        {
            switch (neighbours)
            {
                case Neighbours.None:
                    return new Point();
                case Neighbours.Top:
                    return new Point(0, 1);
                case Neighbours.TopRight:
                    return new Point(1, 1);
                case Neighbours.Right:
                    return new Point(1, 0);
                case Neighbours.BottomRight:
                    return new Point(1, -1);
                case Neighbours.Bottom:
                    return new Point(0, -1);
                case Neighbours.BottomLeft:
                    return new Point(-1, -1);
                case Neighbours.Left:
                    return new Point(-1, 0);
                case Neighbours.TopLeft:
                    return new Point(-1, 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(neighbours), neighbours, null);
            }
        }
    }
}
