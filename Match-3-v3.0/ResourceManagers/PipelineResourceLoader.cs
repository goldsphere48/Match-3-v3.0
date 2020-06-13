using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.ResourceManagers
{
    internal class PipelineResourceLoader<TResource> : IResourceLoader<string, TResource>
    {
        private readonly ContentManager _content;

        public PipelineResourceLoader(ContentManager content)
        {
            _content = content;
        }

        public TResource Load(GraphicsDevice device, string info) => _content.Load<TResource>(info);
    }
}