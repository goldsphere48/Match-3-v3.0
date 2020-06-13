using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;

namespace Match_3_v3._0.Systems
{
    [With(typeof(DelayedDying))]
    internal class DelayedDyingSystem : AEntitySystem<float>
    {
        private readonly World _world;

        public DelayedDyingSystem(World world)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
        }

        protected override void Update(float state, in Entity entity)
        {
            entity.Remove<DelayedDying>();
            if (!entity.Has<Dying>())
            {
                entity.Set<Dying>();
            }
        }
    }
}