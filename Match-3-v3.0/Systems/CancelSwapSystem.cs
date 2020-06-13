using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using System;

namespace Match_3_v3._0.Systems
{
    [With(typeof(OriginalPosition))]
    [With(typeof(FrameAnimation))]
    [WhenRemoved(typeof(TargetPosition))]
    internal class CancelSwapSystem : AEntitySystem<float>
    {
        public CancelSwapSystem(World world)
            : base(world)
        {
        }

        protected override void Update(float state, ReadOnlySpan<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var originalPosition = entity.Get<OriginalPosition>();
                entity.Set(new TargetPosition { Position = originalPosition.Value });
                entity.Remove<OriginalPosition>();
            }
        }
    }
}