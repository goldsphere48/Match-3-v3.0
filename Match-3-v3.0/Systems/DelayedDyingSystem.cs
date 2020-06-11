using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(DelayedDying))]
    [With(typeof(DelayedDying))]
    class DelayedDyingSystem : AEntitySystem<float>
    {
        private World _world;

        public DelayedDyingSystem(World world)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
        }

        protected override void Update(float state, ReadOnlySpan<Entity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Remove<DelayedDying>();
                entity.Set<Dying>();
            }
        }
    }
}
