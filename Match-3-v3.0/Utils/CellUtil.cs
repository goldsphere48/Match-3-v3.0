using DefaultEcs;
using Match_3_v3._0.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Utils
{
    static class CellUtil
    {
        public static bool IsBonus(Entity entity)
        {
            return entity.Has<LineBonus>() || entity.Has<BombBonus>();
        }

        public static void Kill(Entity entity)
        {
            if (IsBonus(entity))
            {
                entity.Set<DelayedDying>();
            }
            else
            {
                if (!entity.Has<Dying>() && entity.IsEnabled())
                {
                    entity.Set<Dying>();
                }
            }
        }
    }
}
