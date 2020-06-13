using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Data
{
    [Flags]
    enum Neighbours
    {
        None = 0,
        Top = 1 << 0,
        TopRight = 1 << 1,
        Right = 1 << 2,
        BottomRight = 1 << 3,
        Bottom = 1 << 4,
        BottomLeft = 1 << 5,
        Left = 1 << 6,
        TopLeft = 1 << 7
    }
}
