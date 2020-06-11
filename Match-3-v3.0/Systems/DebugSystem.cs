using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class DebugSystem : InputSystem
    {
        private GameState _gameState;
        private World _world;
        private EntitySet _cells;

        public DebugSystem(World world, GameWindow window, GameState initState)
            : base(world.GetEntities().With<Grid>().AsSet(), window)
        {
            _gameState = initState;
            _world = world;
            _world.Subscribe(this);
            _cells = _world.GetEntities().With<Cell>().AsSet();
        }

        [Subscribe]
        private void On(in NewStateMessage newState)
        {
            _gameState = newState.Value;
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_gameState == GameState.WaitForUserInput)
            {
                var grid = entity.Get<Grid>();
                if (_state.RightButton == ButtonState.Released && _oldState.RightButton == ButtonState.Pressed)
                {
                    foreach (var cell in _cells.GetEntities())
                    {
                        cell.Set<Dying>();
                    }
                    _world.Publish(new NewStateMessage { Value = GameState.CellDestroying });
                }
            }
        }
    }
}
