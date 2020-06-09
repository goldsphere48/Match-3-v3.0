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

        public DyingSystem(World world, CellPool cellPool)
            : base(world)
        {
            _cellPool = cellPool;
        }

        protected override void Update(float state, in Entity entity)
        {
            entity.Remove<Dying>();
            entity.Dispose();
        }
    }
}
