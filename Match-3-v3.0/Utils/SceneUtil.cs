using DefaultEcs;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Utils
{
    static class SceneUtil
    {
        public static Vector2 GetCenterFor(Entity entity, GraphicsDevice device)
        {
            if (entity.Has<SpriteRenderer>())
            {
                var windowCenter = new Vector2(device.PresentationParameters.BackBufferWidth / 2,
                   device.PresentationParameters.BackBufferHeight / 2);
                var spriteSize = entity.Get<SpriteRenderer>().Destination.Size.ToVector2();
                var position = Vector2.Subtract(windowCenter, Vector2.Divide(spriteSize, 2));
                return position;
            } else
            {
                throw new Exception("Require SpriteRenderer component");
            }
        }
    }
}
