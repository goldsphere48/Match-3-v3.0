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

        protected override void Update(float state, ref Timer timer)
        {
            timer.CurrentTime += state;
            if (timer.CurrentTime >= timer.Interval)
            {
                timer.CurrentTime = 0;
                timer.TimerTick?.Invoke();
            }
        }
    }
}