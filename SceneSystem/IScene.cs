using DefaultEcs;
using DefaultEcs.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneSystem
{
    public interface IScene : IDisposable
    {
        void Setup();
        void OnEnable();
        void OnDisable();
        void Update(float elapsedTime);
    }
}
