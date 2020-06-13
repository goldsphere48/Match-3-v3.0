using DefaultEcs;
using Match_3_v3._0.Data;

namespace Match_3_v3._0.Components
{
    internal struct LineBonus
    {
        public Direction FirstDirection;
        public Entity Reference;
        public Direction SecondDirection;

        public LineBonus(Direction firstDirection, Direction secondDirection, Entity reference)
        {
            FirstDirection = firstDirection;
            SecondDirection = secondDirection;
            Reference = reference;
        }
    }
}