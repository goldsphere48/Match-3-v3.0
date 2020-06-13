using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SceneSystem
{
    public sealed class SceneManager : ISceneManager
    {
        private static SceneManager _instance;
        private readonly List<SceneController> _scenes = new List<SceneController>();
        private SceneController _currentScene;
        private Game _game;
        public static SceneManager Instance => _instance ?? (_instance = new SceneManager());

        public IScene CurrentScene => _currentScene;

        private SceneManager()
        {
        }

        public void Clear()
        {
            foreach (var scene in _scenes)
            {
                scene.Dispose();
            }
            _scenes.Clear();
        }

        public void Initialize(Game game)
        {
            _game = game;
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
                if (!scene.IsInitialized)
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
            if (!scene.IsInitialized)
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
            else if (!IsScene(sceneType))
            {
                throw new ArgumentException($"{sceneType.Name} is not subclass of IScene");
            }
            else if (!Contains(sceneType))
            {
                throw new ArgumentException($"Scene {sceneType.Name} scene doesn't exist");
            }
        }

        public void Update(float elapsedTime)
        {
            _currentScene.Update(elapsedTime);
        }

        private static bool IsScene(Type type) => typeof(IScene).IsAssignableFrom(type);

        private bool Contains(Type sceneType)
        {
            return _scenes.Any(sceneController => sceneType == sceneController.SceneType);
        }

        private SceneController GetSceneController(Type type)
        {
            return _scenes.Find(sceneController => sceneController.SceneType == type);
        }

        private SceneController GetSceneController<T>() where T : IScene
        {
            return GetSceneController(typeof(T));
        }
    }
}