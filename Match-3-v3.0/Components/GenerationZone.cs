using Microsoft.Xna.Framework;

namespace Match_3_v3._0.Components
{
    internal struct GenerationZone
    {
        public bool IsSecondaryGeneration { get; set; }
        public Point[][] NewCellPositionsInGrid { get; set; }
        public float VerticalOffset { get; set; }
    }
}