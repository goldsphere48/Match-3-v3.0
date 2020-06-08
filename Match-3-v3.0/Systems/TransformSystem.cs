using DefaultEcs;
using DefaultEcs.Resource;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Match_3_v3._0.Components;
using Match_3_v3._0.ResourceManagers;
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
            : base(world.GetEntities().WhenAdded<Transform>().WhenChanged<Transform>().WithEither<SpriteRenderer>().Or<TextRenderer>().Or<FrameAnimation>().AsSet(), runner)
        {

        }

        protected override void Update(float state, in Entity entity)
        {
            Vector2 position = entity.Get<Transform>().Position;
            HandleRenderer<SpriteRenderer>(entity, position);
            HandleRenderer<TextRenderer>(entity, position);
            HandleRenderer<FrameAnimation>(entity, position);
        }

        private void HandleRenderer<T>(Entity entity, Vector2 position) where T : RendererComponent
        {
            if (entity.Has<T>())
            {
                T renderer = entity.Get<T>();
                renderer.Destination.X = (int)position.X;
                renderer.Destination.Y = (int)position.Y;
            }
        }

    }
}
