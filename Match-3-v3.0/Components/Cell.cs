﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components
{
    enum CellColor
    {
        Gold,
        Green,
        Blue,
        Brown,
        Purple
    }

    class Cell
    {
        public Point PositionInGrid { get; set; }
        public CellColor Color { get; set; }
        public override string ToString()
        {
            return Color.ToString();
        }
    }
}
