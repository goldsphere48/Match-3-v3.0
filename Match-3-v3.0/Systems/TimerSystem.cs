using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class TimerSystem : AComponentSystem<float, Timer>
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
