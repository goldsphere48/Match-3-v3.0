using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class SpriteRenderSystem : AEntitySystem<float>
    {
        private readonly SpriteBatch _batch;

        public SpriteRenderSystem(SpriteBatch batch, EntitySet set)
            : base(set)
        {
            _batch = batch;
        }

        protected override void PreUpdate(float state) => _batch.Begin();

        protected override void Update(float state, in Entity entity)
        {
            var component = entity.Get<SpriteRenderer>();
            _batch.Draw(
                component.Texture, 
                component.Destination, 
                new Rectangle(0, 0, component.Destination.Width, component.Destination.Height), 
                component.Color, 
                component.Angle, 
                component.Origin, 
                SpriteEffects.None, 
                0
            );
        }

        protected override void PostUpdate(float state) => _batch.End();
    }
}
