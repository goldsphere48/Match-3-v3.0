using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    [WhenRemoved(typeof(TargetPosition))]
    [With(typeof(Destroyer))]
    class DestroyersDyingSystem : AEntitySystem<float>
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
