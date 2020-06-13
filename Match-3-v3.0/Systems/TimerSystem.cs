using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;

namespace Match_3_v3._0.Systems
{
    internal class TimerSystem : AComponentSystem<float, Timer>
    {
        public TimerSystem(World world)
            : base(world)
        {
        }

        protected override void Update(float state, ref Timer component)
        {
            component.CurrentTime += state;
            if (component.CurrentTime >= component.Interval)
            {
                component.CurrentTime = 0;
                component.TimerTick?.Invoke();
            }
        }
    }
}