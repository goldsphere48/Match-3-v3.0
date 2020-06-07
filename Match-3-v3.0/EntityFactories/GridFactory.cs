using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.EntityFactories
{
    class GridFactory : IDisposable
    {
        private World _world;
        private GraphicsDevice _device;
        private Texture2D _background;
        private readonly int _width;
        private readonly int _height;
        private readonly int _cellSize = 81;

        public GridFactory(World world, GraphicsDevice device, int width, int height)
        {
            _world = world;
            _device = device;
            _width = width;
            _height = height;
            _background = CreateTexture();
        }

        public Entity Create()
        {
            var entity = _world.CreateEntity();
            entity.Set(
                new SpriteRenderer 
                { 
                    Sprite = _background, 
                    Destination = new Rectangle(0, 0, _background.Width * _cellSize, _background.Height * _cellSize) 
                }
            );
            var position = Vector2.Add(SceneUtil.GetCenterFor(entity, _device), new Vector2(0, 30));
            entity.Set(new Transform { Position = position});
            return entity;
        }

        private Texture2D CreateTexture()
        {
            Texture2D texture = new Texture2D(_device, _width, _height);
            Color[] data = new Color[_width * _height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                data[pixel] = Color.Black * 0.7f;
            }
            texture.SetData(data);
            return texture;
        }

        public void Dispose()
        {
            _background.Dispose();
        }
    }
}
