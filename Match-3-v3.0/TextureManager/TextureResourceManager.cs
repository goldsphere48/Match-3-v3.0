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

namespace Match_3_v3._0.TextureManager
{
    class TextureResourceManager : AResourceManager<string, Texture2D>
    {
        private readonly GraphicsDevice _device;
        private readonly ITextureLoader<string> _loader;

        public TextureResourceManager(GraphicsDevice device, ITextureLoader<string> loader)
        {
            _device = device;
            _loader = loader;
        }

        protected override Texture2D Load(string info) => _loader.Load(_device, info);

        protected override void OnResourceLoaded(in Entity entity, string info, Texture2D resource)
        {
            entity.Get<SpriteRenderer>().Sprite = resource;
            entity.Get<SpriteRenderer>().Destination = new Rectangle(0, 0, resource.Width, resource.Height);
        }
    }
}
