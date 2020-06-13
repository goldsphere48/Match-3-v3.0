using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.Systems
{
    internal class SpriteRenderSystem : AEntitySystem<float>
    {
        private readonly SpriteBatch _batch;

        public SpriteRenderSystem(SpriteBatch batch, EntitySet cellSet)
            : base(cellSet)
        {
            _batch = batch;
        }

        protected override void PostUpdate(float state) => _batch.End();

        protected override void PreUpdate(float state) => _batch.Begin();

        protected override void Update(float state, in Entity entity)
        {
            var renderer = entity.Get<SpriteRenderer>();
            _batch.Draw(
                renderer.Texture,
                renderer.Destination,
                new Rectangle(0, 0, renderer.Destination.Width, renderer.Destination.Height),
                renderer.Color,
                renderer.Angle,
                renderer.Origin,
                SpriteEffects.None,
                0
            );
        }
    }
}