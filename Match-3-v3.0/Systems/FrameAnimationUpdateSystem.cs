using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;

namespace Match_3_v3._0.Systems
{
    [With(typeof(FrameAnimation))]
    internal class FrameAnimationUpdateSystem : AEntitySystem<float>
    {
        public FrameAnimationUpdateSystem(World world)
            : base(world)
        {
        }

        protected override void Update(float state, in Entity entity)
        {
            var renderer = entity.Get<FrameAnimation>();
            if (renderer.Play)
            {
                renderer.CurrentState += state;
                if (renderer.CurrentState >= renderer.AnimationSpeed)
                {
                    renderer.CurrentState = 0;
                    if (renderer.CurrentFrame + 1 == renderer.FrameCount)
                    {
                        renderer.CurrentFrame = 0;
                        if (!renderer.IsLooping)
                        {
                            renderer.Play = false;
                        }
                    }
                    else
                    {
                        renderer.CurrentFrame++;
                    }
                }
            }
        }
    }
}