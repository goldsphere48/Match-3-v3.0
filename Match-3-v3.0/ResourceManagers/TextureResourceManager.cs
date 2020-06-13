using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.ResourceManagers
{
    internal class TextureResourceManager : AResourceManager<string, Texture2D>
    {
        private readonly GraphicsDevice _device;
        private readonly IResourceLoader<string, Texture2D> _loader;

        public TextureResourceManager(GraphicsDevice device, IResourceLoader<string, Texture2D> loader)
        {
            _device = device;
            _loader = loader;
        }

        protected override Texture2D Load(string info) => _loader.Load(_device, info);

        protected override void OnResourceLoaded(in Entity entity, string info, Texture2D resource)
        {
            if (entity.Has<SpriteRenderer>())
            {
                entity.Get<SpriteRenderer>().Texture = resource;
                entity.Get<SpriteRenderer>().Destination = new Rectangle(0, 0, resource.Width, resource.Height);
            }
            else if (entity.Has<FrameAnimation>())
            {
                entity.Get<FrameAnimation>().Texture = resource; ;
            }
        }
    }
}