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
            var difference = Vector2.Subtract(first.PositionInGrid, second.PositionInGrid);
            var length = difference.Length();
            return length == 1;
        }
    }
}
