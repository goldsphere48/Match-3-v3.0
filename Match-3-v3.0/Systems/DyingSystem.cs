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
    [WhenAdded(typeof(Dying))]
    [WhenChanged(typeof(Dying))]
    [With(typeof(Dying))]
    class DyingSystem : AEntitySystem<float>
    {
        private World _world;
        private GameState _gameState = GameState.Generating;

        public DyingSystem(World world, GameState initState)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
            _gameState = initState;
        }

        [Subscribe]
        private void On(in NewStateMessage newState)
        {
            _gameState = newState.Value;
        }

        protected override void Update(float state, ReadOnlySpan<Entity> entities)
        {
            if (_gameState == GameState.CellDestroying || _gameState == GameState.DestroyersMoving)
            {
                var score = 0;
                foreach (var entity in entities)
                {
                    entity.Remove<Dying>();
                    entity.Disable();
                    score++;
                }
                _world.Publish(new AddScoreMessage { Value = score });
                if (_gameState == GameState.CellDestroying)
                {
                    _world.Publish(new NewStateMessage { Value = GameState.Falling });
                }
            }
        }
    }
}
