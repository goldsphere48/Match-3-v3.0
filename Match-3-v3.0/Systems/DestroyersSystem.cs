using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Match_3_v3._0.Systems
{
    [With(typeof(Destroyer))]
    internal class DestroyersSystem : AEntitySystem<float>
    {
        private readonly World _world;
        private Dictionary<Point, Entity> _cellDictionary;
        private GameState _gameState;

        public DestroyersSystem(World world, GameState initState)
            : base(world)
        {
            _gameState = initState;
            _world = world;
            _world.Subscribe(this);
        }

        protected override void Update(float state, ReadOnlySpan<Entity> destroyers)
        {
            if (_gameState == GameState.DestroyersMoving)
            {
                foreach (var destroyer in destroyers)
                {
                    var destroyerDirection = destroyer.Get<Destroyer>().Direction;
                    var destroyerLocalPosition = destroyer.Get<Transform>().LocalPosition;
                    var positionIngrid = GetCurrentPositinInGrid(destroyerLocalPosition, destroyerDirection);
                    TryToDestroyCell(positionIngrid, destroyerLocalPosition, destroyerDirection);
                }
            }
        }

        private int CalculatePartialPosition(float value, Direction direction)
        {
            int partialPosition;
            if (direction == Direction.Left || direction == Direction.Up)
            {
                partialPosition = (int)Math.Ceiling(value / PlayerPrefs.Get<int>("CellSize"));
            }
            else
            {
                partialPosition = (int)Math.Floor(value / PlayerPrefs.Get<int>("CellSize"));
            }
            return partialPosition;
        }

        private void DestroyCellUnderDestroyerWithCondition(Entity cellEntity, Cell cell, Func<bool> canDestroy)
        {
            if (canDestroy() && !cellEntity.Has<Dying>())
            {
                CellUtil.Kill(cellEntity);
                _cellDictionary.Remove(cell.PositionInGrid);
            }
        }

        private Point GetCurrentPositinInGrid(Vector2 destroyerLocalPosition, Direction direction)
        {
            int posX = CalculatePartialPosition(destroyerLocalPosition.X, direction);
            int posY = CalculatePartialPosition(destroyerLocalPosition.Y, direction);
            return new Point(posX, posY);
        }

        [Subscribe]
        private void On(in NewStateMessage newStateMessage)
        {
            _gameState = newStateMessage.Value;
            if (_gameState == GameState.DestroyersMoving)
            {
                _cellDictionary = GridUtil.CellsSetToDictionary(
                    _world.GetEntities().With<Cell>().AsSet(),
                    PlayerPrefs.Get<int>("Width"),
                    PlayerPrefs.Get<int>("Height")
                );
            }
        }

        private void TryToDestroyCell(Point cellPosition, Vector2 destroyerLocalPosition, Direction destroyerDirection)
        {
            if (_cellDictionary.TryGetValue(cellPosition, out var nearestCellEntity))
            {
                var cell = nearestCellEntity.Get<Cell>();
                var cellLocalPosition = cell.PositionInGrid.ToVector2() * PlayerPrefs.Get<int>("CellSize");
                switch (destroyerDirection)
                {
                    case Direction.Up:
                        DestroyCellUnderDestroyerWithCondition(nearestCellEntity, cell, () => destroyerLocalPosition.Y <= cellLocalPosition.Y);
                        break;
                    case Direction.Down:
                        DestroyCellUnderDestroyerWithCondition(nearestCellEntity, cell, () => destroyerLocalPosition.Y >= cellLocalPosition.Y);
                        break;
                    case Direction.Left:
                        DestroyCellUnderDestroyerWithCondition(nearestCellEntity, cell, () => destroyerLocalPosition.X <= cellLocalPosition.X);
                        break;
                    case Direction.Right:
                        DestroyCellUnderDestroyerWithCondition(nearestCellEntity, cell, () => destroyerLocalPosition.X >= cellLocalPosition.X);
                        break;
                }
            }
        }
    }
}