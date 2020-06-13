using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(CountPresenter))]
    [WhenChanged(typeof(CountPresenter))]
    [With(typeof(CountPresenter))]
    [With(typeof(Text))]
    internal class CounterSystem : AEntitySystem<float>
    {
        public CounterSystem(World world)
            : base(world)
        {
        }

        protected override void Update(float state, in Entity entity)
        {
            var text = entity.Get<Text>();
            var counter = entity.Get<CountPresenter>();
            text.Value = counter.ToString();
            entity.Set(text);
        }
    }
}