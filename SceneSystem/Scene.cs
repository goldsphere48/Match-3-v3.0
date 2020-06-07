using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneSystem
{
    public abstract class Scene : IScene
    {
        protected Game _game;
        protected SpriteBatch _batch;

        public Scene()
        {

        }

        internal void Initialize(Game game)
        {
            _game = game;
            _batch = new SpriteBatch(_game.GraphicsDevice);
        }

        public abstract void Setup();
        public abstract void Update(float elapsedTime);

        public virtual void OnEnable()
        {
            
        }
        public virtual void OnDisable()
        {

        }
        public virtual void Dispose()
        {
            _batch.Dispose();
        }
    }
}
