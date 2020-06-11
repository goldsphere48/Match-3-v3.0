using DefaultEcs;
using DefaultEcs.Resource;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.EntityFactories
{
    class DestroyerFactory
    {
        private World _world;

        public DestroyerFactory(World world)
        {
            _world = world;
        }

        public Entity Create(Point spawnPositionInGrid, Direction direction, Transform parent)
        {
            var entity = _world.CreateEntity();
            entity.Set(new SpriteRenderer());
            entity.Set(new ManagedResource<string, Texture2D>("Destroyer"));
            entity.Set(new Destroyer(direction));
            entity.Set(new Rotation { Speed = 20 });
            return entity;
        }
    }
}
