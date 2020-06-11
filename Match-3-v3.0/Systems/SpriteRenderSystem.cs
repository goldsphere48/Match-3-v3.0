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
        private SpriteBatch _batch;
        private int _id;

        public SpriteRenderSystem(SpriteBatch batch, EntitySet set, int id = 0)
            : base(set)
        {
            _batch = batch;
            _id = id;
        }

        protected override void PreUpdate(float state)
        {
            _batch.Begin();
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_id == 228)
            {
                Console.WriteLine("Render destroyer");
            }
            var component = entity.Get<SpriteRenderer>();
            _batch.Draw(
                component.Sprite, 
                component.Destination, 
                new Rectangle(0, 0, component.Destination.Width, component.Destination.Height), 
                component.Color, 
                component.Angle, 
                component.Origin, 
                SpriteEffects.None, 
                0
            );
        }

        protected override void PostUpdate(float state)
        {
            _batch.End();
        }
    }
}
