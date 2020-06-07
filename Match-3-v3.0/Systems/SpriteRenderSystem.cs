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
    class SpriteRenderSystem : AComponentSystem<float, SpriteRenderer>
    {
        private SpriteBatch _batch;

        public SpriteRenderSystem(SpriteBatch batch, World world)
            : base(world)
        {
            _batch = batch;
        }

        protected override void PreUpdate(float state)
        {
            _batch.Begin();
        }

        protected override void Update(float state, ref SpriteRenderer component)
        {
            _batch.Draw(component.Sprite, component.Destination, component.Color);
        }

        protected override void PostUpdate(float state)
        {
            _batch.End();
        }
    }
}
