using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.ResourceManagers
{
    internal class SpriteFontResourceManager : AResourceManager<string, SpriteFont>
    {
        private readonly GraphicsDevice _device;
        private readonly IResourceLoader<string, SpriteFont> _loader;

        public SpriteFontResourceManager(GraphicsDevice device, IResourceLoader<string, SpriteFont> loader)
        {
            _device = device;
            _loader = loader;
        }

        protected override SpriteFont Load(string info) => _loader.Load(_device, info);

        protected override void OnResourceLoaded(in Entity entity, string info, SpriteFont resource)
        {
            var text = entity.Get<Text>();
            var size = resource.MeasureString(text.Value);
            entity.Get<TextRenderer>().SpriteFont = resource;
            entity.Get<TextRenderer>().Destination = new Rectangle(0, 0, (int)size.X, (int)size.Y);
        }
    }
}