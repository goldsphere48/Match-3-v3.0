using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class InputSystem : AEntitySystem<float>
    {
        private readonly GameWindow _window;

        protected MouseState _state;
        protected MouseState _oldState;

        public InputSystem(EntitySet set, GameWindow window)
            : base(set)
        {
            _window = window;
        }

        protected override void PreUpdate(float state)
        {
            _state = Mouse.GetState(_window);
        }

        protected override void PostUpdate(float state)
        {
            _oldState = _state;
        }
    }
}
