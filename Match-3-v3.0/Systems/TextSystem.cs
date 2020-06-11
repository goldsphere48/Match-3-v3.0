using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class TextSystem : AEntitySystem<float>
    {
        public TextSystem(World world)
            : base(world.GetEntities().WhenAdded<Text>().WhenChanged<Text>().With<Text>().With<TextRenderer>().AsSet())
        {

        }

        protected override void Update(float state, in Entity entity)
        {
            var text = entity.Get<Text>();
            var renderer = entity.Get<TextRenderer>();
            var oldDestionation = renderer.Destination;
            var textMessure = renderer.SpriteFont.MeasureString(text.Value);
            renderer.Destination = new Rectangle(oldDestionation.X, oldDestionation.Y, (int)textMessure.X, (int)textMessure.Y);
        }

    }
}
