using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Match_3_v3._0.EntityFactories
{
    internal class GridFactory : IDisposable
    {
        private readonly Texture2D _background;
        private readonly int _cellSize;
        private readonly GraphicsDevice _device;
        private readonly int _height;
        private readonly int _width;
        private readonly World _world;

        public GridFactory(World world, GraphicsDevice device, int width, int height, int cellSize)
        {
            _world = world;
            _device = device;
            _width = width;
            _height = height;
            _background = CreateTexture();
            _cellSize = cellSize;
        }

        public Entity Create()
        {
            var entity = _world.CreateEntity();
            entity.Set(
                new SpriteRenderer
                {
                    Texture = _background,
                    Destination = new Rectangle(0, 0, _background.Width * _cellSize, _background.Height * _cellSize)
                }
            );
            var position = Vector2.Add(SceneUtil.GetCenterFor(entity, _device), new Vector2(0, 30));
            entity.Set(new Transform { Position = position });
            entity.Set(new Grid(_width, _height));
            entity.Set(new GenerationZone { NewCellPositionsInGrid = GridUtil.GetFullGridMatrix(_width, _height), VerticalOffset = 0 });
            return entity;
        }

        public void Dispose()
        {
            _background.Dispose();
        }

        private Texture2D CreateTexture()
        {
            var texture = new Texture2D(_device, _width, _height);
            var data = new Color[_width * _height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                data[pixel] = Color.Black * 0.7f;
            }
            texture.SetData(data);
            return texture;
        }
    }
}