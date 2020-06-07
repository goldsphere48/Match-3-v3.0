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
            var windowCenter = new Vector2(device.PresentationParameters.BackBufferWidth / 2,
                  device.PresentationParameters.BackBufferHeight / 2);
            var spriteSize = GetSpriteSize(entity);
            var position = Vector2.Subtract(windowCenter, Vector2.Divide(spriteSize, 2));
            return position;
        }

        private static Vector2 GetSpriteSize(Entity entity)
        {
            if (entity.Has<SpriteRenderer>())
            {
                return entity.Get<SpriteRenderer>().Destination.Size.ToVector2();
            }
            else if (entity.Has<TextRenderer>())
            {
                return entity.Get<TextRenderer>().Destination.Size.ToVector2();
            }
            else
            {
                throw new Exception("Require SpriteRenderer component");
            }
        }
    }
}
