using DefaultEcs;
using Match_3_v3._0.Data;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components
{
    struct LineBonus
    {
        public Direction FirstDirection;
        public Direction SecondDirection;
        public Entity Reference;

        public LineBonus(Direction firstDirection, Direction secondDirection, Entity reference)
        {
            FirstDirection = firstDirection;
            SecondDirection = secondDirection;
            Reference = reference;
        }
    }
}
