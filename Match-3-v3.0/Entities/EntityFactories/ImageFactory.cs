using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.EntityFactories
{
    internal class ImageFactory
    {
        private readonly World _world;

        public ImageFactory(World world)
        {
            _world = world;
        }

        public Entity Create(string spriteName, Color? color = null)
        {
            var entity = _world.CreateEntity();
            entity.Set(new Transform());
            entity.Set(new SpriteRenderer { Color = color ?? Color.White });
            entity.Set(new ManagedResource<string, Texture2D>(spriteName));
            return entity;
        }
    }
}