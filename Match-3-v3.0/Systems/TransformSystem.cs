using DefaultEcs;
using DefaultEcs.Resource;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Match_3_v3._0.Components;
using Match_3_v3._0.TextureManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class TransformSystem : AEntitySystem<float>
    {
        public TransformSystem(World world, IParallelRunner runner)
            : base(world.GetEntities().WhenAdded<Transform>().WhenChanged<Transform>().With<SpriteRenderer>().AsSet(), runner)
        {

        }

        protected override void Update(float state, in Entity entity)
        {
            Vector2 position = entity.Get<Transform>().Position;
            ref SpriteRenderer renderer = ref entity.Get<SpriteRenderer>();

            renderer.Destination.X = (int)position.X;
            renderer.Destination.Y = (int)position.Y;
        }
    }
}
