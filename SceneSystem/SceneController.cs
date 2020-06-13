using Microsoft.Xna.Framework;
using System;

namespace SceneSystem
{
    internal class SceneController : IScene
    {
        private readonly Scene _scene;
        private bool _disposed = false;
        private bool _enabled = true;
        public bool IsInitialized { get; set; }
        public Type SceneType => _scene.GetType();

        public SceneController(Type sceneType, Game game)
        {
            _scene = CreateScene(sceneType, game);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                if (_enabled)
                {
                    OnDisable();
                }
                _scene.Dispose();
            }
        }

        public void OnDisable()
        {
            _enabled = false;
            _scene.OnDisable();
        }

        public void OnEnable()
        {
            _enabled = true;
            _scene.OnEnable();
        }

        public void Setup()
        {
            _scene.Setup();
        }

        public void Update(float elapsedTime)
        {
            _scene.Update(elapsedTime);
        }

        private Scene CreateScene(Type type, Game game)
        {
            var scene = Activator.CreateInstance(type) as Scene;
            scene.Initialize(game);
            return scene;
        }
    }
}