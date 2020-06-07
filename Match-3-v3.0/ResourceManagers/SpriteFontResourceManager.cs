using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.ResourceManagers
{
    class SpriteFontResourceManager : AResourceManager<string, SpriteFont>
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
            entity.Get<TextRenderer>().SpriteFont = resource;
        }
    }
}
