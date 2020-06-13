using DefaultEcs;
using Match_3_v3._0.Components;

namespace Match_3_v3._0.Utils
{
    internal static class CellUtil
    {
        public static bool IsBonus(Entity cellEntity) => cellEntity.Has<LineBonus>() || cellEntity.Has<BombBonus>();

        public static void Kill(Entity cellEntity)
        {
            if (IsBonus(cellEntity))
            {
                cellEntity.Set<DelayedDying>();
            }
            else
            {
                if (!cellEntity.Has<Dying>() && cellEntity.IsEnabled())
                {
                    cellEntity.Set<Dying>();
                }
            }
        }
    }
}