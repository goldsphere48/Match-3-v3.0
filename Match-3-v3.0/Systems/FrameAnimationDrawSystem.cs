using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.Systems
{
    [With(typeof(FrameAnimation))]
    internal class FrameAnimationDrawSystem : AEntitySystem<float>
    {
        private readonly SpriteBatch _batch;

        public FrameAnimationDrawSystem(SpriteBatch batch, World world)
            : base(world)
        {
            _batch = batch;
        }

        protected override void PostUpdate(float state) => _batch.End();

        protected override void PreUpdate(float state) => _batch.Begin();

        protected override void Update(float state, in Entity entity)
        {
            var renderer = entity.Get<FrameAnimation>();
            _batch.Draw(
                renderer.Texture,
                renderer.Destination,
                new Rectangle(
                    renderer.CurrentFrame * renderer.FrameWidth,
                    0,
                    renderer.FrameWidth,
                    renderer.FrameHeight
                ),
                renderer.Color
            );
        }
    }
}