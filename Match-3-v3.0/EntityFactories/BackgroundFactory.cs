using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.EntityFactories
{
    class BackgroundFactory
    {
        private readonly World _world;

        public BackgroundFactory(World world)
        {
            _world = world;
        }

        public Entity Create(string spriteName, Color? color = null)
        {
            var entity = _world.CreateEntity();
            entity.Set(new Transform());
            entity.Set(new SpriteRenderer { Color = color.HasValue ? color.Value : Color.White });
            entity.Set(new ManagedResource<string, Texture2D>(spriteName));
            return entity;
        }
    }
}
