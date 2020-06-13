using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.EntityFactories;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Match_3_v3._0.Utils
{
    internal class DestroyersPool
    {
        private readonly DestroyerFactory _destroyerFactory;
        private readonly List<Entity?> data;

        public DestroyersPool(DestroyerFactory factory)
        {
            _destroyerFactory = factory;
            data = new List<Entity?>();
        }

        public Entity RequestDestroyer(Point spawnPositionInGrid, Direction direction, Transform parent)
        {
            Entity? destroyerEntity = data.Find(e => !e.Value.IsEnabled());
            if (!destroyerEntity.HasValue)
            {
                destroyerEntity = _destroyerFactory.Create(spawnPositionInGrid, direction, parent);
                data.Add(destroyerEntity.Value);
            }
            else
            {
                Reset(destroyerEntity, direction);
            }
            destroyerEntity.Value.Set(new TargetPosition { Position = GetTargetPosition(direction, spawnPositionInGrid), UseLocalPosition = true });
            PlaceDestroyer(destroyerEntity, spawnPositionInGrid, parent);
            return destroyerEntity.Value;
        }

        private Vector2 GetTargetPosition(Direction direction, Point spawnPositionInGrid)
        {
            var cellSize = PlayerPrefs.Get<int>("CellSize");
            var width = PlayerPrefs.Get<int>("Width");
            var height = PlayerPrefs.Get<int>("Height");
            var targetPosition = spawnPositionInGrid.ToVector2() * cellSize;
            switch (direction)
            {
                case Direction.Up:
                    targetPosition.Y = -cellSize;
                    break;
                case Direction.Down:
                    targetPosition.Y = height * (cellSize + 1);
                    break;
                case Direction.Left:
                    targetPosition.X = -cellSize;
                    break;
                case Direction.Right:
                    targetPosition.X = width * (cellSize + 1);
                    break;
            }
            return targetPosition;
        }

        private void PlaceDestroyer(Entity? destroyerEntity, Point spawnPositionInGrid, Transform parent)
        {
            var destroyerTransform = new Transform();
            destroyerTransform.Parent = parent;
            destroyerTransform.LocalPosition = spawnPositionInGrid.ToVector2() * PlayerPrefs.Get<int>("CellSize");
            destroyerTransform.Origin = new Vector2(40, 40);
            destroyerEntity.Value.Set(destroyerTransform);
        }

        private void Reset(Entity? destroyerEntity, Direction direction)
        {
            destroyerEntity.Value.Enable();
            var destroyer = destroyerEntity.Value.Get<Destroyer>();
            destroyer.Direction = direction;
            destroyerEntity.Value.Set(destroyer);
        }
    }
}