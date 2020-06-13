using Match_3_v3._0.Data;
using Microsoft.Xna.Framework;

namespace Match_3_v3._0.Components
{
    internal struct Cell
    {
        public CellColor Color;
        public Point PositionInGrid;

        public override string ToString()
        {
            return $"{Color.ToString()} | {PositionInGrid}";
        }
    }
}