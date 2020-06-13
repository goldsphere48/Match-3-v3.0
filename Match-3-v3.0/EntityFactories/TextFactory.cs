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
    struct TextArgs
    {
        public string FontName { get; set; }
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Color? Color { get; set; }
    }

    class TextFactory
    {
        private readonly World _world;

        public TextFactory(World world)
        {
            _world = world;
        }

        public Entity Create(TextArgs args)
        {
            var entity = _world.CreateEntity();
            entity.Set(new Text(args.Text));
            entity.Set(new TextRenderer { Color = args.Color.HasValue ? args.Color.Value : Color.White });
            entity.Set(new ManagedResource<string, SpriteFont>(args.FontName));
            entity.Set(new Transform { Position = args.Position });
            return entity;
        }
    }
}
