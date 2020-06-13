using System;

namespace SceneSystem
{
    internal interface ISceneManager
    {
        IScene CurrentScene { get; }

        void Clear();

        void LoadScene<T>() where T : IScene;

        void LoadScene(Type sceneType);

        void SetActiveScene<T>() where T : IScene;

        void SetActiveScene(Type sceneType);

        void UnloadScene<T>() where T : IScene;

        void UnloadScene(Type sceneType);

        void Update(float elapsedTime);
    }
}