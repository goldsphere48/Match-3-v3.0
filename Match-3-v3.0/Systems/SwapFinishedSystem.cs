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
    [With(typeof(SwapSuccess))]
    [Without(typeof(OriginalPosition))]
    [WhenRemoved(typeof(TargetPosition))]
    class SwapFinishedSystem : AEntitySystem<float>
    {
        private readonly World _world;

        public SwapFinishedSystem(World world)
            : base(world)
        {
            _world = world;
        }

        protected override void Update(float state, in Entity entity)
        {
            var success = entity.Get<SwapSuccess>();
            GameState newState;
            if (success.Value == SwapResult.Success)
            {
                newState = GameState.Matching;
            } else
            {
                _world.Publish(new UnselectMessage());
                newState = GameState.WaitForUserInput;
            }
            entity.Remove<SwapSuccess>();
            _world.Publish(new NewStateMessage { Value = newState });
        }
    }
}
