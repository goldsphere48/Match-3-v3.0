using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
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
    class ButtonSystem : AEntitySystem<float>
    {
        private readonly GameWindow _window;

        private MouseState _state;
        private MouseState _oldState;

        public ButtonSystem(World world, GameWindow window)
            : base(world.GetEntities().With<Button>().With<SpriteRenderer>().AsSet())
        {
            _window = window;
        }

        protected override void PreUpdate(float state)
        {
            _state = Mouse.GetState(_window);
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_state.LeftButton == ButtonState.Released && _oldState.LeftButton == ButtonState.Pressed)
            {
                var position = _state.Position.ToVector2();
                var renderer = entity.Get<SpriteRenderer>();
                if (renderer.Destination.Contains(position))
                {
                    entity.Get<Button>().Click?.Invoke();
                }
            }
        }

        protected override void PostUpdate(float state)
        {
            _oldState = _state;
        }
    }
}
