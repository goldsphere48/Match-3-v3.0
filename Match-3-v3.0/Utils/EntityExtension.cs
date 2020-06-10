using DefaultEcs;
using Match_3_v3._0.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Utils
{
    static class EntityExtension
    {
        public static string ToString(this Entity entity)
        {
            if (entity.Has<Cell>())
            {
                return entity.Get<Cell>().ToString();
            }
            return "Not cell";
        }
    }
}
