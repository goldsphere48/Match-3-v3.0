using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.ResourceManagers
{
    internal interface IResourceLoader<TInfo, TResource>
    {
        TResource Load(GraphicsDevice device, TInfo info);
    }
}