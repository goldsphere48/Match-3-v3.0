using Match_3_v3._0.Data;
using System;

namespace Match_3_v3._0.Utils
{
    internal static class CellColorGenerator
    {
        private static readonly Random _random = new Random();

        public static CellColor Get()
        {
            Array values = Enum.GetValues(typeof(CellColor));
            return (CellColor)values.GetValue(_random.Next(values.Length));
        }
    }
}