using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.EntityFactories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Utils
{
    class DestroyersPool
    {
        private readonly DestroyerFactory _destroyerFactory;
        private readonly List<Entity?> _destroyers;

        public DestroyersPool(DestroyerFactory factory)
        {
            _destroyerFactory = factory;
            _destroyers = new List<Entity?>();
        }

        public Entity RequestDestroyer(Point spawnPositionInGrid, Direction direction, Transform parent)
        {
            Entity? destroyerEntity = _destroyers.Find(e => !e.Value.IsEnabled());
            if (!destroyerEntity.HasValue)
            {
                destroyerEntity = _destroyerFactory.Create(spawnPositionInGrid, direction, parent);
                _destroyers.Add(destroyerEntity.Value);
            }
            else
            {
                Reset(destroyerEntity, direction);
            }
            destroyerEntity.Value.Set(new TargetPosition { Position = GetTargetPosition(direction, spawnPositionInGrid), UseLocalPosition = true });
            PlaceDestroyer(destroyerEntity, spawnPositionInGrid, parent);
            return destroyerEntity.Value;
        }

        private void Reset(Entity? entity, Direction direction)
        {
            entity.Value.Enable();
            var component = entity.Value.Get<Destroyer>();
            component.Direction = direction;
            entity.Value.Set(component);
        }

        private void PlaceDestroyer(Entity? destroyer, Point spawnPositionInGrid, Transform parent)
        {
            var destroyerTransform = new Transform();
            destroyerTransform.Parent = parent;
            destroyerTransform.LocalPosition = spawnPositionInGrid.ToVector2() * PlayerPrefs.Get<int>("CellSize");
            destroyerTransform.Origin = new Vector2(40, 40);
            destroyer.Value.Set(destroyerTransform);
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
    }
}
