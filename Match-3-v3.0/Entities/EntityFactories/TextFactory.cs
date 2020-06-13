using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.EntityFactories
{
    internal struct TextArgs
    {
        public Color? Color { get; set; }
        public string FontName { get; set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }
    }

    internal class TextFactory
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