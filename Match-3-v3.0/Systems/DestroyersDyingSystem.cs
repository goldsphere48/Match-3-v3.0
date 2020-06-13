using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;

namespace Match_3_v3._0.Systems
{
    [WhenRemoved(typeof(TargetPosition))]
    [With(typeof(Destroyer))]
    internal class DestroyersDyingSystem : AEntitySystem<float>
    {
        private readonly World _world;

        public DestroyersDyingSystem(World world)
            : base(world)
        {
            _world = world;
        }

        protected override void Update(float state, in Entity entity)
        {
            if (!entity.Has<Dying>())
            {
                entity.Set<Dying>();
            }
        }
    }
}