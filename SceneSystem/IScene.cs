using System;

namespace SceneSystem
{
    public interface IScene : IDisposable
    {
        void OnDisable();

        void OnEnable();

        void Setup();

        void Update(float elapsedTime);
    }
}