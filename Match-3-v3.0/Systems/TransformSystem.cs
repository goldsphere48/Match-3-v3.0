using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Match_3_v3._0.Components;

namespace Match_3_v3._0.Systems
{
    internal class TransformSystem : AEntitySystem<float>
    {
        public TransformSystem(World world, IParallelRunner runner)
            : base(
                  world.GetEntities()
                  .WithEither<SpriteRenderer>()
                  .Or<TextRenderer>()
                  .Or<FrameAnimation>()
                  .AsSet()
            )
        {
        }

        protected override void Update(float state, in Entity entity)
        {
            Transform transform = entity.Get<Transform>();
            HandleRenderer<SpriteRenderer>(entity, transform);
            HandleRenderer<TextRenderer>(entity, transform);
            HandleRenderer<FrameAnimation>(entity, transform);
        }

        private void HandleRenderer<T>(Entity entity, Transform transform) where T : RendererComponent
        {
            if (entity.Has<T>())
            {
                T renderer = entity.Get<T>();
                renderer.Destination.X = (int)(transform.Position.X + transform.Origin.X);
                renderer.Destination.Y = (int)(transform.Position.Y + transform.Origin.X);
                renderer.Angle = transform.Angle;
                renderer.Origin = transform.Origin;
            }
        }
    }
}