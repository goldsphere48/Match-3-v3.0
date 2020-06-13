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
            var component = entity.Get<FrameAnimation>();
            if (component.Play)
            {
                component.CurrentState += state;
                if (component.CurrentState >= component.AnimationSpeed)
                {
                    component.CurrentState = 0;
                    if (component.CurrentFrame + 1 == component.FrameCount)
                    {
                        component.CurrentFrame = 0;
                        if (!component.IsLooping)
                        {
                            component.Play = false;
                        }
                    }
                    else
                    {
                        component.CurrentFrame++;
                    }
                }
            }
        }
    }
}