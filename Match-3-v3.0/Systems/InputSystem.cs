using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Match_3_v3._0.Systems
{
    internal class InputSystem : AEntitySystem<float>
    {
        protected MouseState _oldState;
        protected MouseState _state;
        private readonly GameWindow _window;

        public InputSystem(EntitySet cellSet, GameWindow window)
            : base(cellSet)
        {
            _window = window;
        }

        protected override void PostUpdate(float state) => _oldState = _state;

        protected override void PreUpdate(float state) => _state = Mouse.GetState(_window);
    }
}