using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.EntityFactories;
using Match_3_v3._0.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(Generator))]
    [WhenChanged(typeof(Generator))]
    [With(typeof(Generator))]
    [With(typeof(Transform))]
    class GenerationSystem : AEntitySystem<float>
    {
        private CellPool _cellPool;

        public GenerationSystem(World world)
            : base(world)
        {
            _cellPool = new CellPool(new CellFactory(world, PlayerPrefs.Get<int>("CellSize")));
        }

        protected override void Update(float state, in Entity entity)
        {
            ref var generator = ref entity.Get<Generator>();
            var parentTransform = entity.Get<Transform>();
            foreach (var column in generator.NewCellPositionsInGrid)
            {
                foreach (var cellPosition in column)
                {
                    Entity cell = _cellPool.RequestCell(cellPosition, generator.VerticalOffset, parentTransform);
                    entity.SetAsParentOf(cell);
                }
            }
        }
    }
}
