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
    [WhenAdded(typeof(Swap))]
    [With(typeof(Swap))]
    [With(typeof(Grid))]
    class SwapSystem : AEntitySystem<float>
    {
        public SwapSystem(World world)
            : base(world)
        {

        }

        protected override void Update(float state, in Entity entity)
        {
           
        }

        private void CheckMatch(Grid grid, Swap swap)
        {
            var firstCell = swap.First.Get<Cell>();
            var secondCell = swap.Second.Get<Cell>();
        }
    }
}
