using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SceneSystem
{
    internal class SceneController : IScene
    {
        public Type SceneType => _scene.GetType();

        private readonly Scene _scene;
        private bool _enabled = true;
        private bool _disposed = false;

        public SceneController(Type sceneType, Game game)
        {
            _scene = CreateScene(sceneType, game);
        }

        public bool IsInitialized { get; set; }
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
    }
}
