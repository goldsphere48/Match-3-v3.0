using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.EntityFactories;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using System.Linq;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(Dying))]
    [With(typeof(LineBonus))]
    internal class LineBonusSystem : AEntitySystem<float>
    {
        private readonly DestroyersPool _destroyersPool;
        private readonly World _world;

        public LineBonusSystem(World world)
            : base(world)
        {
            _world = world;
            _destroyersPool = new DestroyersPool(new DestroyerFactory(world));
        }

        protected override void Update(float state, in Entity cellEntity)
        {
            var gridTransform = _world.First(e => e.Has<Grid>()).Get<Transform>();
            var lineBonus = cellEntity.Get<LineBonus>();
            var cell = lineBonus.Reference.Get<Cell>();
            _destroyersPool.RequestDestroyer(cell.PositionInGrid, lineBonus.FirstDirection, gridTransform);
            _destroyersPool.RequestDestroyer(cell.PositionInGrid, lineBonus.SecondDirection, gridTransform);
            Clear(cellEntity);
            _world.Publish(new NewStateMessage { Value = GameState.DestroyersMoving });
        }

        private void Clear(Entity cellEntity)
        {
            cellEntity.Remove<SpriteRenderer>();
            cellEntity.Remove<LineBonus>();
        }
    }
}