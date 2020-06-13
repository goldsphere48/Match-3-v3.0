using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SceneSystem
{
    public abstract class Scene : IScene
    {
        protected SpriteBatch _batch;
        protected Game _game;

        public Scene()
        {
        }

        public virtual void Dispose()
        {
            _batch.Dispose();
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnEnable()
        {
        }

        public abstract void Setup();

        public abstract void Update(float elapsedTime);

        internal void Initialize(Game game)
        {
            _game = game;
            _batch = new SpriteBatch(_game.GraphicsDevice);
        }
    }
}