using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class WaitFallingSystem : ISystem<float>
    {
        public bool IsEnabled { get; set; }
        private readonly EntitySet _fallingEntities;
        private readonly World _world;
        private GameState _gameState;

        public WaitFallingSystem(World world, GameState _initState)
        {
            _world = world;
            _fallingEntities = _world.GetEntities().With(typeof(TargetPosition)).AsSet();
            _gameState = _initState;
            _world.Subscribe(this);
        }

        [Subscribe]
        private void On(in NewStateMessage newStateMessage) => _gameState = newStateMessage.Value;

        public void Dispose() { }

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
    }
}
