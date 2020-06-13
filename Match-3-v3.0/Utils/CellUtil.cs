using DefaultEcs;
using Match_3_v3._0.Components;

namespace Match_3_v3._0.Utils
{
    internal static class CellUtil
    {
        public static bool IsBonus(Entity entity) => entity.Has<LineBonus>() || entity.Has<BombBonus>();

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