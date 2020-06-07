using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.EntityFactories
{
    class ButtonFactory
    {
        private World _world;
        private Vector2 _windowCenter;

        public ButtonFactory(World world, GraphicsDevice device)
        {
            _world = world;
            _windowCenter = new Vector2(device.PresentationParameters.BackBufferWidth / 2,
               device.PresentationParameters.BackBufferHeight / 2);
        }

        public Entity Create(string spriteName, Vector2 position, Action OnClick)
        {
            var entity = _world.CreateEntity();
            entity.Set(new SpriteRenderer());
            entity.Set(new ManagedResource<string, Texture2D>(spriteName));
            entity.Set(new Button { Click = OnClick });
            entity.Set(new Transform { Position = position });
            return entity;
        }

        public void CreateAtCenter(string spriteName, Action OnClick)
        {
            var entity = Create(spriteName, new Vector2(0, 0), OnClick);
            var spriteSize = entity.Get<SpriteRenderer>().Destination.Size.ToVector2();
            var position = Vector2.Subtract(_windowCenter, Vector2.Divide(spriteSize, 2));
            var transform = entity.Get<Transform>();
            transform.Position = position;
            entity.Set(transform);
        }
    }
}
