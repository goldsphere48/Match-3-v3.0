using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(Dying))]
    [With(typeof(Dying))]
    class DyingSystem : AEntitySystem<float>
    {
        private CellPool _cellPool;
        private World _world;

        public DyingSystem(World world, CellPool cellPool)
            : base(world)
        {
            _cellPool = cellPool;
            _world = world;
        }

        protected override void Update(float state, ReadOnlySpan<Entity> entities)
        {
            var grid = _world.First(e => e.Has<Grid>()).Get<Grid>();
            foreach (var entity in entities)
            {
                var cell = entity.Get<Cell>().PositionInGrid;
                Console.WriteLine(cell);
                entity.Remove<Dying>();
                entity.Disable();
                entity.Dispose();
            }
        }
    }
}
