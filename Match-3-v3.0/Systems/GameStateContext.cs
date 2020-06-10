using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Components.States;
using Match_3_v3._0.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class GameStateContext
    {
        public bool IsEnabled { get; set; }
        private State _currentState;
        private World _world;
        private Entity _stateHandler;

        public GameStateContext(World world)
        {
            _world = world;
            _world.Subscribe(this);
            _stateHandler = _world.CreateEntity();
        }

        public void SetState(State newState)
        {
            _currentState = newState;
            _stateHandler.Set(newState);
        }

        private void On(NewStateMessage newStateMessage)
        {
            _currentState.Handle(this, newStateMessage.Value);
        }
    }
}
