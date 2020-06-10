﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components
{
    struct GenerationZone
    {
        public Point[][] NewCellPositionsInGrid { get; set; }
        public float VerticalOffset { get; set; }
        public bool IsSecondaryGeneration { get; set; }
    }
}
