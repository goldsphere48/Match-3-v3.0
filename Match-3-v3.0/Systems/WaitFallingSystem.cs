using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;

namespace Match_3_v3._0.Systems
{
    internal class WaitFallingSystem : ISystem<float>
    {
        private readonly EntitySet _fallingEntities;
        private readonly World _world;
        private GameState _gameState;
        public bool IsEnabled { get; set; }

        public WaitFallingSystem(World world, GameState _initState)
        {
            _world = world;
            _fallingEntities = _world.GetEntities().With(typeof(TargetPosition)).AsSet();
            _gameState = _initState;
            _world.Subscribe(this);
        }

        public void Dispose()
        {
        }

        public void Update(float state)
        {
            if (_gameState == GameState.WaitForFalling)
            {
                if (_fallingEntities.Count == 0)
                {
                    _world.Publish(new NewStateMessage { Value = GameState.Matching });
                }
            }
        }

        [Subscribe]
        private void On(in NewStateMessage newStateMessage) => _gameState = newStateMessage.Value;
    }
}