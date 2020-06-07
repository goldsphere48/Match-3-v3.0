using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.TextureManager
{
    class PipelineTextureLoader : ITextureLoader<string>
    {
        private ContentManager _content;

        public PipelineTextureLoader(ContentManager content)
        {
            _content = content;    
        }

        public Texture2D Load(GraphicsDevice device, string info)
        {
            return _content.Load<Texture2D>(info);
        }
    }
}
