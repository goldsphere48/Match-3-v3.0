﻿using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.Systems
{
    [With(typeof(TextRenderer))]
    [With(typeof(Text))]
    internal class TextRenderSystem : AEntitySystem<float>
    {
        private readonly SpriteBatch _batch;

        public TextRenderSystem(SpriteBatch batch, World world)
            : base(world)
        {
            _batch = batch;
        }

        protected override void PostUpdate(float state) => _batch.End();

        protected override void PreUpdate(float state) => _batch.Begin();

        protected override void Update(float state, in Entity entity)
        {
            var renderer = entity.Get<TextRenderer>();
            var text = entity.Get<Text>();
            var position = new Vector2(renderer.Destination.X, renderer.Destination.Y);
            _batch.DrawString(renderer.SpriteFont, text.Value, position, renderer.Color);
        }
    }
}