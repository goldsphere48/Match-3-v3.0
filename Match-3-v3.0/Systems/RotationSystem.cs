using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using System;

namespace Match_3_v3._0.Systems
{
    [With(typeof(Transform))]
    [With(typeof(Rotation))]
    internal class RotationSystem : AEntitySystem<float>
    {
        public RotationSystem(World world)
            : base(world)
        {
        }

        protected override void Update(float state, ReadOnlySpan<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var transform = entity.Get<Transform>();
                transform.Angle += entity.Get<Rotation>().Speed * state;
                entity.Set(transform);
            }
        }
    }
}