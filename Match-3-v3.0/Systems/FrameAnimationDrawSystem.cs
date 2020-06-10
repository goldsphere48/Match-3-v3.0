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
    [With(typeof(FrameAnimation))]
    class FrameAnimationDrawSystem : AEntitySystem<float>
    {
        private SpriteBatch _batch;

        public FrameAnimationDrawSystem(SpriteBatch batch, World world)
            : base(world)
        {
            _batch = batch;
        }

        protected override void PreUpdate(float state)
        {
            _batch.Begin();
        }

        protected override void Update(float state, in Entity entity)
        {
            var component = entity.Get<FrameAnimation>();
            _batch.Draw(
                component.Texture, 
                component.Destination,
                new Rectangle(
                    component.CurrentFrame * component.FrameWidth,
                    0,
                    component.FrameWidth,
                    component.FrameHeight
                ),
                component.Color
            );
        }

        protected override void PostUpdate(float state)
        {
            _batch.End();
        }
    }
}
