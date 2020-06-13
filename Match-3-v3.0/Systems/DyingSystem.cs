using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using System;

namespace Match_3_v3._0.Systems
{
    [With(typeof(Dying))]
    internal class DyingSystem : AEntitySystem<float>
    {
        private readonly EntitySet _destroyers;
        private readonly World _world;
        private GameState _gameState = GameState.Generating;

        public DyingSystem(World world, GameState initState)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
            _gameState = initState;
            _destroyers = _world.GetEntities().With<Destroyer>().AsSet();
        }

        protected override void Update(float state, ReadOnlySpan<Entity> entities)
        {
            var score = 0;
            foreach (var entity in entities)
            {
                if (entity.Has<Cell>())
                {
                    score++;
                }
                entity.Remove<Dying>();
                entity.Disable();
            }
            if (score > 0)
            {
                _world.Publish(new AddScoreMessage { Value = score });
            }
            if (_destroyers.Count == 0)
            {
                _world.Publish(new NewStateMessage { Value = GameState.Falling });
            }
        }

        [Subscribe]
        private void On(in NewStateMessage newState) => _gameState = newState.Value;
    }
}