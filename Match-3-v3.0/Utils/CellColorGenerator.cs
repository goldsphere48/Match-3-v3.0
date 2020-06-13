using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Utils
{
    static class CellColorGenerator
    {
        private static readonly Random _random = new Random();
        public static CellColor Get()
        {
            Array values = Enum.GetValues(typeof(CellColor));
            return (CellColor)values.GetValue(_random.Next(values.Length));
        }
    }
}
