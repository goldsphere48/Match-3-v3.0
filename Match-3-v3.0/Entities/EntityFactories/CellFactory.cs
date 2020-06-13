using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.EntityFactories
{
    internal class CellFactory
    {
        private readonly int _cellSize;
        private readonly World _world;

        public CellFactory(World world, int cellSize)
        {
            _world = world;
            _cellSize = cellSize;
        }

        public Entity Create(Cell cell, Transform parent)
        {
            var cellEntity = _world.CreateEntity();
            cellEntity.Set(new Transform { Parent = parent });
            cellEntity.Set(cell);
            cellEntity.Set(new FrameAnimation
            {
                FrameCount = 5,
                AnimationSpeed = 0.1f,
                IsLooping = true
            });
            cellEntity.Set(new ManagedResource<string, Texture2D>(cell.Color.ToString()));
            return cellEntity;
        }

        private Vector2 CalculateCellPosition(Vector2 positionInGrid)
        {
            return Vector2.Multiply(positionInGrid, _cellSize);
        }
    }
}