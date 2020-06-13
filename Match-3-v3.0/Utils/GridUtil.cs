using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
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

        public static Point[][] GetFullGridMatrix(int width, int height)
        {
            var positions = new Point[width][];
            for (int i = 0; i < width; ++i)
            {
                positions[i] = new Point[height];
                for (int j = 0; j < height; ++j)
                {
                    positions[i][j] = new Point(i, j);
                }
            }
            return positions;
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

            if (x > 0 && y > 0)
            {
                mask |= Neighbours.BottomLeft;
            }

            if (y > 0)
            {
                mask |= Neighbours.Bottom;
            }

            if (x < width - 1 && y > 0)
            {
                mask |= Neighbours.BottomRight;
            }

            if (x < width - 1)
            {
                mask |= Neighbours.Right;
            }

            if (x < width - 1 && y < height - 1)
            {
                mask |= Neighbours.TopRight;
            }

            if (y < height - 1)
            {
                mask |= Neighbours.Top;
            }

            if (y < height - 1 && x > 0)
            {
                mask |= Neighbours.TopLeft;
            }
            return mask;
        }

        public static void Generate(World world, Point[][] positions, int verticalOffset)
        {
            world.First(e => e.Has<Grid>()).Set(
                new GenerationZone
                {
                    NewCellPositionsInGrid = positions,
                    VerticalOffset = verticalOffset,
                    IsSecondaryGeneration = true
                }
            );
            world.Publish(new NewStateMessage { Value = GameState.Generating });
        }

        public static Dictionary<Point, Entity> CellsSetToDictionary(EntitySet cells, int width, int height)
        {
            var result = new Dictionary<Point, Entity>(width * height);
            foreach (var cell in cells.GetEntities())
            {
                result.Add(cell.Get<Cell>().PositionInGrid, cell);
            }
            return result;
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
