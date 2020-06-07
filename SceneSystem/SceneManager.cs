using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneSystem
{
    public sealed class SceneManager : ISceneManager
    {
        public static SceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SceneManager();
                }
                return _instance;
            }
        }

        public void Initialize(Game game)
        {
            _game = game;
        }

        private SceneManager()
        {
        }

        private static SceneManager _instance;

        private Game _game;
        private SceneController _currentScene;
        private List<SceneController> _scenes = new List<SceneController>();

        public IScene CurrentScene => _currentScene;

        public void Clear()
        {
            foreach (var scene in _scenes)
            {
                scene.Dispose();
            }
            _scenes.Clear();
        }

        public void LoadScene<T>() where T : IScene
        {
            LoadScene(typeof(T));
        }

        public void LoadScene(Type sceneType)
        {
            if (IsScene(sceneType))
            {
                var scene = GetSceneController(sceneType);
                if (scene == null)
                {
                    scene = new SceneController(sceneType, _game);
                    _scenes.Add(scene);
                }
                if (scene.IsInitialized == false)
                {
                    scene.Setup();
                }
            }
            else
            {
                throw new ArgumentException($"{sceneType.Name} must be subclass of IScene");
            }
        }

        public void SetActiveScene<T>() where T : IScene
        {
            SetActiveScene(typeof(T));
        }

        public void SetActiveScene(Type sceneType)
        {
            var scene = GetSceneController(sceneType);
            if (scene == null)
            {
                LoadScene(sceneType);
                scene = GetSceneController(sceneType);
            }
            _currentScene?.OnDisable();
            _currentScene = scene;
            if (scene.IsInitialized == false)
            {
                scene.Setup();
            }
            scene.OnEnable();
        }

        public void UnloadScene<T>() where T : IScene
        {
            UnloadScene(typeof(T));
        }

        public void UnloadScene(Type sceneType)
        {
            if (IsScene(sceneType) && Contains(sceneType))
            {
                var scene = GetSceneController(sceneType);
                if (scene.IsInitialized)
                {
                    scene.Dispose();
                }
                _scenes.Remove(scene);
            }
            else if (IsScene(sceneType) == false)
            {
                throw new ArgumentException($"{sceneType.Name} is not subclass of IScene");
            }
            else if (Contains(sceneType) == false)
            {
                throw new ArgumentException($"Scene {sceneType.Name} scene doesn't exist");
            }
        }

        public void Update(float elapsedTime)
        {
            _currentScene.Update(elapsedTime);
        }

        private SceneController GetSceneController(Type type)
        {
            return _scenes.Find(sceneController => sceneController.SceneType == type);
        }

        private SceneController GetSceneController<T>() where T : IScene
        {
            return GetSceneController(typeof(T));
        }

        private static bool IsScene(Type type)
        {
            return typeof(IScene).IsAssignableFrom(type);
        }

        private bool Contains(Type sceneType)
        {
            return _scenes.Any(sceneController => sceneType == sceneController.SceneType);
        }
    }
}
