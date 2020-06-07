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
    [WhenAdded(typeof(Count))]
    [WhenChanged(typeof(Count))]
    [With(typeof(Count))]
    [With(typeof(Text))]
    class CounterSystem : AEntitySystem<float>
    {
        public CounterSystem(World world)
            : base(world)
        {

        }

        protected override void Update(float state, in Entity entity) 
        {
            var text = entity.Get<Text>();
            var score = entity.Get<Count>();
            text.Value = text.Value + score.Value;
            entity.Set(text);
        }
    }
}
