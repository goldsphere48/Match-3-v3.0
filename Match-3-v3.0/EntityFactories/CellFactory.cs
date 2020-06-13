using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.EntityFactories
{
    class CellFactory
    {
        private readonly World _world;
        private readonly int _cellSize;

        public CellFactory(World world, int cellSize)
        {
            _world = world;
            _cellSize = cellSize;
        }

        public Entity Create(Cell cellInfo, Transform parent)
        {
            var entity = _world.CreateEntity();
            entity.Set(new Transform { Parent = parent });
            entity.Set(cellInfo);
            entity.Set(new FrameAnimation
            {
                FrameCount = 5,
                AnimationSpeed = 0.1f,
                IsLooping = true
            });
            entity.Set(new ManagedResource<string, Texture2D>(cellInfo.Color.ToString()));
            return entity;
        }

        private Vector2 CalculateCellPosition(Vector2 positionInGrid)
        {
            return Vector2.Multiply(positionInGrid, _cellSize);
        }
    }
}
