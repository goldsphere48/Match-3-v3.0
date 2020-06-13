using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Threading;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(Dying))]
    [With(typeof(BombBonus))]
    [With(typeof(Cell))]
    internal class BombSystem : AEntitySystem<float>
    {
        private readonly EntitySet _cellsSet;
        private Dictionary<Point, Entity> _cellDictionary;

        public BombSystem(World world)
            : base(world)
        {
            _cellsSet = world.GetEntities().With<Cell>().AsSet();
        }

        protected override void Update(float state, in Entity cellEntity)
        {
            var width = PlayerPrefs.Get<int>("Width");
            var height = PlayerPrefs.Get<int>("Height");
            _cellDictionary = GridUtil.CellsSetToDictionary(_cellsSet, width, height);
            var cellPosition = cellEntity.Get<Cell>().PositionInGrid;
            BlowUp(cellPosition, width, height);
            Clear(cellEntity);
        }

        private void BlowUp(Point bombPosition, int width, int height)
        {
            Thread.Sleep(250);
            foreach (var cellPosition in GetExplosionZone(bombPosition, width, height))
            {
                if (_cellDictionary.TryGetValue(cellPosition, out var cellEntity) && bombPosition != cellPosition)
                {
                    CellUtil.Kill(cellEntity);
                }
            }
        }

        private void Clear(Entity cellEntity)
        {
            cellEntity.Remove<SpriteRenderer>();
            cellEntity.Remove<BombBonus>();
        }

        private int End(int value, int width)
        {
            return value + 1 < width ? value + 1 : width - 1;
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
    }
}