using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneSystem
{
    interface ISceneManager
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
