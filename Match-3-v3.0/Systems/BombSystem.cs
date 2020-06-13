using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(Dying))]
    [With(typeof(BombBonus))]
    [With(typeof(Cell))]
    class BombSystem : AEntitySystem<float>
    {
        private Dictionary<Point, Entity> _cells;
        private readonly EntitySet _cellsSet;

        public BombSystem(World world)
            : base(world)
        {
            _cellsSet = world.GetEntities().With<Cell>().AsSet();
        }

        protected override void Update(float state, in Entity entity)
        {
            var width = PlayerPrefs.Get<int>("Width");
            var height = PlayerPrefs.Get<int>("Height");
            _cells = GridUtil.CellsSetToDictionary(_cellsSet, width, height);
            var cellPosition = entity.Get<Cell>().PositionInGrid;
            BlowUp(cellPosition, width, height);
            Clear(entity);
        }

        private void BlowUp(Point bombPosition, int width, int height)
        {
            Thread.Sleep(250);
            foreach (var position in GetExplosionZone(bombPosition, width, height))
            {
                if (_cells.TryGetValue(position, out var entity) && bombPosition != position)
                {
                    CellUtil.Kill(entity);
                }
            }
        }

        private IEnumerable<Point> GetExplosionZone(Point position, int width, int height)
        {
            for (int i = Start(position.X); i <= End(position.X, width); ++i)
            {
                for (int j = Start(position.Y); j <= End(position.Y, height); ++j)
                {
                    yield return new Point(i, j);
                }
            }
        }

        private int Start(int value)
        {
            return value - 1 >= 0 ? value - 1 : 0;
        }

        private int End(int value, int width)
        {
            return value + 1 < width ? value + 1 : width - 1;
        }

        private void Clear(Entity entity)
        {
            entity.Remove<SpriteRenderer>();
            entity.Remove<BombBonus>();
        }
    }
}
