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
    class SwapSystem : AEntitySystem<float>
    {
        private Entity _selectedEntity;

        public SwapSystem(World world)
            : base(world)
        {

        }

        protected override void Update(float state, in Entity entity)
        {
            if (_selectedEntity == null)
            {
                _selectedEntity = entity;
            } else if (_selectedEntity == entity)
            {

            }
            else
            {
                TrySwap(entity, _selectedEntity);
            }
        }

        private void TrySwap(Entity first, Entity second)
        {

        }
    }
}
