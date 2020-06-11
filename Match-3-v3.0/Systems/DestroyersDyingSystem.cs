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
        private World _world;
        private EntitySet _destroyers;

        public DestroyersDyingSystem(World world)
            : base(world)
        {
            _world = world;
            _destroyers = _world.GetEntities().With<Destroyer>().AsSet();
        }

        protected override void Update(float state, in Entity entity)
        {
            entity.Disable();
            if (_destroyers.Count == 0)
            {
                _world.Publish(new NewStateMessage { Value = GameState.Falling });
            }
        }
    }
}
