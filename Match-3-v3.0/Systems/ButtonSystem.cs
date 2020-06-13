using DefaultEcs;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Match_3_v3._0.Systems
{
    internal class ButtonSystem : InputSystem
    {
        public ButtonSystem(World world, GameWindow window)
            : base(world.GetEntities().With<Button>().With<SpriteRenderer>().AsSet(), window)
        {
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_state.LeftButton == ButtonState.Released && _oldState.LeftButton == ButtonState.Pressed)
            {
                OnUserClick(state, entity);
            }
        }

        private void OnUserClick(float state, in Entity entity)
        {
            var position = _state.Position.ToVector2();
            var renderer = entity.Get<SpriteRenderer>();
            if (renderer.Destination.Contains(position))
            {
                entity.Get<Button>().Click?.Invoke();
            }
        }
    }
}