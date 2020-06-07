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
    class TextFactory
    {
        private World _world;

        public TextFactory(World world)
        {
            _world = world;
        }

        public void Create(string fontName, string text, Vector2 position)
        {
            var entity = _world.CreateEntity();
            entity.Set(new TextRenderer());
            entity.Set(new ManagedResource<string, SpriteFont>(fontName));
            entity.Set(new Text(text));
            entity.Set(new Transform { Position = position });
        }
    }
}
